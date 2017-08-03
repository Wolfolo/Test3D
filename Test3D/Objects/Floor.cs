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
    class Floor
    {
        VertexPositionTexture[] floorVerts;
        Texture2D texture;
        BasicEffect effect;
        GraphicsDevice graphicsDevice;

        public Floor(GraphicsDevice device)
        {
            graphicsDevice = device;
        }

        public void Initialize(ContentManager contentManager, string textureName, Vector2 size)
        {
            effect = new BasicEffect(graphicsDevice);

            floorVerts = new VertexPositionTexture[6];

            floorVerts[0].Position = new Vector3(-size.X, -size.Y, 0);
            floorVerts[1].Position = new Vector3(-size.X, size.Y, 0);
            floorVerts[2].Position = new Vector3(size.X, -size.Y, 0);

            floorVerts[3].Position = floorVerts[1].Position;
            floorVerts[4].Position = new Vector3(size.X, size.Y, 0);
            floorVerts[5].Position = floorVerts[2].Position;

            floorVerts[0].TextureCoordinate = new Vector2(0, 0);
            floorVerts[1].TextureCoordinate = new Vector2(0, size.Y);
            floorVerts[2].TextureCoordinate = new Vector2(size.X, 0);

            floorVerts[3].TextureCoordinate = floorVerts[1].TextureCoordinate;
            floorVerts[4].TextureCoordinate = new Vector2(size.X, size.Y);
            floorVerts[5].TextureCoordinate = floorVerts[2].TextureCoordinate;

            texture = contentManager.Load<Texture2D>(textureName);
        }

        public void Draw(BaseCamera camera)
        {
            // The assignment of effect.View and effect.Projection
            // are nearly identical to the code in the Model drawing code.
            effect.View = camera.ViewMatrix;
            effect.Projection = camera.ProjectionMatrix;

            effect.TextureEnabled = true;
            effect.Texture = texture;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphicsDevice.DrawUserPrimitives(
                    // We’ll be rendering two triangles
                    PrimitiveType.TriangleList,
                    // The array of verts that we want to render
                    floorVerts,
                    // The offset, which is 0 since we want to start 
                    // at the beginning of the floorVerts array
                    0,
                    // The number of triangles to draw
                    2);
            }
        }
    }
}
