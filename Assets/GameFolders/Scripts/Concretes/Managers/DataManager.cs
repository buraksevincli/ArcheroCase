using HHGArchero.Scriptables;
using HHGArchero.Utilities;
using UnityEngine;

namespace HHGArchero.Managers
{
    public class DataManager : MonoSingleton<DataManager>
    {
        [SerializeField] private EventData eventData;
        [SerializeField] private GameData gameData;
        
        public EventData EventData => eventData;
        public GameData GameData => gameData;
    }
}