//-----------------------------------------------------------------------------
// Ship.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PirateShipGame.Objects
{
    class Ship : GameObject
    {
        public Model Model;
        public Matrix[] Transforms;

        //Velocity of the model, applied each frame to the model's position
        public Vector3 Velocity = Vector3.Zero;
        private const float VelocityScale = 5.0f; //amplifies controller speed input
        public bool isActive = true;
        private float rotation;
        

        public void Update(GamePadState controllerState)
        {
            // Rotate the model using the left thumbstick, and scale it down.
            Rotation -= controllerState.ThumbSticks.Left.X * 0.10f;

            // Finally, add this vector to our velocity.
            Velocity += RotationMatrix.Forward * VelocityScale * controllerState.Triggers.Right;
        }
    }
}
