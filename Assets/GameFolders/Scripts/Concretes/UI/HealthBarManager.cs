using UnityEngine;
using UnityEngine.UI;

namespace HHGArchero.UI
{
    public class HealthBarManager : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - _camera.transform.position);
        }

        public void UpdateHealthBar(int maxHealth, int currentHealth)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }
    }
}
