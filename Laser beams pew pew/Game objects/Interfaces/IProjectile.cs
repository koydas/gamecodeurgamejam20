namespace Laser_beams_pew_pew.Game_objects.Interfaces
{
    public interface IProjectile
    {
        bool HasHitSomething { get; set; }
        bool IsExploding { get; set; }
    }
}
