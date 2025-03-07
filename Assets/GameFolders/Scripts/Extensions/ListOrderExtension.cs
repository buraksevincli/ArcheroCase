using System.Collections.Generic;
using UnityEngine;

namespace HHGArchero.Extensions
{
    public static class ListOrderExtension
    {
        /// <summary>
        /// Find closest target with sorting algorithm and return closest target transform.
        /// </summary>
        /// <returns></returns>
        public static Transform GetClosestTransform(this List<Transform> transforms, Transform target, Transform ignoredTransform = null)
        {
            if (transforms == null || transforms.Count == 0)
                return null;
            
            List<Transform> currentTransforms = new List<Transform>();
            currentTransforms.AddRange(transforms);
            
            if (ignoredTransform)
            {
                if (currentTransforms.Contains(ignoredTransform))
                {
                    currentTransforms.Remove(ignoredTransform);
                }
            }

            Transform closestTransform = currentTransforms[0];
            float minDistance = Vector3.Distance(target.position, currentTransforms[0].position);

            foreach (Transform transform in currentTransforms)
            {
                float distance = Vector3.Distance(target.position, transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTransform = transform;
                }
            }

            return closestTransform;
        }
    }
}