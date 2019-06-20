using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Laser_beams_pew_pew.DebugHelpers;
using Laser_beams_pew_pew.Game_objects;
using Laser_beams_pew_pew.Game_objects.Bosses;
using Laser_beams_pew_pew.Game_objects.Projectiles;
using Laser_beams_pew_pew.Scenes.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Laser_beams_pew_pew.Scenes
{
    public class GamePlay: IScene
    {
        public Player Player;
        public LaserBoss LaserBoss;

        public List<Bullet> Bullets = new List<Bullet>();
        public List<Laser> Lasers = new List<Laser>();

        private bool _gameOver;
        private SpriteFont _font;

        public GamePlay()
        {
            _font = Main.Self.Content.Load<SpriteFont>("fonts/Space Age");

            Main.Self.IsMouseVisible = false;

            Player = new Player(Bullets);
            LaserBoss = new LaserBoss(Lasers, Player);
        }

        public void Update(GameTime gameTime)
        {
            if (_gameOver)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed ||
                    Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    Main.Self.CurrentScene = new Menu();
                } 

                return;
            }

            if (Main.Self.DebugModeEnabled && Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Thread.Sleep(30);
            }

            Player.Update(gameTime);
            LaserBoss.Update(gameTime);

            for (var index = 0; index < Bullets.Count; index++)
            {
                var bullet = Bullets[index];

                bullet.Update(gameTime);

                LaserBoss.IsHit(bullet);

                if (bullet.Position.X > Main.Self.WindowWidth || (bullet.HasHitSomething && bullet.ExplosionFinished))
                {
                    Bullets.RemoveAt(index);
                }
            }

            for (var index = 0; index < Lasers.Count; index++)
            {
                var laser = Lasers[index];

                Player.IsHit(laser);

                laser.Update(gameTime);

                if (laser.Position.X > Main.Self.WindowWidth || laser.HasHitSomething)
                {
                    Lasers.RemoveAt(index);
                }
            }

            if (LaserBoss.HitPoints <= 0 || (Player.HitPoints <= 0 && Player.ExplosionFinished))
            {
                Main.Self.IsMouseVisible = true;
                _gameOver = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Main.Self.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            if (Main.Self.DebugModeEnabled) Player.DrawHitBox();
            
            Player.Draw(spriteBatch, gameTime);

            if (Main.Self.DebugModeEnabled) LaserBoss.DrawHitBox();
            LaserBoss.Draw(spriteBatch, gameTime);

            foreach (var bullet in Bullets)
            {
                if (Main.Self.DebugModeEnabled) bullet.DrawHitBox();
                bullet.Draw(spriteBatch, gameTime);
            }

            foreach (var laser in Lasers)
            {
                if (Main.Self.DebugModeEnabled) laser.DrawHitBox();
                laser.Draw(spriteBatch, gameTime);
            }

            if (_gameOver)
            {
                var overlayTexture = new Texture2D(Main.Self.GraphicsDevice, 1, 1);
                overlayTexture.SetData(new[] { Color.Black });

                spriteBatch.Draw(
                    overlayTexture,
                    Vector2.Zero,
                    null,
                    new Color(Color.White, 0.8f),
                    0f,
                    Vector2.Zero,
                    new Vector2(1920, 1080), 
                    SpriteEffects.None,
                    1f);

                string text;
                text = Player.HitPoints <= 0 ? "You lost !" : "You won !";

                spriteBatch.DrawString(
                    _font,
                    text,
                    new Vector2
                    {
                        X = Main.Self.WindowWidth/3f,
                        Y = Main.Self.WindowHeight/3f,
                    }, 
                    Color.White,
                    0f,
                    Vector2.Zero,
                    2f,
                    SpriteEffects.None,
                    1f);

                spriteBatch.DrawString(
                    _font,
                    "Click to try again",
                    new Vector2
                    {
                        X = Main.Self.WindowWidth / 3f,
                        Y = Main.Self.WindowHeight / 2f,
                    },
                    Color.White,
                    0f,
                    Vector2.Zero,
                    2f,
                    SpriteEffects.None,
                    1f);
            }

            spriteBatch.End();
        }
    }
}
