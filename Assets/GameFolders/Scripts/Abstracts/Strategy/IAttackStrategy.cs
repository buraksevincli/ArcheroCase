namespace HHGArchero.Strategy
{
    public interface IAttackStrategy
    {
        public void OnEnter(AttackContext context);
        public void OnUpdate(AttackContext context);
        public void OnExit(AttackContext context);
    }
}