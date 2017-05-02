﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Test3D.Objects
{
    public class Camera
    {
        // We need this to calculate the aspectRatio in the ProjectionMatrix property.
        GraphicsDevice graphicsDevice;
        Point center;

        Vector3 position;
        Vector3 up;
        Vector3 look;

        MouseState oldMouseState;

        const float unitsPerSecond = 10;

        int sensitivity = 5;
        bool invertMouse = true;

        public enum CameraRotations
        {
            PitchDown,
            PitchUp,
            RollAntiClockwise,
            RollClockwise,
            YawLeft,
            YawRight,
        }

        public enum CameraMovements
        {
            StrafeDown,
            StrafeLeft,
            StrafeRight,
            StrafeUp,
            ThrustBackward,
            ThrustForward,
        }

        public Matrix ViewMatrix
        {
            get
            {
                /* The CreateLookAt function requires a camera position, a target and an up direction.
                 * We know the camera position and the up vector but we do not know the target.
                 * However the look vector points in the direction the camera is facing so we can use that to create a target by adding it to the position.
                 */
                return Matrix.CreateLookAt(position, look + position, up);
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

            center = new Point(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
        }

        public void SetPosition(Vector3 pos)
        {
            position = pos;
        }

        public void SetRotation(Matrix rot)
        {
            up = rot.Up;
            look = rot.Forward;
        }

        private void Thrust(float amount)
        {
            look.Normalize();
            position += look * amount;
        }

        private void StrafeHorizontal(float amount)
        {
            var left = Vector3.Cross(up, look);
            left.Normalize();

            position += left * amount;
        }

        private void StrafeVertical(float amount)
        {
            up.Normalize();
            position += up * amount;
        }

        private void Yaw(float amount)
        {
            look.Normalize();

            look = Vector3.Transform(look, Matrix.CreateFromAxisAngle(up, amount));
        }

        private void Pitch(float amount)
        {
            look.Normalize();
            var left = Vector3.Cross(up, look);
            left.Normalize();

            look = Vector3.Transform(look, Matrix.CreateFromAxisAngle(left, amount));
            up = Vector3.Transform(up, Matrix.CreateFromAxisAngle(left, amount));
        }

        private void Roll(float amount)
        {
            up.Normalize();
            var left = Vector3.Cross(up, look);
            left.Normalize();

            up = Vector3.Transform(up, Matrix.CreateFromAxisAngle(look, amount));
        }

        /// <summary>
        /// Rotate the camera
        /// </summary>
        /// <param name="rot"></param>
        /// <param name="angle">Angle in radians</param>
        public void Rotate(CameraRotations rot, float angle)
        {
            switch (rot)
            {
                case CameraRotations.YawLeft:
                    Yaw(-angle);
                    break;
                case CameraRotations.YawRight:
                    Yaw(angle);
                    break;
                case CameraRotations.PitchUp:
                    Pitch(-angle);
                    break;
                case CameraRotations.PitchDown:
                    Pitch(angle);
                    break;
                case CameraRotations.RollClockwise:
                    Roll(angle);
                    break;
                case CameraRotations.RollAntiClockwise:
                    Roll(-angle);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Move the camera in the given direction
        /// </summary>
        /// <param name="mov"></param>
        /// <param name="amount"></param>
        public void Move(CameraMovements mov, float amount)
        {
            var speed = unitsPerSecond * amount;

            switch (mov)
            {
                case CameraMovements.StrafeLeft:
                    StrafeHorizontal(speed);
                    break;
                case CameraMovements.StrafeRight:
                    StrafeHorizontal(-speed);
                    break;
                case CameraMovements.ThrustForward:
                    Thrust(speed);
                    break;
                case CameraMovements.ThrustBackward:
                    Thrust(-speed);
                    break;
                case CameraMovements.StrafeUp:
                    StrafeVertical(speed);
                    break;
                case CameraMovements.StrafeDown:
                    StrafeVertical(-speed);
                    break;
                default:
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            float unit = .01f;

            // Mouse look
            if (Mouse.GetState().RightButton == ButtonState.Released)
            {
                if (Mouse.GetState().Position.X > center.X)
                {
                    Rotate(invertMouse ? CameraRotations.YawLeft : CameraRotations.YawRight, (float)sensitivity / 200);
                }
                else if (Mouse.GetState().Position.X < center.X)
                {
                    Rotate(invertMouse ? CameraRotations.YawRight : CameraRotations.YawLeft, (float)sensitivity / 200);
                }

                if (Mouse.GetState().Position.Y > center.Y)
                {
                    Rotate(invertMouse ? CameraRotations.PitchDown : CameraRotations.PitchUp, (float)sensitivity / 200);
                }
                else if (Mouse.GetState().Position.Y < center.Y)
                {
                    Rotate(invertMouse ? CameraRotations.PitchUp : CameraRotations.PitchDown, (float)sensitivity / 200);
                }
            }
            else
            {
                if (Mouse.GetState().Position.X > center.X)
                {
                    Rotate(invertMouse ? CameraRotations.RollAntiClockwise : CameraRotations.RollClockwise, (float)sensitivity / 200);
                }
                else if (Mouse.GetState().Position.X < center.X)
                {
                    Rotate(invertMouse ? CameraRotations.RollClockwise : CameraRotations.RollAntiClockwise, (float)sensitivity / 200);
                }
            }

            // Keep the mouse in the center or the camera will move continuously
            Mouse.SetPosition(center.X, center.Y);

            // Invert the movement direction?
            if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
            {
                invertMouse = !invertMouse;
            }

            // Sensitivitsy
            if (Mouse.GetState().ScrollWheelValue < oldMouseState.ScrollWheelValue)
            {
                sensitivity += sensitivity > 1 ? -1 : 0;
            }
            else if (Mouse.GetState().ScrollWheelValue > oldMouseState.ScrollWheelValue)
            {
                sensitivity += sensitivity < 10 ? 1 : 0;
            }

            // Update the mouse state
            oldMouseState = Mouse.GetState();

            // Movement
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                Move(CameraMovements.StrafeLeft, unit);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                Move(CameraMovements.StrafeRight, unit);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                Move(CameraMovements.ThrustForward, unit);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                Move(CameraMovements.ThrustBackward, unit);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.PageUp))
            {
                Move(CameraMovements.StrafeUp, unit);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.PageDown))
            {
                Move(CameraMovements.StrafeDown, unit);
            }
        }
    }
}
