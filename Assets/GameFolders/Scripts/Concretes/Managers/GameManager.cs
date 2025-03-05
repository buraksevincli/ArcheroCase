using System;
using HHGArchero.Utilities;
using UnityEngine;

namespace HHGArchero.Managers
{
    [DefaultExecutionOrder(-50)]
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
