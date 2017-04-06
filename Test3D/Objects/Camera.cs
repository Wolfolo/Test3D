﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test3D.Objects
{
    public class Camera
    {
        // We need this to calculate the aspectRatio in the ProjectionMatrix property.
        GraphicsDevice graphicsDevice;

        Vector3 position = new Vector3(0, 20, 10);

        float angle;

        public Matrix ViewMatrix
        {
            get
            {
                var lookAtVector = new Vector3(0, -1, -.5f);

                // We'll create a rotation matrix using our angle
                var rotationMatrix = Matrix.CreateRotationZ(angle);
                // Then we'll modify the vector using this matrix:
                lookAtVector = Vector3.Transform(lookAtVector, rotationMatrix);
                lookAtVector += position;

                var upVector = Vector3.UnitZ;

                return Matrix.CreateLookAt(position, lookAtVector, upVector);
            }
        }

        public Matrix ProjectionMatrix
        {
            get
            {
                float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
                float nearClipPlane = 1;
                float farClipPlane = 200;
                float aspectRatio = graphicsDevice.Viewport.Width / (float)graphicsDevice.Viewport.Height;

                return Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
            }
        }

        public Camera(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public void Update(GameTime gameTime)
        {
            // Rotation
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                angle += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                angle -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            //Movement
            var forwardVector = new Vector3(0, -1, 0);
            var rotationMatrix = Matrix.CreateRotationZ(angle);
            const float unitsPerSecond = 10;

            forwardVector = Vector3.Transform(forwardVector, rotationMatrix);

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                position += forwardVector * unitsPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                position -= forwardVector * unitsPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
