using UnityEngine;

namespace Game
{
    public class MainMenuUIPresenter : MonoBehaviour
    {
        public MainMenuUIView View;

        private void Start()
        {
            View.PlayButton.onClick.AddListener(() => UIEvents.MainMenu.PlayPressed?.Invoke());
            View.QuitButton.onClick.AddListener(() => UIEvents.MainMenu.QuitPressed?.Invoke());
        }
    }
}