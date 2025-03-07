using HHGArchero.Scriptables;
using HHGArchero.Utilities;
using UnityEngine;

namespace HHGArchero.Managers
{
    public class DataManager : MonoSingleton<DataManager>
    {
        [SerializeField] private EventData eventData;
        [SerializeField] private PlayerData playerData;
        [SerializeField] private EnemyData enemyData;
        [SerializeField] private ProjectileData projectileData;
        [SerializeField] private SkillData skillData;
        
        public EventData EventData => eventData;
        public PlayerData PlayerData => playerData;
        public EnemyData EnemyData => enemyData;
        public ProjectileData ProjectileData => projectileData;
        public SkillData SkillData => skillData;
    }
}