using HHGArchero.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace HHGArchero.UI
{
    public class SkillPanelManager : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private GameObject panel;
        private bool _isPanelOpen;
        
        private void Start()
        {
            button.onClick.AddListener(ButtonClicked);
        }

        private void ButtonClicked()
        {
            _isPanelOpen = !_isPanelOpen;
            panel.SetActive(_isPanelOpen);
            GameManager.Instance.IsPaused = _isPanelOpen;
        }
    }
}
