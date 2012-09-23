using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PirateShipGame.Objects
{
    class GameObject
    {
        public Matrix RotationMatrix = Matrix.CreateRotationX(MathHelper.PiOver2);

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
                    RotationMatrix = Matrix.CreateRotationX(MathHelper.PiOver2) *
                        Matrix.CreateRotationZ(rotation);
                }
            }
        }
    }
}
