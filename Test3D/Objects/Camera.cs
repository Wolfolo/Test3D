using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Test3D.Tools;

namespace Test3D.Objects
{
    public abstract class BaseCamera
    {
        // We need this to calculate the aspectRatio in the ProjectionMatrix property.
        protected GraphicsDevice graphicsDevice;
        Point center;

        protected Vector3 position;
        protected Vector3 up;
        protected Vector3 look;

        protected MouseState oldMouseState;

        protected ControlHandler lookController;
        protected ControlHandler movementController;

        const float unitsPerSecond = 10;

        int sensitivity = 5;
        bool invert = true;

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

        public BaseCamera(GraphicsDevice graphicsDevice, ControlHandler lookController, ControlHandler movementController)
        {
            this.graphicsDevice = graphicsDevice;
            this.lookController = lookController;
            this.movementController = movementController;

            BindControls();

            center = new Point(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
        }

        protected void BindControls()
        {
            lookController.UpPressed += delegate (object sender, EventArgs ea)
            {
                Rotate(invert ? CameraRotations.PitchDown : CameraRotations.PitchUp, (float)sensitivity / 200);
            };

            lookController.DownPressed += delegate (object sender, EventArgs ea)
            {
                Rotate(invert ? CameraRotations.PitchUp : CameraRotations.PitchDown, (float)sensitivity / 200);
            };

            lookController.LeftPressed += delegate (object sender, EventArgs ea)
            {
                Rotate(CameraRotations.YawLeft, (float)sensitivity / 200);
            };

            lookController.RightPressed += delegate (object sender, EventArgs ea)
            {
                Rotate(CameraRotations.YawRight, (float)sensitivity / 200);
            };

            lookController.RotateCWPressed += delegate (object sender, EventArgs ea)
            {
                Rotate(CameraRotations.RollClockwise, (float)sensitivity / 200);
            };

            lookController.RotateCCWPressed += delegate (object sender, EventArgs ea)
            {
                Rotate(CameraRotations.RollAntiClockwise, (float)sensitivity / 200);
            };

            var unit = .01f;

            movementController.LeftPressed += delegate (object sender, EventArgs ea)
            {
                Move(CameraMovements.StrafeLeft, unit);
            };

            movementController.RightPressed += delegate (object sender, EventArgs ea)
            {
                Move(CameraMovements.StrafeRight, unit);
            };

            movementController.UpPressed += delegate (object sender, EventArgs ea)
            {
                Move(CameraMovements.ThrustForward, unit);
            };

            movementController.DownPressed += delegate (object sender, EventArgs ea)
            {
                Move(CameraMovements.ThrustBackward, unit);
            };

            movementController.StrafeUpPressed += delegate (object sender, EventArgs ea)
            {
                Move(CameraMovements.StrafeUp, unit);
            };

            movementController.StrafeDownPressed += delegate (object sender, EventArgs ea)
            {
                Move(CameraMovements.StrafeDown, unit);
            };
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

        protected virtual void Thrust(float amount)
        {
        }

        protected virtual void StrafeHorizontal(float amount)
        {
        }

        protected virtual void StrafeVertical(float amount)
        {
        }

        protected virtual void Yaw(float amount)
        {
        }

        protected virtual void Pitch(float amount)
        {
        }

        protected virtual void Roll(float amount)
        {
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
            lookController.Update();
            movementController.Update();

            // Keep the mouse in the center or the camera will move continuously
            Mouse.SetPosition(center.X, center.Y);

            // Invert the movement direction?
            if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
            {
                invert = !invert;
            }

            // Sensitivity
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
        }

        public void Draw()
        {
#if DEBUG
            /*
             * Draw 3D Helper Axes on the world origin
             * X is red
             * Y is green
             * Z is blue
             */

            using (BasicEffect basicEffect = new BasicEffect(graphicsDevice))
            {
                var lines = new VertexPositionColor[6];
                lines[0] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Red);
                lines[1] = new VertexPositionColor(new Vector3(10, 0, 0), Color.Red);
                lines[2] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Green);
                lines[3] = new VertexPositionColor(new Vector3(0, 10, 0), Color.Green);
                lines[4] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Blue);
                lines[5] = new VertexPositionColor(new Vector3(0, 0, 10), Color.Blue);

                basicEffect.World = Matrix.Identity;
                basicEffect.View = ViewMatrix;
                basicEffect.Projection = ProjectionMatrix;
                basicEffect.VertexColorEnabled = true;

                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, lines, 0, 3);
                }
            }
#endif
        }
    }

    public class FPCamera : BaseCamera
    {
        public FPCamera(GraphicsDevice graphicsDevice, ControlHandler lookController, ControlHandler movementController)
            : base(graphicsDevice, lookController, movementController)
        {
        }

        protected override void Thrust(float amount)
        {
            look.Normalize();
            position += look * amount;
        }

        protected override void StrafeHorizontal(float amount)
        {
            var left = Vector3.Cross(up, look);
            left.Normalize();

            position += left * amount;
        }

        protected override void StrafeVertical(float amount)
        {
            up.Normalize();
            position += up * amount;
        }

        protected override void Yaw(float amount)
        {
            look.Normalize();

            look = Vector3.Transform(look, Matrix.CreateFromAxisAngle(up, amount));
        }

        protected override void Pitch(float amount)
        {
            look.Normalize();
            var left = Vector3.Cross(up, look);
            left.Normalize();

            look = Vector3.Transform(look, Matrix.CreateFromAxisAngle(left, amount));
            up = Vector3.Transform(up, Matrix.CreateFromAxisAngle(left, amount));
        }

        protected override void Roll(float amount)
        {
            up.Normalize();
            var left = Vector3.Cross(up, look);
            left.Normalize();

            up = Vector3.Transform(up, Matrix.CreateFromAxisAngle(look, amount));
        }
    }

    public class AbsoluteCamera : BaseCamera
    {
        public AbsoluteCamera(GraphicsDevice graphicsDevice, ControlHandler lookController, ControlHandler movementController)
            : base(graphicsDevice, lookController, movementController)
        {
        }

        protected override void Thrust(float amount)
        {
            position += Vector3.Up * amount;
        }

        protected override void StrafeHorizontal(float amount)
        {
            position += Vector3.Left * amount;
        }

        protected override void StrafeVertical(float amount)
        {
            position += Vector3.Forward * amount;
        }

        protected override void Yaw(float amount)
        {
            look = Vector3.Transform(look, Matrix.CreateFromYawPitchRoll(amount, 0, amount));
        }

        protected override void Pitch(float amount)
        {
            look = Vector3.Transform(look, Matrix.CreateFromYawPitchRoll(0, amount, 0));
        }

        protected override void Roll(float amount)
        {
            up = Vector3.Transform(up, Matrix.CreateFromYawPitchRoll(0, 0, amount));
        }
    }
}
