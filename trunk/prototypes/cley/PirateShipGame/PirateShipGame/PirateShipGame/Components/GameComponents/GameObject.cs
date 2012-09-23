using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PirateShipGame.Components.GameComponents
{
    class GameObject
    {
        public Matrix RotationMatrix = Matrix.CreateRotationX(0);

        protected Vector3 position;

        protected float rotation;

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set
            {
                while (value >= MathHelper.TwoPi)
                {
                    value -= MathHelper.TwoPi;
                }
                while (value < 0)
                {
                    value += MathHelper.TwoPi;
                }
                if (rotation != value)
                {
                    rotation = value;
                    RotationMatrix = Matrix.CreateRotationY(rotation);
                }
            }
        }
    }
}
