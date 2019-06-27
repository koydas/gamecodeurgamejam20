using System;
using System.Collections.Generic;
using System.Threading;
using Laser_beams_pew_pew.Game_objects;
using Laser_beams_pew_pew.Game_objects.Bosses;
using Laser_beams_pew_pew.Game_objects.Interfaces;
using Laser_beams_pew_pew.Game_objects.Projectiles;
using Laser_beams_pew_pew.Helpers;
using Laser_beams_pew_pew.Scenes.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Laser_beams_pew_pew.Scenes
{
    public class GamePlay<T> : IScene where T : IBoss
    {
        public Player Player;
        public GameObject Boss;

        public List<Bullet> Bullets = new List<Bullet>();
        public List<IProjectile> EnemyProjectile = new List<IProjectile>();

        private bool _gameOver;
        private SpriteFont _font;
        private Texture2D _planet;
        private Texture2D _spiral;
        private Texture2D _star;

        private int _nbOfStars = 30;
        private readonly IList<Star> _stars = new List<Star>();
        private Texture2D _healbarBorder;
        private Texture2D _healbarFill;
        private MouseState _oldMouseState;

        private struct Star
        {
            public Vector2 Position;
            public float Scale;
            public float Rotation;
        }

        public GamePlay()
        {
            _font = Main.Self.Content.Load<SpriteFont>("fonts/Space Age");
            _planet = Main.Self.Content.Load<Texture2D>("images/planet");
            _spiral = Main.Self.Content.Load<Texture2D>("images/spiral");
            _star = Main.Self.Content.Load<Texture2D>("images/star");

            _healbarBorder = Main.Self.Content.Load<Texture2D>("images/healthbar-border");
            _healbarFill = Main.Self.Content.Load<Texture2D>("images/healthbar-fill");
            var music = Main.Self.Content.Load<Song>("sounds/battle-music");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.3f;
            MediaPlayer.Play(music);

            Main.Self.IsMouseVisible = false;

            Player = new Player(Bullets);

            if (typeof(T).IsAssignableFrom(typeof(LaserBoss)))
            {
                Boss = new LaserBoss(EnemyProjectile, Player);
            }
            else if (typeof(T).IsAssignableFrom(typeof(BombBoss)))
            {
                Boss = new BombBoss(EnemyProjectile, Player);
            }

            Random random = new Random();
            for (int i = 0; i < _nbOfStars; i++)
            {
                float scale = random.Next(2, 7) / 100f;
                float x = random.Next(0, Main.Self.WindowWidth);
                float y = random.Next(0, Main.Self.WindowHeight);

                float rotation = random.Next(1, 1000) / 1000f;

                _stars.Add(new Star()
                {
                    Position = new Vector2(x, y),
                    Scale = scale,
                    Rotation = rotation
                });
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_gameOver)
            {
                if ((Mouse.GetState().LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released) ||
                    Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    Main.Self.CurrentScene = new Menu();
                }

                _oldMouseState = Mouse.GetState();

                return;
            }

            if (Main.Self.DebugModeEnabled && Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Thread.Sleep(30);
            }

            Player.Update(gameTime);
            Boss.Update(gameTime);

            for (var index = 0; index < Bullets.Count; index++)
            {
                var bullet = Bullets[index];
                bullet.Update(gameTime);

                if (Boss.HitPoints <= 0 || Player.HitPoints <= 0) break;

                Boss.IsHit(bullet);

                if (Boss is BombBoss && bullet.Position.X > Main.Self.WindowWidth / 2f)
                {
                    bullet.HasHitSomething = true;
                }

                if (bullet.Position.X > Main.Self.WindowWidth || (bullet.HasHitSomething && bullet.ExplosionFinished))
                {
                    Bullets.RemoveAt(index);
                }
            }

            for (var index = 0; index < EnemyProjectile.Count; index++)
            {
                var enemyProjectile = EnemyProjectile[index];
                enemyProjectile.Update(gameTime);

                if (Boss.HitPoints <= 0 || Player.HitPoints <= 0) break;

                Player.IsHit(enemyProjectile);
                Boss.IsHit(enemyProjectile);

                if (!(Boss is LaserBoss laserBoss && laserBoss.WhileConeAttack))
                {
                    foreach (var bullet in Bullets)
                    {
                        enemyProjectile.IsHit(bullet);
                    }

                    foreach (var enemyProjectile2 in EnemyProjectile)
                    {
                        if (enemyProjectile2 != enemyProjectile)
                            enemyProjectile.IsHit(enemyProjectile2);
                    }
                }

                if (enemyProjectile.Position.X < 0 || enemyProjectile.Position.X > Main.Self.WindowWidth || enemyProjectile.ExplosionFinished)
                {
                    EnemyProjectile.RemoveAt(index);
                }
            }

            if (Boss.HitPoints <= 0 || (Player.HitPoints <= 0 && Player.ExplosionFinished))
            {
                Main.Self.Content.Load<SoundEffect>("sounds/smallboom").Play();
                Main.Self.IsMouseVisible = true;
                _gameOver = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Main.Self.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            // Draw Stars
            var random = new Random();
            foreach (var star in _stars)
            {
                spriteBatch.Draw(
                    _star,
                    star.Position,
                    null,
                    Color.DarkGray,
                    star.Rotation,
                    Vector2.Zero,
                    star.Scale,
                    SpriteEffects.None,
                    1f);
            }

            spriteBatch.Draw(
                _spiral,
                new Vector2(250, 50),
                null,
                Color.Gray,
                0f,
                Vector2.Zero,
                Vector2.One,
                SpriteEffects.None,
                1f);

            spriteBatch.Draw(
                _planet,
                new Vector2(Main.Self.WindowWidth - _planet.Width / 2, -250),
                null,
                Color.White,
                0f,
                Vector2.Zero,
                Vector2.One,
                SpriteEffects.None,
                1f);

            if (Main.Self.DebugModeEnabled) Player.DrawHitBox();

            Player.Draw(spriteBatch, gameTime);

            if (Main.Self.DebugModeEnabled) Boss.DrawHitBox();
            Boss.Draw(spriteBatch, gameTime);

            foreach (var bullet in Bullets)
            {
                if (Main.Self.DebugModeEnabled) bullet.DrawHitBox();
                bullet.Draw(spriteBatch, gameTime);
            }

            foreach (var laser in EnemyProjectile)
            {
                if (Main.Self.DebugModeEnabled) laser.DrawHitBox();
                laser.Draw(spriteBatch, gameTime);
            }

            var healbarBgFill = new Texture2D(Main.Self.GraphicsDevice, 1, 1);
            healbarBgFill.SetData(new[] { Color.Black });

            spriteBatch.Draw(
                healbarBgFill,
                new Vector2(175, Main.Self.WindowHeight - _healbarBorder.Height - 25),
                null,
                Color.Black,
                0f,
                Vector2.Zero,
                new Vector2(_healbarBorder.Width, 98),
                SpriteEffects.None,
                1f);

            spriteBatch.Draw(
                _healbarFill,
                new Vector2(175, Main.Self.WindowHeight - _healbarBorder.Height - 25),
                null,
                new Color(Color.White, 0.8f),
                0f,
                Vector2.Zero,
                new Vector2(Boss.HitPointsPercentage, 1f),
                SpriteEffects.None,
                1f);

            spriteBatch.Draw(_healbarBorder, new Vector2(175, Main.Self.WindowHeight - _healbarBorder.Height - 25));

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
                        X = Main.Self.WindowWidth / 3f,
                        Y = Main.Self.WindowHeight / 3f,
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
                        X = Main.Self.WindowWidth / 3f - 250,
                        Y = Main.Self.WindowHeight / 2f + 100,
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

