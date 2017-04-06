using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Test3D.Objects;

namespace Test3D
{
    class Test3DDemo : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Floor floor;
        Tank tank;

        //Camera
        Vector3 cameraPosition = new Vector3(15, 10, 10);

        public Test3DDemo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            floor = new Floor();
            floor.Initialize(GraphicsDevice, Content, "Textures/checkerboard");

            tank = new Tank();
            tank.Initialize(Content, "Models/Tank_Light");
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            tank.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            float aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
            floor.Draw(cameraPosition, aspectRatio);
            tank.Draw(cameraPosition, aspectRatio);

            base.Draw(gameTime);
        }
    }
}
