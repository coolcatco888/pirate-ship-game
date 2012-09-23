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

        public float FollowDistance = 5000.0f;

        Vector3 velocity = Vector3.Zero;

        public ChaseCamera()
        {
            this.cameraPosition = new Vector3(0.0f, FollowDistance, GameConstants.CameraHeight);
        }

        public override void Update(GameTime gameTime)
        {
            //Rotate with the target
            Matrix rotationMatrix = Matrix.CreateRotationY(Target.Rotation);
            Vector3 direction = new Vector3(0.0f, GameConstants.CameraHeight, FollowDistance);
            direction = Vector3.Transform(direction, rotationMatrix);

            Vector3 newPosition = Target.Position + direction;

            newPosition -= cameraPosition;
            float length = newPosition.Length();
            float velocity = length * 0.0085f;
            newPosition.Normalize();
            newPosition *= velocity * gameTime.ElapsedGameTime.Milliseconds;
            cameraPosition += newPosition;

            viewMatrix = Matrix.CreateLookAt(cameraPosition, Target.Position, Vector3.Up);
        }
    }
}
