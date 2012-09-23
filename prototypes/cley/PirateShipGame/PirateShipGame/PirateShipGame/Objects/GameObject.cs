using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PirateShipGame.Objects
{
    class GameObject
    {
        protected Vector3 position;

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
    }
}
