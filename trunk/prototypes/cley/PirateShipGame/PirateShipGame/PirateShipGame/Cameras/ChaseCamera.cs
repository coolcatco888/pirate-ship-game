using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PirateShipGame.Objects;

namespace PirateShipGame.Cameras
{
    class ChaseCamera : Camera
    {

        public GameObject Target;

        public float FollowDistance = 1000.0f;

        public ChaseCamera()
        {
            this.cameraPosition = new Vector3(0.0f, FollowDistance, GameConstants.CameraHeight);
        }

        public override void Update(GameTime gameTime)
        {
            /*Matrix rotationMatrix = Matrix.CreateRotationX(MathHelper.PiOver2) *
                        Matrix.CreateRotationZ(Target.Rotation);

            Vector3 direction = new Vector3(FollowDistance);
            direction = Vector3.Transform(direction, rotationMatrix);
            cameraPosition = cameraPosition + direction;*/

            viewMatrix = Matrix.CreateLookAt(cameraPosition, Target.Position, Vector3.Up);
        }
    }
}
