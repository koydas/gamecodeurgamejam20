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
        public Ship Ship;
        public Boss Boss;

        public List<Bullet> Lasers = new List<Bullet>();
        
        public GamePlay()
        {
            Ship = new Ship(Lasers);
            Boss = new Boss();
        }

        public void Update(GameTime gameTime)
        {
            Ship.Update(gameTime);
            Boss.Update(gameTime);

            for (var index = 0; index < Lasers.Count; index++)
            {
                var laser = Lasers[index];

                laser.Update(gameTime);

                Boss.IsHit(laser);

                if (laser.Position.X > Main.Self.WindowWidth || laser.HasHitSomething)
                {
                    Lasers.RemoveAt(index);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Main.Self.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            Ship.Draw(spriteBatch, gameTime);
            Boss.Draw(spriteBatch, gameTime);

            foreach (var laser in Lasers)
            {
                laser.Draw(spriteBatch, gameTime);
            }

            spriteBatch.End();
        }
    }
}
