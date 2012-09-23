using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using PirateShipGame.Cameras;
using PirateShipGame.Objects;

namespace PirateShipGame
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GamePadState lastState = GamePad.GetState(PlayerIndex.One);

        //Camera/View information
        ChaseCamera camera = new ChaseCamera();

        //Audio components
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        // Cue so we can hang on to the sound of the engine.
        Cue engineSound = null;

        //Visual components
        Ship ship = new Ship();
        Model asteroidModel;
        Matrix[] asteroidTransforms;
        Asteroid[] asteroidList = new Asteroid[GameConstants.NumAsteroids];
        Random random = new Random();
        Model bulletModel;
        Matrix[] bulletTransforms;
        Bullet[] bulletList = new Bullet[GameConstants.NumBullets];
        Texture2D stars;
        SpriteFont lucidaConsole;
        int score;
        Vector2 scorePosition = new Vector2(100, 50);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            audioEngine = new AudioEngine("Content\\Audio\\MyGameAudio.xgs");
            waveBank = new WaveBank(audioEngine, "Content\\Audio\\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content\\Audio\\Sound Bank.xsb");

            camera.Initialize();
            camera.Target = ship;

            //TODO: Refactor Initialization
            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                asteroidList[i] = new Asteroid();
            }

            for (int i = 0; i < GameConstants.NumBullets; i++)
            {
                bulletList[i] = new Bullet();
            }

            ResetAsteroids();

            base.Initialize();
        }

        private Matrix[] SetupEffectDefaults(Model myModel)
        {
            Matrix[] absoluteTransforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(absoluteTransforms);

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Projection = camera.ProjectionMatrix;
                    effect.View = camera.ViewMatrix;
                }
            }
            return absoluteTransforms;
        }

        private void ResetAsteroids()
        {
            float xStart;
            float yStart;
            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                if (random.Next(2) == 0)
                {
                    xStart = (float)-GameConstants.PlayfieldSizeX;
                }
                else
                {
                    xStart = (float)GameConstants.PlayfieldSizeX;
                }
                yStart = (float)random.NextDouble() * GameConstants.PlayfieldSizeY;
                asteroidList[i].Position = new Vector3(xStart, yStart, 0.0f);
                double angle = random.NextDouble() * 2 * Math.PI;
                asteroidList[i].direction.X = -(float)Math.Sin(angle);
                asteroidList[i].direction.Y = (float)Math.Cos(angle);
                asteroidList[i].speed = GameConstants.AsteroidMinSpeed +
                   (float)random.NextDouble() * GameConstants.AsteroidMaxSpeed;
                asteroidList[i].isActive = true;
            }
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ship.Model = Content.Load<Model>("Models/p1_wedge");
            ship.Transforms = SetupEffectDefaults(ship.Model);
            asteroidModel = Content.Load<Model>("Models/asteroid1");
            asteroidTransforms = SetupEffectDefaults(asteroidModel);
            bulletModel = Content.Load<Model>("Models/pea_proj");
            bulletTransforms = SetupEffectDefaults(bulletModel);
            stars = Content.Load<Texture2D>("Textures/B1_stars");
            lucidaConsole = Content.Load<SpriteFont>("Fonts/Lucida Console");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Get some input.
            UpdateInput();

            // Update audioEngine.
            audioEngine.Update();

            // Add velocity to the current position.
            ship.Position += ship.Velocity;

            // Bleed off velocity over time.
            ship.Velocity *= 0.95f;

            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                asteroidList[i].Update(timeDelta);
            }
            for (int i = 0; i < GameConstants.NumBullets; i++)
            {
                if (bulletList[i].isActive)
                {
                    bulletList[i].Update(timeDelta);
                }
            }

            //bullet-asteroid collision check
            for (int i = 0; i < asteroidList.Length; i++)
            {
                if (asteroidList[i].isActive)
                {
                    BoundingSphere asteroidSphere =
                      new BoundingSphere(asteroidList[i].Position,
                               asteroidModel.Meshes[0].BoundingSphere.Radius *
                                     GameConstants.AsteroidBoundingSphereScale);
                    for (int j = 0; j < bulletList.Length; j++)
                    {
                        if (bulletList[j].isActive)
                        {
                            BoundingSphere bulletSphere = new BoundingSphere(
                              bulletList[j].Position,
                              bulletModel.Meshes[0].BoundingSphere.Radius);
                            if (asteroidSphere.Intersects(bulletSphere))
                            {
                                soundBank.PlayCue("explosion2");
                                asteroidList[i].isActive = false;
                                bulletList[j].isActive = false;
                                score += GameConstants.KillBonus;
                                break; //no need to check other bullets
                            }
                        }
                    }
                }
            }

            //ship-asteroid collision check
            if (ship.isActive)
            {
                BoundingSphere shipSphere = new BoundingSphere(
                    ship.Position, ship.Model.Meshes[0].BoundingSphere.Radius *
                                         GameConstants.ShipBoundingSphereScale);
                for (int i = 0; i < asteroidList.Length; i++)
                {
                    if (asteroidList[i].isActive)
                    {
                        BoundingSphere b = new BoundingSphere(asteroidList[i].Position,
                        asteroidModel.Meshes[0].BoundingSphere.Radius *
                        GameConstants.AsteroidBoundingSphereScale);
                        if (b.Intersects(shipSphere))
                        {
                            //blow up ship
                            soundBank.PlayCue("explosion3");
                            ship.isActive = false;
                            asteroidList[i].isActive = false;
                            score -= GameConstants.DeathPenalty;
                            break; //exit the loop
                        }

                    }
                }

            }
            camera.Update(gameTime);
            base.Update(gameTime);
        }


        protected void UpdateInput()
        {
            // Get the game pad state.
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            if (currentState.IsConnected)
            {
                ship.Update(currentState);

                // Set some audio based on whether we're pressing a trigger.
                if (currentState.Triggers.Right > 0)
                {
                    if (engineSound == null)
                    {
                        engineSound = soundBank.GetCue("engine_2");
                        engineSound.Play();
                    }

                    else if (engineSound.IsPaused)
                    {
                        engineSound.Resume();
                    }
                }
                else
                {
                    if (engineSound != null && engineSound.IsPlaying)
                    {
                        engineSound.Pause();
                    }
                }

                // In case you get lost, press B to warp back to the center.
                if (currentState.Buttons.B == ButtonState.Pressed &&
                    lastState.Buttons.B == ButtonState.Released)
                {
                    ship.Position = Vector3.Zero;
                    ship.Velocity = Vector3.Zero;
                    ship.Rotation = 0.0f;
                    ship.isActive = true;
                    score -= GameConstants.WarpPenalty;
                    // Make a sound when we warp.
                    soundBank.PlayCue("hyperspace_activate");
                }
            }
            //are we shooting?
            if (ship.isActive && currentState.Buttons.A == ButtonState.Pressed &&
                    lastState.Buttons.A == ButtonState.Released)
            {
                //add another bullet.  Find an inactive bullet slot and use it
                //if all bullets slots are used, ignore the user input
                for (int i = 0; i < GameConstants.NumBullets; i++)
                {
                    if (!bulletList[i].isActive)
                    {
                        bulletList[i].direction = ship.RotationMatrix.Forward;
                        bulletList[i].speed = GameConstants.BulletSpeedAdjustment;
                        bulletList[i].Position = ship.Position +
                  (200 * bulletList[i].direction);
                        bulletList[i].isActive = true;
                        score -= GameConstants.ShotPenalty;
                        soundBank.PlayCue("tx0_fire1");

                        break; //exit the loop     
                    }
                }
            }
            lastState = currentState;
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            //spriteBatch.Draw(stars, new Rectangle(0, 0, 800, 600), Color.White);
            spriteBatch.End();
            Matrix shipTransformMatrix = ship.RotationMatrix
                    * Matrix.CreateTranslation(ship.Position);
            if (ship.isActive)
            {
                DrawModel(ship.Model, shipTransformMatrix, ship.Transforms);
            }
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.DrawString(lucidaConsole, "Score: " + score,
                                   scorePosition, Color.LightGreen);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void DrawModel(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms)
        {
            //Draw the model, a model can have multiple meshes, so loop
            foreach (ModelMesh mesh in model.Meshes)
            {
                //This is where the mesh orientation is set
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.View = camera.ViewMatrix;
                    effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * modelTransform;
                }
                //Draw the mesh, will use the effects set above.
                mesh.Draw();
            }
        }
    }
}
