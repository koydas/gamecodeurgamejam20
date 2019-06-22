using System.Collections.Generic;
using Laser_beams_pew_pew.Game_objects.Interfaces;
using Laser_beams_pew_pew.Game_objects.Projectiles;
using Microsoft.Xna.Framework;

namespace Laser_beams_pew_pew.Game_objects.Bosses
{
    public abstract class Boss : GameObject, IBoss
    {
        protected Boss(List<IProjectile> projectile, Player player)
        {
        }
    }
}
