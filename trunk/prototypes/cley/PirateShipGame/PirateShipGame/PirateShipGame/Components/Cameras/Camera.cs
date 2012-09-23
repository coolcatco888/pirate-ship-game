using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PirateShipGame.Components.Cameras
{
    class Camera
    {

        //Camera/View information
        protected Vector3 cameraPosition = new Vector3(0.0f, 0.0f, GameConstants.CameraHeight);
        protected float aspectRatio = (float)GraphicsDeviceManager.DefaultBackBufferWidth / GraphicsDeviceManager.DefaultBackBufferHeight;
        protected Matrix projectionMatrix;
        protected Matrix viewMatrix;

        public Vector3 CameraPosition
        {
            get { return cameraPosition; }
            set { cameraPosition = value; }
        }

        public Matrix ProjectionMatrix
        {
            get { return projectionMatrix; }
        }

        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
        }

        public void Initialize()
        {
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                  MathHelper.ToRadians(45.0f), aspectRatio,
                  1.0f,//GameConstants.CameraHeight - 1000.0f,
                  30000.0f);

            viewMatrix = Matrix.CreateLookAt(cameraPosition, Vector3.Left, Vector3.Up);
        }

        public virtual void Update(GameTime gameTime)
        {
        }
    }
}
