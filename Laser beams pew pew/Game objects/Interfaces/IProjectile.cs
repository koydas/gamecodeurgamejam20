using Microsoft.Xna.Framework;

namespace Laser_beams_pew_pew.Game_objects.Interfaces
{
    public interface IProjectile : IGameObject
    {
        bool HasHitSomething { get; set; }
        bool IsExploding { get; set; }
    }
}
