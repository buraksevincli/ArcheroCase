using HHGArchero.Managers;

namespace HHGArchero.Strategy
{
    public class AttackContext
    {
        public Player.PlayerController Player { get; }
        public float FireTimer { get; set; }
        public int ProjectilesFired { get; set; }
        public float LastProjectileTime { get; set; }
        public float FireRate { get; set; }
        public float ProjectileDelay { get; set; }

        public AttackContext(Player.PlayerController player)
        {
            Player = player;
            FireRate = DataManager.Instance.ProjectileData.FireRate;
            ProjectileDelay = DataManager.Instance.ProjectileData.ProjectileDelay;
            FireTimer = 0f;
            ProjectilesFired = 0;
            LastProjectileTime = 0f;
        }
    }
}