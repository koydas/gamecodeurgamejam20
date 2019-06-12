using Laser_beams_pew_pew.Scenes;
using Laser_beams_pew_pew.Scenes.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Laser_beams_pew_pew
{
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public int WindowWidth;
        public int WindowHeight;

        public IScene CurrentScene = new Menu();
        public static Main Self;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Self = this;
        }

        protected override void Initialize()
        {
            WindowWidth = GraphicsDevice.PresentationParameters.Bounds.Width;
            WindowHeight = GraphicsDevice.PresentationParameters.Bounds.Height;
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            CurrentScene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            CurrentScene.Draw(spriteBatch, gameTime);

            base.Draw(gameTime);
        }
    }
}
