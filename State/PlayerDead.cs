namespace Game
{
    public class PlayerDead : IState
    {
        private readonly MorpehECS _morpehEcs;

        public PlayerDead(MorpehECS morpehEcs)
        {
            _morpehEcs = morpehEcs;
        }

        public void Enter()
        {
            GameplayEvents.GameFinished?.Invoke();
            
            UIEvents.PlayerDeath.MenuPressed += OnMenuPressed;
            UIEvents.PlayerDeath.RestartPressed += OnRestartPressed;
        }

        public void Exit()
        {
            UIEvents.PlayerDeath.MenuPressed -= OnMenuPressed;
            UIEvents.PlayerDeath.RestartPressed -= OnRestartPressed;
        }

        private void OnMenuPressed()
        {
            // _morpehEcs.Dispose();
        }

        private void OnRestartPressed()
        {
            // _morpehEcs.Dispose();
        }
    }
}