using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Test3D.Tools
{
    public class ControlHandler
    {
        public enum ControlType
        {
            Movement,
            Look,
        }

        Point center;
        ControlType controlType;

        private static ControlHandler lookController;
        private static ControlHandler movementController;

        private ControlHandler(ControlType controlType, Point center)
        {
            this.center = center;
            this.controlType = controlType;
        }

        public static ControlHandler GetLookController(Point center)
        {
            if (lookController == null)
            {
                lookController = new ControlHandler(ControlType.Look, center);
            }

            return lookController;
        }

        public static ControlHandler GetMovementController(Point center)
        {
            if (movementController == null)
            {
                movementController = new ControlHandler(ControlType.Movement, center);
            }

            return movementController;
        }

        public void Update()
        {
            switch (controlType)
            {
                case ControlType.Movement:
                    HandleMovement();
                    break;
                case ControlType.Look:
                    HandleLook();
                    break;
                default:
                    break;
            }
        }

        private void HandleLook()
        {
            if (Mouse.GetState().RightButton == ButtonState.Released)
            {
                if (Mouse.GetState().Position.X > center.X)
                {
                    RightPressed?.Invoke(this, null);
                }
                else if (Mouse.GetState().Position.X < center.X)
                {
                    LeftPressed?.Invoke(this, null);
                }

                if (Mouse.GetState().Position.Y > center.Y)
                {
                    UpPressed?.Invoke(this, null);
                }
                else if (Mouse.GetState().Position.Y < center.Y)
                {
                    DownPressed?.Invoke(this, null);
                }
            }
            else
            {
                if (Mouse.GetState().Position.X > center.X)
                {
                    RotateCWPressed?.Invoke(this, null);
                }
                else if (Mouse.GetState().Position.X < center.X)
                {
                    RotateCCWPressed?.Invoke(this, null);
                }
            }
        }

        private void HandleMovement()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                RightPressed?.Invoke(this, null);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                LeftPressed?.Invoke(this, null);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                UpPressed?.Invoke(this, null);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                DownPressed?.Invoke(this, null);
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.PageUp))
            {
                StrafeUpPressed?.Invoke(this, null);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.PageDown))
            {
                StrafeDownPressed?.Invoke(this, null);
            }
        }

        public event EventHandler UpPressed;
        public event EventHandler UpReleased;
        public event EventHandler DownPressed;
        public event EventHandler DownReleased;
        public event EventHandler LeftPressed;
        public event EventHandler LeftReleased;
        public event EventHandler RightPressed;
        public event EventHandler RightReleased;
        public event EventHandler RotateCWPressed;
        public event EventHandler RotateCWReleased;
        public event EventHandler RotateCCWPressed;
        public event EventHandler RotateCCWReleased;
        public event EventHandler CtrlPressed;
        public event EventHandler CtrlReleased;
        public event EventHandler ShiftPressed;
        public event EventHandler ShiftReleased;
        public event EventHandler StrafeUpPressed;
        public event EventHandler StrafeUpReleased;
        public event EventHandler StrafeDownPressed;
        public event EventHandler StrafeDownReleased;
    }
}
