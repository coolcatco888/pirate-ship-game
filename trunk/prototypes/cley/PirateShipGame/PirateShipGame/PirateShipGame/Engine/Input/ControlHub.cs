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

namespace PirateShipGame.Engine.Input
{
    class InputHub : Dictionary<PlayerIndex, GamePadDevice>
    {
        PlayerIndex masterInput;

        public PlayerIndex MasterInput
        {
            get { return masterInput; }
        }

        public InputHub()
            : base()
        {
            GamePadDevice gamePadDevice;

            gamePadDevice = new GamePadDevice(GameEngine.BaseScreen, PlayerIndex.Four);
            gamePadDevice.Initialize();
            this.Add(gamePadDevice.PlayerIndex, gamePadDevice);

            if (gamePadDevice.IsConnected)
                masterInput = gamePadDevice.PlayerIndex;

            gamePadDevice = new GamePadDevice(GameEngine.BaseScreen, PlayerIndex.Three);
            gamePadDevice.Initialize();
            this.Add(gamePadDevice.PlayerIndex, gamePadDevice);

            if (gamePadDevice.IsConnected)
                masterInput = gamePadDevice.PlayerIndex;

            gamePadDevice = new GamePadDevice(GameEngine.BaseScreen, PlayerIndex.Two);
            gamePadDevice.Initialize();
            this.Add(gamePadDevice.PlayerIndex, gamePadDevice);

            if (gamePadDevice.IsConnected)
                masterInput = gamePadDevice.PlayerIndex;

            gamePadDevice = new GamePadDevice(GameEngine.BaseScreen, PlayerIndex.One);
            gamePadDevice.Initialize();
            this.Add(gamePadDevice.PlayerIndex, gamePadDevice);

            if (gamePadDevice.IsConnected)
                masterInput = gamePadDevice.PlayerIndex;
        }



    }
}
