using UnityEngine;

namespace HHGArchero.Scriptables
{
    [CreateAssetMenu(fileName = "EventData", menuName = "Data/Event Data")]
    public class EventData : ScriptableObject
    {
        public System.Action Test { get; set; }
    }
}