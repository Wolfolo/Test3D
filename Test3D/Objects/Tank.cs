using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test3D.Objects
{
    public class Tank
    {
        Model model;

        float angle;
        Vector3 position;

        public void Initialize(ContentManager contentManager, string modelName, Vector3 startPos)
        {
            model = contentManager.Load<Model>(modelName);
            position = startPos;
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

        public void Update(GameTime gameTime)
        {
            // TotalSeconds is a double so we need to cast to float
            angle += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        Matrix GetWorldMatrix()
        {
            // this matrix moves the model "out" from the origin
            Matrix positionMatrix = Matrix.CreateTranslation(position);

            // this matrix rotates everything around the origin
            Matrix rotationMatrix = Matrix.CreateRotationZ(angle);

            // We combine the two to have the model move in a circle:
            Matrix combined = rotationMatrix * positionMatrix;

            return combined;
        }
    }
}
