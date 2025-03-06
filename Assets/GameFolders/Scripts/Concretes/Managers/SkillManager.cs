using System;
using HHGArchero.Utilities;

namespace HHGArchero.Managers
{
    public class SkillManager : MonoSingleton<SkillManager>
    {
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
            if (_multiply)
            {
                _projectileMultiplicationCount = _rage ? 4 : 2;
            }
            else
            {
                _projectileMultiplicationCount = 1;
            }

            if (_bounce)
            {
                _projectileBounceCount = _rage ? 2 : 1;
            }
            else
            {
                _projectileBounceCount = 0;
            }

            if (_burn)
            {
                _projectileBurnTime = _rage ? 6 : 3;
            }
            else
            {
                _projectileBurnTime = 0;
            }

            if (_speed)
            {
                _projectileFireSpeedCount = _rage ? 4 : 2;
            }
            else
            {
                _projectileFireSpeedCount = 1;
            }
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