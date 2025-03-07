using UnityEngine;

namespace HHGArchero.Scriptables
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "Data/Skill Data")]
    public class SkillData : ScriptableObject
    {
        [Header("Multiplication Settings")] 
        [SerializeField] private int multiplicationDefault = 1;
        [SerializeField] private int multiplicationCount = 2;
        [SerializeField] private int multiplicationCountWithRage = 4;

        [Header("Bounce Settings")] 
        [SerializeField] private int bounceDefault = 0;
        [SerializeField] private int bounceCount = 1;
        [SerializeField] private int bounceCountWithRage = 2;
        [Header("Burn Settings")] 
        [SerializeField] private int burnDefault = 0;
        [SerializeField] private int burnTime = 3;
        [SerializeField] private int burnTimeWithRage = 6;

        [Header("Fire Speed Settings")]
        [SerializeField] private int attackSpeedDefault = 1;
        [SerializeField] private int attackSpeedCount = 2;
        [SerializeField] private int attackSpeedCountWithRage = 4;

        public int MultiplicationDefault => multiplicationDefault;
        public int MultiplicationCount => multiplicationCount;
        public int MultiplicationCountWithRage => multiplicationCountWithRage;
        public int BounceDefault => bounceDefault;
        public int BounceCount => bounceCount;
        public int BounceCountWithRage => bounceCountWithRage;
        public int BurnDefault => burnDefault;
        public int BurnTime => burnTime;
        public int BurnTimeWithRage => burnTimeWithRage;
        public int AttackSpeedDefault => attackSpeedDefault;
        public int AttackSpeedCount => attackSpeedCount;
        public int AttackSpeedCountWithRage => attackSpeedCountWithRage;
    }
}