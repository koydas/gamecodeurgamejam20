using System.Collections.Generic;
using Laser_beams_pew_pew.Game_objects;
using Laser_beams_pew_pew.Game_objects.Projectiles;
using Laser_beams_pew_pew.Scenes.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laser_beams_pew_pew.Scenes
{
    public class GamePlay: IScene
    {
        public Player Player;
        public LaserBoss LaserBoss;

        public List<Bullet> Bullets = new List<Bullet>();
        public List<Laser> Lasers = new List<Laser>();
        
        public GamePlay()
        {
            Main.Self.IsMouseVisible = false;

            Player = new Player(Bullets);
            LaserBoss = new LaserBoss(Lasers, Player);
        }

        public void Update(GameTime gameTime)
        {
            Player.Update(gameTime);
            LaserBoss.Update(gameTime);

            for (var index = 0; index < Bullets.Count; index++)
            {
                var bullet = Bullets[index];

                bullet.Update(gameTime);

                LaserBoss.IsHit(bullet);

                if (bullet.Position.X > Main.Self.WindowWidth || bullet.HasHitSomething)
                {
                    Bullets.RemoveAt(index);
                }
            }

            for (var index = 0; index < Lasers.Count; index++)
            {
                var laser = Lasers[index];

                laser.Update(gameTime);

                Player.IsHit(laser);

                if (laser.Position.X > Main.Self.WindowWidth || laser.HasHitSomething)
                {
                    Lasers.RemoveAt(index);
                }
            }

            if (LaserBoss.HitPoints <= 0 || Player.HitPoints <= 0)
            {
                Main.Self.CurrentScene = new Menu();
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Main.Self.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            Player.Draw(spriteBatch, gameTime);
            LaserBoss.Draw(spriteBatch, gameTime);

            foreach (var bullet in Bullets)
            {
                bullet.Draw(spriteBatch, gameTime);
            }

            foreach (var laser in Lasers)
            {
                laser.Draw(spriteBatch, gameTime);
            }

            spriteBatch.End();
        }
    }
}
