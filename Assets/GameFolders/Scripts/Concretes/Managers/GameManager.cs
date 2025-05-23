using System;
using HHGArchero.Utilities;

namespace HHGArchero.Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private bool _isPaused;

        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                _isPaused = value;
                OnGamePaused?.Invoke(_isPaused);
            }
        }

        public event Action<bool> OnGamePaused;
    }
}