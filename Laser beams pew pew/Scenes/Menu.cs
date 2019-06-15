using Laser_beams_pew_pew.Scenes.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Laser_beams_pew_pew.Scenes
{
    public class Menu: IScene
    {
        private SpriteFont _font;
        private readonly Texture2D _laserboss;
        private readonly Vector2 _laserBossPosition = new Vector2(100, 100);
        private Color _laserBossColor = Color.White;

        public Menu()
        {
            Main.Self.IsMouseVisible = true;

            _font = Main.Self.Content.Load<SpriteFont>("fonts/Space Age");
            _laserboss = Main.Self.Content.Load<Texture2D>("images/ennemy-ship");
        }

        public void Update(GameTime gameTime)
        {
            var mouseRect = new Rectangle
            {
                Width = 1,
                Height = 1,
                X = Mouse.GetState().Position.X,
                Y = Mouse.GetState().Position.Y
            };

            var laserBossRect = new Rectangle
            {
                Width = _laserboss.Width / 2,
                Height = _laserboss.Height / 2,
                X = (int)_laserBossPosition.X,
                Y = (int)_laserBossPosition.Y,
            };

            _laserBossColor = mouseRect.Intersects(laserBossRect) ? Color.White : Color.Gray;

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (mouseRect.Intersects(laserBossRect))
                {
                    Main.Self.CurrentScene = new GamePlay();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Main.Self.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.DrawString(_font, "Choose your enemy", Vector2.Zero, Color.White);

            spriteBatch.Draw(
                _laserboss,
                _laserBossPosition,
                null,
                _laserBossColor,
                0f,
                Vector2.Zero,
                0.5f,
                SpriteEffects.None,
                1f);

            spriteBatch.End();
        }
    }
}
