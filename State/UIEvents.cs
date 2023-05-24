using System;

namespace Game
{
    public static class GameplayEvents
    {
        public static Action PlayerDied;
        public static Action GameFinished;
    }
    
    public static class UIEvents
    {
        public static class MainMenu
        {
            public static Action PlayPressed;
            public static Action QuitPressed;
        }

        public static class PlayerDeath
        {
            public static Action RestartPressed;
            public static Action MenuPressed;
        }
    }
}