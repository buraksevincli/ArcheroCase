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
        public bool IsRage => _rage;

        public event Action<bool> OnSkillStateChanged;
        private bool _skillActive;

        public bool SkillActivated
        {
            get => _skillActive;
            set
            {
                if (_skillActive == value) return;
                _skillActive = value;
                OnSkillStateChanged?.Invoke(_skillActive);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            InitializeData();
        }

        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnsubscribeEvents();

        private void InitializeData() => _skillData = DataManager.Instance.SkillData;

        private void SubscribeEvents()
        {
            EventData eventData = DataManager.Instance.EventData;
            eventData.ProjectileMultiplication += ProjectileMultiplicationHandler;
            eventData.ProjectileBounce += ProjectileBounceHandler;
            eventData.ProjectileBurn += ProjectileBurnHandler;
            eventData.ProjectileFireSpeed += ProjectileFireSpeedHandler;
            eventData.RageMode += RageModeHandler;
        }

        private void UnsubscribeEvents()
        {
            EventData eventData = DataManager.Instance.EventData;
            eventData.ProjectileMultiplication -= ProjectileMultiplicationHandler;
            eventData.ProjectileBounce -= ProjectileBounceHandler;
            eventData.ProjectileBurn -= ProjectileBurnHandler;
            eventData.ProjectileFireSpeed -= ProjectileFireSpeedHandler;
            eventData.RageMode -= RageModeHandler;
        }

        private void ProjectileMultiplicationHandler() => ToggleSkill(ref _multiply);
        private void ProjectileBounceHandler() => ToggleSkill(ref _bounce);
        private void ProjectileBurnHandler() => ToggleSkill(ref _burn);
        private void ProjectileFireSpeedHandler() => ToggleSkill(ref _speed);
        private void RageModeHandler() => ToggleSkill(ref _rage);

        private void ToggleSkill(ref bool skillFlag)
        {
            skillFlag = !skillFlag;
            RecalculateMultipliers();
            SkillActivated = IsSkillActivated();
        }

        private void RecalculateMultipliers()
        {
            _projectileMultiplicationCount = GetMultiplier(
                _multiply,
                _skillData.MultiplicationCount,
                _skillData.MultiplicationCountWithRage,
                _skillData.MultiplicationDefault
            );

            _projectileBounceCount = GetMultiplier(
                _bounce,
                _skillData.BounceCount,
                _skillData.BounceCountWithRage,
                _skillData.BounceDefault
            );

            _projectileBurnTime = GetMultiplier(
                _burn,
                _skillData.BurnTime,
                _skillData.BurnTimeWithRage,
                _skillData.BurnDefault
            );

            _projectileFireSpeedCount = GetMultiplier(
                _speed,
                _skillData.AttackSpeedCount,
                _skillData.AttackSpeedCountWithRage,
                _skillData.AttackSpeedDefault
            );
        }

        private int GetMultiplier(bool isEnabled, int normal, int rage, int defaultValue)
            => isEnabled ? (_rage ? rage : normal) : defaultValue;

        public bool IsSkillActivated()
            => _multiply || _bounce || _burn || _speed;
    }
}
