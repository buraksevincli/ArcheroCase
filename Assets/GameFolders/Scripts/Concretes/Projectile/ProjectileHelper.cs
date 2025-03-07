using HHGArchero.Managers;
using UnityEngine;

namespace HHGArchero.Projectile
{
    public static class ProjectileHelper
    {
        public static bool CalculateLaunchVelocity(Vector3 startPos, Vector3 targetPos, out Vector3 launchVelocity)
        {
            launchVelocity = Vector3.zero;
            float angleRadians = DataManager.Instance.ProjectileData.ProjectileAngle * Mathf.Deg2Rad;
            Vector3 toTarget = targetPos - startPos;
            Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);
            float distance = toTargetXZ.magnitude;
            float yOffset = toTarget.y;
            float gravity = Mathf.Abs(Physics.gravity.y);
            
            float cosAngle = Mathf.Cos(angleRadians);
            float sinAngle = Mathf.Sin(angleRadians);
            float denominator = distance * Mathf.Tan(angleRadians) - yOffset;
            if (denominator <= 0)
                return false;
        
            float initialSpeed = Mathf.Sqrt((gravity * distance * distance) / (2 * denominator * cosAngle * cosAngle));
            if (float.IsNaN(initialSpeed))
                return false;
        
            launchVelocity = toTargetXZ.normalized * (initialSpeed * cosAngle) + Vector3.up * (initialSpeed * sinAngle);
            return true;
        }
    }
}

