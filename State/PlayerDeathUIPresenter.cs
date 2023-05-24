using UnityEngine;

namespace Game
{
    public class PlayerDeathUIPresenter : MonoBehaviour
    {
        public PlayerDeathUIView View;

        private void Start()
        {
            View.RestartButton.onClick.AddListener(() => UIEvents.PlayerDeath.RestartPressed?.Invoke());
            View.MenuButton.onClick.AddListener(() => UIEvents.PlayerDeath.MenuPressed?.Invoke());
        }
    }
}