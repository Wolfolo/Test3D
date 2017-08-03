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
        Tank tank_light;
        Tank tank_medium;
        Tank tank_dual;
        Camera camera;

        public Test3DDemo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            floor = new Floor(GraphicsDevice);
            floor.Initialize(Content, "Textures/checkerboard");

            tank_light = new Tank();
            tank_light.Initialize(Content.Load<Model>("Models/Tank_Light"));
            tank_light.Move(Matrix.CreateTranslation(new Vector3(10, 0, 0)), Matrix.Identity);

            tank_medium = new Tank();
            tank_medium.Initialize(Content.Load<Model>("Models/Tank_Medium"));
            tank_medium.Move(Matrix.CreateTranslation(new Vector3(0, 0, 0)), Matrix.Identity);

            tank_dual = new Tank();
            tank_dual.Initialize(Content.Load<Model>("Models/Tank_Dual"));
            tank_dual.Move(Matrix.CreateTranslation(new Vector3(-10, 0, 0)), Matrix.Identity);

            camera = new Camera(GraphicsDevice);
            camera.SetPosition(new Vector3(0, -20, 10));
            camera.SetRotation(Matrix.CreateFromYawPitchRoll(0, 1, 0));
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

            tank_light.Update(gameTime);
            tank_medium.Update(gameTime);
            tank_dual.Update(gameTime);
            camera.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            floor.Draw(camera);

            tank_light.Draw(camera);
            tank_medium.Draw(camera);
            tank_dual.Draw(camera);

            base.Draw(gameTime);
        }
    }
}
