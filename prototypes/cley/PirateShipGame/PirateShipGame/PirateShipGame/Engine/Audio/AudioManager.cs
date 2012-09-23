﻿
#region Using Statements

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using PirateShipGame.Components.BaseComponents;
using PirateShipGame.Engine.Screens;

#endregion  // Using Statements

namespace PirateShipGame.Engine.Audio
{
    public class AudioManager : Component
    {
        #region Fields

        // XACT objects
        private AudioEngine audioEngine;
        private WaveBank    waveBank;
        private SoundBank   soundBank;

        /// <summary>
        /// Point that will be hearing the 3D sounds (camera).
        /// </summary>
        private AudioListener listener = new AudioListener();
        public AudioListener Listener
        {
            get { return listener; }
        }

        /// <summary>
        /// Source of a 3D sound.
        /// </summary>
        private AudioEmitter emitter = new AudioEmitter();

        /// <summary>
        /// Currently playing 3D sounds.
        /// </summary>
        private List<Cue3D> activeCues = new List<Cue3D>();

        /// <summary>
        /// Currently unused Cue3D containers.
        /// These are stored so they may be reused.
        /// </summary>
        private Stack<Cue3D> cuePool = new Stack<Cue3D>();

        #endregion  // Fields

        #region Accessors

        public AudioEngine AudioEngine
        {
            get { return audioEngine; }
        }

        public SoundBank SoundBank
        {
            get { return soundBank; }
        }

        public WaveBank WaveBank
        {
            get { return waveBank; }
        }

        #endregion

        public AudioManager(Screen parent) : base(parent)
        {
            this.Initialize();
        }

        /// <summary>
        /// Load XACT files. (AudioEngine, WaveBank, & SoundBank).
        /// </summary>
        public override void Initialize()
        {
            audioEngine = new AudioEngine("Content\\Audio\\Asplosions.xgs");
            waveBank = new WaveBank(audioEngine, "Content\\Audio\\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content\\Audio\\Sound Bank.xsb");

            base.Initialize();
        }

        /// <summary>
        /// Dispose of XACT objects.
        /// </summary>
        /// <param name="disposing">True if disposing.</param>
        public override void Dispose()
        {
            try
            {
                soundBank.Dispose();
                waveBank.Dispose();
                audioEngine.Dispose();
            }
            finally
            {
                base.Dispose();
            }
        }

        /// <summary>
        /// Update the 3D audio system.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            int index = 0;

            while (index < activeCues.Count)
            {
                Cue3D cue3D = activeCues[index];

                // If cue has stopped playing.
                if (cue3D.Cue.IsStopped)
                {
                    // Dispose.
                    cue3D.Cue.Dispose();

                    // Move the Cue3D instance to cuePool.
                    cuePool.Push(cue3D);

                    // Remove the Cue3D instance from list of active cues.
                    activeCues.RemoveAt(index);
                }
                // Else cue is still playing.
                else
                {
                    // Update 3D information of cue.
                    Apply3D(cue3D);

                    // Increment index.
                    index++;
                }
            }

            // Update XACT Audio Engine.
            audioEngine.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// Setup Cue3D & play cue.
        /// </summary>
        /// <param name="cueTitle">The cue to play from sound bank.</param>
        /// <param name="emitter">Source of the sound.</param>
        /// <returns>The playing cue.</returns>
        public Cue Play3DCue(string cueTitle, IAudioEmitter emitter)
        {
            Cue3D cue3D;

            //If cuePool is not empty.
            if (cuePool.Count > 0)
            {
                // Reuse Cue3D instance.
                cue3D = cuePool.Pop();
            }
            // Else create new Cue3D.
            else
            {
                cue3D = new Cue3D();
            }

            // Assign cue & audio emitter.
            cue3D.Cue = soundBank.GetCue(cueTitle);
            cue3D.Emitter = emitter;

            // Set 3D position of the new cue.
            Apply3D(cue3D);

            // Play
            cue3D.Cue.Play();

            // Store new Cue3D in active cues.
            activeCues.Add(cue3D);

            return cue3D.Cue;
        }

        /// <summary>
        /// Pauses all active cues.
        /// </summary>
        public void PauseAllCues()
        {
            for (int i = 0; i < activeCues.Count; i++)
            {
                activeCues[i].Cue.Pause();
            }
        }

        /// <summary>
        /// Resumes all active cues.
        /// </summary>
        public void ResumeAllCues()
        {
            for (int i = 0; i < activeCues.Count; i++)
            {
                activeCues[i].Cue.Resume();
            }
        }

        public void StopAllCues()
        {
            for (int i = 0; i < activeCues.Count; i++)
            {
                activeCues[i].Cue.Stop(AudioStopOptions.Immediate);
            }
        }

        /// <summary>
        /// Update position & velocity properties of a Cue3D
        /// </summary>
        /// <param name="cue3D">Cue3D to be updated.</param>
        private void Apply3D(Cue3D cue3D)
        {
            emitter.Position = cue3D.Emitter.Position;
            emitter.Forward = cue3D.Emitter.Forward;
            emitter.Up = cue3D.Emitter.Up;
            emitter.Velocity = cue3D.Emitter.Velocity;

            cue3D.Cue.Apply3D(listener, emitter);
        }

        /// <summary>
        /// Internal helper class for keeping track of an active 3D cue,
        /// and remembering which emitter object it is attached to.
        /// </summary>
        private class Cue3D
        {
            public Cue Cue;
            public IAudioEmitter Emitter;
        }
    }
}
