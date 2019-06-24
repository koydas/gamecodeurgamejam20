using Laser_beams_pew_pew.Game_objects.Bosses;
using Laser_beams_pew_pew.Game_objects.Bosses.Enums;
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
        private readonly Vector2 _laserBossPosition = new Vector2(550, 500);
        private Color _laserBossColor = Color.White;

        private Texture2D _bombBoss;
        private readonly Vector2 _bombBossPosition;
        private Color _bombBossColor = Color.White;
        private Texture2D _header;

        public Menu()
        {
            Main.Self.IsMouseVisible = true;

            _font = Main.Self.Content.Load<SpriteFont>("fonts/Space Age");
            _header = Main.Self.Content.Load<Texture2D>("images/header");
            _laserboss = Main.Self.Content.Load<Texture2D>("images/ennemy-ship");
            _bombBoss = Main.Self.Content.Load<Texture2D>("images/bomb-boss");


            _bombBossPosition = new Vector2(_laserBossPosition.X + _laserboss.Width, _laserBossPosition.Y);
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

            var bombBossRect = new Rectangle
            {
                Width = _bombBoss.Width / 2,
                Height = _bombBoss.Height / 2,
                X = (int)_bombBossPosition.X,
                Y = (int)_bombBossPosition.Y,
            };

            _laserBossColor = mouseRect.Intersects(laserBossRect) ? Color.White : Color.Gray;
            _bombBossColor = mouseRect.Intersects(bombBossRect) ? Color.White : Color.Gray;

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (mouseRect.Intersects(laserBossRect))
                {
                    Main.Self.CurrentScene = new GamePlay<LaserBoss>();
                }
                else if (mouseRect.Intersects(bombBossRect))
                {
                    Main.Self.CurrentScene = new GamePlay<BombBoss>();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Main.Self.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(_header, Vector2.One);

            spriteBatch.DrawString(_font, "Choose your enemy", new Vector2(600, 400), Color.White);

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

            spriteBatch.Draw(
                _bombBoss,
                _bombBossPosition,
                null,
                _bombBossColor,
                0f,
                Vector2.Zero,
                0.5f,
                SpriteEffects.None,
                1f);

            spriteBatch.End();
        }
    }
}
