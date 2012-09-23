﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using PirateShipGame.Engine.Audio;
using PirateShipGame.Components.BaseComponents;

namespace PirateShipGame.Engine.Screens
{
    public class Screen : IAudioEmitter
    {
        #region Properties

        bool initialized;

        public bool Initialized
        {
            get { return initialized; }
            set { initialized = value; }
        }

        bool visible;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        bool enabled;

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        bool blocksUpdate;

        public bool BlocksUpdate
        {
            get { return blocksUpdate; }
            set { blocksUpdate = value; }
        }

        bool blocksDraw;

        public bool BlocksDraw
        {
            get { return blocksDraw; }
            set { blocksDraw = value; }
        }

        bool alwaysUpdate;

        public bool AlwaysUpdate
        {
            get { return alwaysUpdate; }
            set { alwaysUpdate = value; }
        }

        bool alwaysDraw;

        public bool AlwaysDraw
        {
            get { return alwaysDraw; }
            set { alwaysDraw = value; }
        }

        string name;

        public string Name
        {
            get { return name; }
        }

        

        #endregion

        //ScreenManager owner;
        ComponentCollection components;

        public ComponentCollection Components
        {
            get { return components; }
            set { components = value; }
        }

        public Screen(string name)
        {
            this.name = name;
            this.visible = true;
            this.enabled = true;

            components = new ComponentCollection(this);

            if(GameEngine.Initialized)
                GameEngine.GameScreens.Add(this);
        }

        public virtual void Initialize()
        {
            initialized = true;
        }

        /// <summary>
        /// Load graphics content for the screen.
        /// </summary>
        public virtual void LoadContent() { }


        /// <summary>
        /// Unload content for the screen.
        /// </summary>
        public virtual void UnloadContent() { }


        public virtual void Update(GameTime gameTime)
        {
            // Create a temporary list so we don't crash if
            // a component is added to the collection while
            // updating
            List<Component> updating = new List<Component>();

            // Populate the temporary list
            foreach (Component c in Components)
                updating.Add(c);

            // Update all components that have been initialized
            foreach (Component component in updating)
                if (component.Initialized && component.Enabled)
                    component.Update(gameTime);
        }

        public virtual void Draw()
        {
            // Temporary list
            List<DrawableComponent> drawing = new List<DrawableComponent>();
            List<DrawableComponent> drawList3D = new List<DrawableComponent>();
            List<DrawableComponent> drawListBillboard = new List<DrawableComponent>();
            List<DrawableComponent> drawListEffects = new List<DrawableComponent>();
            List<DrawableComponent> drawList2D = new List<DrawableComponent>();


            foreach (Component component in Components)
            {
                if (component is DrawableComponent)
                {
                    drawing.Add((DrawableComponent)component);
                }
            }

            DrawableComponent drawList;

            while (drawing.Count > 0)
            {
                drawList = drawing.First();
                drawing.Remove(drawList);

                if (drawList is I2DComponent)
                {
                    drawList2D.Add(drawList);
                }
                else if (drawList is IBillboard)
                {
                    drawListBillboard.Add(drawList);
                }
                else
                {
                    drawList3D.Add(drawList);
                }
            }

            foreach (DrawableComponent drawable in drawList3D)
            {
                //Seting up common 3D render states
                /*GameEngine.Graphics.RenderState.DepthBufferEnable = true;
                GameEngine.Graphics.RenderState.DepthBufferWriteEnable = true;
                GameEngine.Graphics.RenderState.AlphaBlendEnable = false;
                GameEngine.Graphics.RenderState.AlphaTestEnable = false;*/
                GameEngine.Graphics.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
                GameEngine.Graphics.SamplerStates[0].AddressV = TextureAddressMode.Wrap;

                if(drawable.Visible)
                    drawable.Draw(GameEngine.GameTime);
            }

            GameEngine.Graphics.Clear(ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            foreach (DrawableComponent drawable in drawListBillboard)
            {
                //setting common render states for billboards
                /*GameEngine.Graphics.RenderState.AlphaTestEnable = true;
                GameEngine.Graphics.RenderState.AlphaFunction = CompareFunction.GreaterEqual;
                GameEngine.Graphics.RenderState.ReferenceAlpha = 200;*/

                if (drawable.Visible)
                    drawable.Draw(GameEngine.GameTime);
            }

            foreach (DrawableComponent drawable in drawListEffects)
            {
                //setting for sprite batch effects
                /*GameEngine.Graphics.RenderState.PointSpriteEnable = true;
                GameEngine.Graphics.RenderState.PointSizeMax = 256;

                GameEngine.Graphics.RenderState.AlphaBlendEnable = true;
                GameEngine.Graphics.RenderState.AlphaBlendOperation = BlendFunction.Add;
                GameEngine.Graphics.RenderState.SourceBlend = Blend.SourceAlpha;
                GameEngine.Graphics.RenderState.DestinationBlend = Blend.One;

                GameEngine.Graphics.RenderState.DepthBufferEnable = true;
                GameEngine.Graphics.RenderState.DepthBufferWriteEnable = false;*/

                if (drawable.Visible)
                    drawable.Draw(GameEngine.GameTime);
            }

            //SpriteBatch handles the device render states

            foreach (DrawableComponent drawable in drawList2D)
            {
                /*GameEngine.Graphics.RenderState.PointSpriteEnable = false;
                GameEngine.Graphics.RenderState.AlphaBlendEnable = false;
                GameEngine.Graphics.RenderState.DepthBufferWriteEnable = true;*/

                if (drawable.Visible)
                    drawable.Draw(GameEngine.GameTime);
            }

        }

        public virtual void Dispose()
        {
            components.Clear();
            GameEngine.GameScreens.Remove(this);
            //owner.GameScreens.Remove(this);
        }

        #region IAudioEmitter Members

        public Vector3 Position
        {
            get { return Vector3.Zero; }
        }

        public Vector3 Forward
        {
            get { return Vector3.Forward; }
        }

        public Vector3 Up
        {
            get { return Vector3.Up; }
        }

        public Vector3 Velocity
        {
            get { return Vector3.Zero; }
        }

        #endregion
    }
}
