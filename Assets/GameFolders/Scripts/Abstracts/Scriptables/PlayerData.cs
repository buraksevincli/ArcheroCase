using HHGArchero.Enemy;
using HHGArchero.Projectile;
using UnityEngine;

namespace HHGArchero.Scriptables
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Player Data")]
    public class PlayerData : ScriptableObject
    { 
        [Header("Player Settings")]
        [SerializeField] private int moveSpeed = 5;
        
        public int MoveSpeed => moveSpeed;
    }
}
