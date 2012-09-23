using System;
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

namespace PirateShipGame.Engine.Screens
{
    public class ScreenManager : DrawableGameComponent
    {
        bool initialized;

        public bool Initialized
        {
            get { return initialized; }
            set { initialized = value; }
        }

        ScreenCollection gameScreens;

        public ScreenCollection GameScreens
        {
            get { return gameScreens; }
            set { gameScreens = value; }
        }

        List<Screen> updateList;
        List<Screen> drawList;
        

        public override void Initialize()
        {
            this.Enabled = true;
            this.Visible = true;
            initialized = true;

            base.Initialize();
        }

        public ScreenManager(Game game)
            : base(game)
        {
            gameScreens = new ScreenCollection();
            updateList = new List<Screen>();
            drawList = new List<Screen>();
        }

        protected override void LoadContent()
        {

            foreach (Screen screen in gameScreens)
            {
                screen.LoadContent();
            }

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            foreach (Screen screen in gameScreens)
            {
                screen.UnloadContent();
            }

            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {

            // Populate the temp list
            foreach (Screen screen in gameScreens)
                if (screen.Enabled)
                    updateList.Add(screen);

            // BlocksUpdate and AlwaysUpdate login
            for (int i = GameScreens.Count - 1; i >= 0; i--)
                if (GameScreens[i].BlocksUpdate)
                {
                    if (i > 0)
                        for (int j = i - 1; j >= 0; j--)
                            if (!GameScreens[j].AlwaysUpdate)
                                updateList.Remove(GameScreens[j]);

                    break;
                }

            // Update remaining components
            foreach (Screen screen in updateList)
                if (screen.Initialized)
                    screen.Update(gameTime);

            // Clear list
            updateList.Clear();

            base.Update(gameTime);

        }

        // Draws the current collection of screens and components. Accepts a
        // ComponentType to render
        public override void Draw(GameTime gameTime)
        {

            // Clear the back buffer
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Populate the temp list if the screen is visible
            foreach (Screen screen in GameScreens)
                if (screen.Visible)
                    drawList.Add(screen);

            // BlocksDraw and OverrideDrawBlocked logic
            for (int i = GameScreens.Count - 1; i >= 0; i--)
                if (GameScreens[i].BlocksDraw)
                {
                    if (i > 0)
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if (!GameScreens[j].AlwaysDraw)
                                drawList.Remove(GameScreens[j]);
                        }

                    break;
                }
            
            // Draw the remaining screens
            foreach (Screen screen in drawList)
                if (screen.Initialized)
                    screen.Draw();

            drawList.Clear();

            base.Draw(gameTime);
        }
    }
}
