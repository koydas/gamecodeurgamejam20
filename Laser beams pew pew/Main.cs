using Laser_beams_pew_pew.Scenes;
using Laser_beams_pew_pew.Scenes.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Laser_beams_pew_pew
{
    public class Main : Game
    {
        public bool DebugModeEnabled = false;
        GraphicsDeviceManager graphics;
        public SpriteBatch SpriteBatch;

        public int WindowWidth;
        public int WindowHeight;

        public IScene CurrentScene;
        public static Main Self;
        private KeyboardState _oldKeyboardState;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1920,
                PreferredBackBufferHeight = 1080,
            };

            Window.IsBorderless = true;
            Window.Position = Point.Zero;

            Content.RootDirectory = "Content";
            Self = this;
        }

        protected override void Initialize()
        {
            WindowWidth = GraphicsDevice.PresentationParameters.Bounds.Width;
            WindowHeight = GraphicsDevice.PresentationParameters.Bounds.Height;
            // TODO: Add your initialization logic here

            CurrentScene = new Menu();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            //    keyboardState.IsKeyDown(Keys.Escape) && _oldKeyboardState.IsKeyUp(Keys.Escape))
            //{
            //    if (CurrentScene is Menu)
            //    {
            //        Exit();
            //    }

            //    CurrentScene = new Menu();
            //}


            if (keyboardState.IsKeyDown(Keys.LeftControl) && keyboardState.IsKeyDown(Keys.D) && _oldKeyboardState.IsKeyUp(Keys.D))
            {
                DebugModeEnabled = !DebugModeEnabled;
            }

            _oldKeyboardState = keyboardState;


            CurrentScene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            CurrentScene.Draw(SpriteBatch, gameTime);

            base.Draw(gameTime);
        }
    }
}
