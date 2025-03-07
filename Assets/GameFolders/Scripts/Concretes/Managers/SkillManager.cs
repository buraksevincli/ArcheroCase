using System;
using HHGArchero.Scriptables;
using HHGArchero.Utilities;

namespace HHGArchero.Managers
{
    public class SkillManager : MonoSingleton<SkillManager>
    {
        private SkillData _skillData;

        private bool _multiply;
        private bool _bounce;
        private bool _burn;
        private bool _speed;
        private bool _rage;

        private int _projectileMultiplicationCount = 1;
        private int _projectileBounceCount = 0;
        private int _projectileBurnTime = 0;
        private int _projectileFireSpeedCount = 1;

        public int ProjectileMultiplicationCount => _projectileMultiplicationCount;
        public int ProjectileBounceCount => _projectileBounceCount;
        public int ProjectileBurnTime => _projectileBurnTime;
        public int ProjectileFireSpeedCount => _projectileFireSpeedCount;

        protected override void Awake()
        {
            base.Awake();
            _skillData = DataManager.Instance.SkillData;
        }

        private void OnEnable()
        {
            DataManager.Instance.EventData.ProjectileMultiplication += ProjectileMultiplicationHandler;
            DataManager.Instance.EventData.ProjectileBounce += ProjectileBounceHandler;
            DataManager.Instance.EventData.ProjectileBurn += ProjectileBurnHandler;
            DataManager.Instance.EventData.ProjectileFireSpeed += ProjectileFireSpeedHandler;
            DataManager.Instance.EventData.RageMode += RageModeHandler;
        }

        private void OnDisable()
        {
            DataManager.Instance.EventData.ProjectileMultiplication -= ProjectileMultiplicationHandler;
            DataManager.Instance.EventData.ProjectileBounce -= ProjectileBounceHandler;
            DataManager.Instance.EventData.ProjectileBurn -= ProjectileBurnHandler;
            DataManager.Instance.EventData.ProjectileFireSpeed -= ProjectileFireSpeedHandler;
            DataManager.Instance.EventData.RageMode -= RageModeHandler;
        }

        private void RecalculateMultipliers()
        {
            _projectileMultiplicationCount = GetMultiplier(_multiply, normal: _skillData.MultiplicationCount, rage: _skillData.MultiplicationCountWithRage, defaultValue: _skillData.MultiplicationDefault);
            _projectileBounceCount = GetMultiplier(_bounce, normal: _skillData.BounceCount, rage: _skillData.BounceCountWithRage, defaultValue: _skillData.BounceDefault);
            _projectileBurnTime = GetMultiplier(_burn, normal: _skillData.BurnTime, rage: _skillData.BurnTimeWithRage, defaultValue: _skillData.BurnDefault);
            _projectileFireSpeedCount = GetMultiplier(_speed, normal: _skillData.AttackSpeedCount, rage: _skillData.AttackSpeedCountWithRage, defaultValue: _skillData.AttackSpeedDefault);
        }

        private int GetMultiplier(bool isEnabled, int normal, int rage, int defaultValue)
        {
            return isEnabled ? (_rage ? rage : normal) : defaultValue;
        }

        private void ProjectileMultiplicationHandler()
        {
            _multiply = !_multiply;
            RecalculateMultipliers();
        }

        private void ProjectileBounceHandler()
        {
            _bounce = !_bounce;
            RecalculateMultipliers();
        }

        private void ProjectileBurnHandler()
        {
            _burn = !_burn;
            RecalculateMultipliers();
        }

        private void ProjectileFireSpeedHandler()
        {
            _speed = !_speed;
            RecalculateMultipliers();
        }

        private void RageModeHandler()
        {
            _rage = !_rage;
            RecalculateMultipliers();
        }
    }
}