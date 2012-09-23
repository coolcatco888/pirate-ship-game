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
using PirateShipGame.Engine.Screens;

namespace PirateShipGame.Engine
{
    public static class GameEngine
    {
        static bool initialized;

        public static bool Initialized
        {
            get { return initialized; }
            set { initialized = value; }
        }

        static GameTime gameTime;

        public static GameTime GameTime
        {
            get { return gameTime; }
            set { gameTime = value; }
        }

        static SpriteBatch spriteBatch;

        public static float Frand
        {
            get
            {
                return (float)Random.NextDouble();
            }
        }

        public static Random Random = new Random();

        /// <summary>
        /// A default SpriteBatch shared by all the screens. This saves
        /// each screen having to bother creating their own local instance.
        /// </summary>
        public static SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public static GraphicsDevice Graphics
        {
            get { return game.GraphicsDevice; }
        }

        public static ContentManager Content
        {
            get { return game.Content; }
        }

        public static GameServiceContainer Services
        {
            get { return game.Services; }
        }

        public static ScreenCollection GameScreens
        {
            get { return screenManager.GameScreens; }
            set { screenManager.GameScreens = value; }
        }

        static Game game;
        static ScreenManager screenManager;

        public static Screen BaseScreen
        {
            get { return GameEngine.baseScreen; }
            set { GameEngine.baseScreen = value; }
        }
        static Screen baseScreen;

        public static void Initialize(Game g)
        {
            game = g;

            screenManager = new ScreenManager(g);
            g.Components.Add(screenManager);

            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            initialized = true;
        }

        public static void EndGame()
        {
            Content.Unload();
            game.Exit();
        }

    }
}
