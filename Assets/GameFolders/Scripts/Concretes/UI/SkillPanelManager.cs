using HHGArchero.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace HHGArchero.UI
{
    public class SkillPanelManager : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Button panelButton;
        [SerializeField] private Button multiplicationButton;
        [SerializeField] private Button bounceButton;
        [SerializeField] private Button burnButton;
        [SerializeField] private Button speedButton;
        [SerializeField] private Button rageButton;
        private bool _isPanelOpen;
        
        private void Start()
        {
            panelButton.onClick.AddListener(PanelButtonClicked);
            multiplicationButton.onClick.AddListener(MultiplicationButtonClicked);
            bounceButton.onClick.AddListener(BounceButtonClicked);
            burnButton.onClick.AddListener(BurnButtonClicked);
            speedButton.onClick.AddListener(SpeedButtonClicked);
            rageButton.onClick.AddListener(RageButtonClicked);
        }

        private void PanelButtonClicked()
        {
            _isPanelOpen = !_isPanelOpen;
            panel.SetActive(_isPanelOpen);
            GameManager.Instance.IsPaused = _isPanelOpen;
        }

        private void MultiplicationButtonClicked()
        {
            DataManager.Instance.EventData.ProjectileMultiplication?.Invoke();
        }

        private void BounceButtonClicked()
        {
            DataManager.Instance.EventData.ProjectileBounce?.Invoke();
        }

        private void BurnButtonClicked()
        {
            DataManager.Instance.EventData.ProjectileBurn?.Invoke();
        }
        
        private void SpeedButtonClicked()
        {
            DataManager.Instance.EventData.ProjectileFireSpeed?.Invoke();
        }

        private void RageButtonClicked()
        {
            DataManager.Instance.EventData.RageMode?.Invoke();
        }
    }
}
