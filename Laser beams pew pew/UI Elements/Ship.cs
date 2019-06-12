using System.Collections.Generic;
using Laser_beams_pew_pew.UI_Elements.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Laser_beams_pew_pew.UI_Elements
{
    public class Ship: IUiElement
    {
        private readonly List<Laser> _lasers;
        private double _lastShotTimer;

        public Ship(List<Laser> lasers)
        {
            Texture = Main.Self.Content.Load<Texture2D>("images/ship");
            Position = new Vector2(20, Main.Self.WindowHeight / 2 - Texture.Height / 2);
            Speed = 5;
            _lasers = lasers;
        }

        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public float Speed { get; set; }

        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Up) && Position.Y > 0)
            {
                Position -= new Vector2(0, Speed);
            }
            if (keyboardState.IsKeyDown(Keys.Down) &&
                Position.Y + Texture.Height < Main.Self.WindowHeight)
            {
                Position += new Vector2(0, Speed);
            }

            
            if (keyboardState.IsKeyDown(Keys.Space) && gameTime.TotalGameTime.TotalMilliseconds - _lastShotTimer > 500)
            {
                _lasers.Add(new Laser(Position));
                _lastShotTimer = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }
    }
}
