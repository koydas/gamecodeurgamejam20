﻿using System;
using System.Collections.Generic;
using Laser_beams_pew_pew.Game_objects.Interfaces;
using Laser_beams_pew_pew.Game_objects.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laser_beams_pew_pew.Game_objects.Bosses
{
    public class LaserBoss : Boss
    {
        public override int HitPoints { get; set; }
        private readonly List<IProjectile> _lasers;
        private double _lastShotTimer;
        private readonly Player _player;
        private double _lastMovementChange;
        private double _lastMovementChangeCoolDown;
        private readonly Random _random = new Random();

        private Vector2 LaserPosition => new Vector2
        {
            X = Position.X + HitBox.Width / 10f,
            Y = Position.Y + HitBox.Height / 3f * 1.8f,
        };

        // Special Move fields
        private bool _whileSpecialMove;
        private bool _specialMoveGotToBottom;

        // Cone Attack
        public bool WhileConeAttack;
        private double _coolDownConeAttack;
        private double _lastConeTimer;

        public LaserBoss(List<IProjectile> projectile, Player player) : base(projectile, player)
        {
            Scale = 0.5f;

            HitPoints = 50;
            MaxHitPoints = HitPoints;
            _lasers = projectile;
            _player = player;

            Texture = Main.Self.Content.Load<Texture2D>("images/ennemy-ship");

            Position = new Vector2
            {
                X = Main.Self.WindowWidth - Texture.Width,
                Y = 0
            };
        }

        public override void Update(GameTime gameTime)
        {
            var elapsedTime = gameTime.TotalGameTime.TotalMilliseconds;

            // phase 1
            var lifePercentage = HitPoints / (float)MaxHitPoints;

            if (lifePercentage > 0.75f)
            {
                Speed = 4;
                ShootLaser(elapsedTime);
                Move(elapsedTime);
            }
            // Phase 2
            else if (lifePercentage > .5f)
            {
                Speed = 6;
                if (!SpecialMove(elapsedTime))
                {
                    // Like Phase 1
                    Move(elapsedTime);
                    ShootLaser(elapsedTime);
                }
            }
            // Phase 3
            else
            {
                Speed = 8;
                if (!ConeAttack(elapsedTime))
                {
                    // Like Phase 1
                    ShootLaser(elapsedTime);
                    Move(elapsedTime);
                }
            }
        }

        private bool SpecialMove(double elapsedTime)
        {
            if (_whileSpecialMove || _random.Next(0, 150) == 11)
            {
                _whileSpecialMove = true;

                // Go to the bottom of the screen and to the top, shotting like crazy
                if (!_specialMoveGotToBottom && Position.Y < Main.Self.WindowHeight - HitBox.Height - 200)
                {
                    ShootLaser(elapsedTime, true);
                    Position += Vector2.UnitY * Speed;
                }
                else if (Position.Y > 0)
                {
                    ShootLaser(elapsedTime, true);
                    _specialMoveGotToBottom = true;
                    Position -= Vector2.UnitY * Speed;
                }
                else
                {
                    _specialMoveGotToBottom = false;
                    _whileSpecialMove = false;
                }

                return true;
            }

            return false;
        }

        private void Move(double elapsedTime)
        {
            MoveUpDown();
            MoveLeftRight(elapsedTime);
        }

        private void MoveLeftRight(double elapsedTime)
        {
            if (_lastMovementChangeCoolDown == 0 || elapsedTime - _lastMovementChangeCoolDown > 5000)
            {
                // Going front for 3sec
                if (_lastMovementChange == 0 || elapsedTime - _lastMovementChange < 3000 &&
                    Position.X > Main.Self.WindowWidth / 2f)
                {
                    if (_lastMovementChange == 0)
                        _lastMovementChange = elapsedTime;

                    Position -= Vector2.UnitX * Speed;

                }
                // go back
                else if (Position.X + HitBox.Width < Main.Self.WindowWidth)
                {
                    if (Position.X + HitBox.Width > Main.Self.WindowWidth)
                        _lastMovementChange = 0;

                    Position += Vector2.UnitX * Speed;
                }
                // 30sec cooldown
                else
                {
                    _lastMovementChange = 0;
                    _lastMovementChangeCoolDown = elapsedTime;
                }
            }
        }

        private void MoveUpDown()
        {
            // doesnt always follow
            if (_random.Next(0, 3) == 1) return;

            if (_player.Position.Y > LaserPosition.Y)
            {
                Position += Vector2.UnitY * Speed;
            }
            if (_player.Position.Y < LaserPosition.Y)
            {
                Position -= Vector2.UnitY * Speed;
            }
        }

        private void ShootLaser(double elapsedTime, bool burstMode = false)
        {
            var attackSpeed = burstMode ? 10 : 30;

            if (elapsedTime - _lastShotTimer > 500 && _random.Next(0, attackSpeed) == 5)
            {
                _lasers.Add(new Laser(LaserPosition, -180f));
                _lastShotTimer = elapsedTime;
            }
        }

        private bool ConeAttack(double elapsedTime)
        {
            if (_coolDownConeAttack != 0 && elapsedTime - _coolDownConeAttack < 5000)
            {
                _lastConeTimer = elapsedTime;
                return false;
            }

            if (elapsedTime - _lastConeTimer < 5000)
            {
                if (!WhileConeAttack)
                {
                    _lastConeTimer = elapsedTime;
                }

                WhileConeAttack = true;
            }
            else
            {
                if (WhileConeAttack || _coolDownConeAttack == 0)
                {
                    _coolDownConeAttack = elapsedTime;
                }

                WhileConeAttack = false;
            }


            if (WhileConeAttack)
            {
                var numberOfProjectiles = _random.Next(1, 20);

                for (int i = 0; i < numberOfProjectiles; i++)
                {
                    _lasers.Add(new Laser(LaserPosition, _random.Next(-200, -160)));
                }

                return true;
            }

            return false;
        }

        public override bool IsHit(IGameObject collider)
        {
            if (collider is Laser) return false;

            return base.IsHit(collider);
        }
    }
}
