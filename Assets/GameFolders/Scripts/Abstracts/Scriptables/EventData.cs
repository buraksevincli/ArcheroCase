using UnityEngine;

namespace HHGArchero.Scriptables
{
    [CreateAssetMenu(fileName = "EventData", menuName = "Data/Event Data")]
    public class EventData : ScriptableObject
    {
        public System.Action ProjectileMultiplication { get; set; }
        
        public System.Action ProjectileBounce { get; set; }
        public System.Action ProjectileBurn { get; set; }
        public System.Action ProjectileFireSpeed { get; set; }
        public System.Action RageMode { get; set; }
    }
}