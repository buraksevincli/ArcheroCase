using HHGArchero.Managers;
using HHGArchero.Player;
using HHGArchero.Strategies;

namespace HHGArchero.StateMachine
{
    public class AttackState : IPlayerState
    {
        private IAttackStrategy _currentStrategy;
        private AttackContext _context;

        public void EnterState(PlayerController player)
        {
            _context = new AttackContext(player);
            _currentStrategy = DecideStrategy();
            _currentStrategy.OnEnter(_context);

            SkillManager.Instance.OnSkillStateChanged += HandleSkillChange;
        }

        public void UpdateState(PlayerController player)
        {
            _currentStrategy.OnUpdate(_context);
        }

        public void FixedUpdateState(PlayerController player) { }

        public void ExitState(PlayerController player)
        {
            _currentStrategy.OnExit(_context);
            SkillManager.Instance.OnSkillStateChanged -= HandleSkillChange;
        }

        private void HandleSkillChange(bool skillActive)
        {
            _currentStrategy.OnExit(_context);
            _currentStrategy = DecideStrategy();
            _currentStrategy.OnEnter(_context);
        }

        private IAttackStrategy DecideStrategy()
        {
            return SkillManager.Instance.IsSkillActivated()
                ? new AttackWithSkill()
                : new NormalAttackStrategy();
        }
    }
}
