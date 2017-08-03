using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test3D.Objects
{
    public class Tank
    {
        Model model;
        Matrix positionMatrix;
        Matrix rotationMatrix;

        public void Initialize(Model model)
        {
            this.model = model;
        }

        public void Draw(Camera camera)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.World = GetWorldMatrix();
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }

                // Now that we've assigned our properties on the effects we can
                // draw the entire mesh
                mesh.Draw();
            }
        }

        public void Move(Matrix position, Matrix rotation)
        {
            // this matrix moves the model "out" from the origin
            positionMatrix = position;

            // this matrix rotates everything around the origin
            rotationMatrix = rotation;
        }

        public void Update(GameTime gameTime)
        {
            // TotalSeconds is a double so we need to cast to float
            rotationMatrix *= Matrix.CreateRotationZ((float)gameTime.ElapsedGameTime.TotalSeconds);

            Move(positionMatrix, rotationMatrix);
        }

        private Matrix GetWorldMatrix()
        {
            // We combine the two to have the model move in a circle:
            return rotationMatrix * positionMatrix;
        }
    }
}
