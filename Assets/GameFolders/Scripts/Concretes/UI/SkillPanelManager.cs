using HHGArchero.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace HHGArchero.UI
{
    public class SkillPanelManager : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject pauseText;
        [SerializeField] private Button panelButton;
        [SerializeField] private Button multiplicationButton;
        [SerializeField] private Button bounceButton;
        [SerializeField] private Button burnButton;
        [SerializeField] private Button speedButton;
        [SerializeField] private Button rageButton;
        
        private bool _isPanelOpen;
        private bool _isPaused;
        private bool _isMultiplicationActive = false;
        private bool _isBounceActive = false;
        private bool _isBurnActive = false;
        private bool _isSpeedActive = false;
        private bool _isRageActive = false;

        [SerializeField] private Color activeColor = Color.green;
        [SerializeField] private Color inactiveColor = Color.white;

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
            _isPaused = !_isPaused;
            panel.SetActive(_isPanelOpen);
            pauseText.SetActive(_isPaused);
            panelButton.image.color = _isPanelOpen ? activeColor : inactiveColor;
            GameManager.Instance.IsPaused = _isPanelOpen;
        }

        private void MultiplicationButtonClicked()
        {
            _isMultiplicationActive = !_isMultiplicationActive;
            DataManager.Instance.EventData.ProjectileMultiplication?.Invoke();
            multiplicationButton.image.color = _isMultiplicationActive ? activeColor : inactiveColor;
        }

        private void BounceButtonClicked()
        {
            _isBounceActive = !_isBounceActive;
            DataManager.Instance.EventData.ProjectileBounce?.Invoke();
            bounceButton.image.color = _isBounceActive ? activeColor : inactiveColor;
        }

        private void BurnButtonClicked()
        {
            _isBurnActive = !_isBurnActive;
            DataManager.Instance.EventData.ProjectileBurn?.Invoke();
            burnButton.image.color = _isBurnActive ? activeColor : inactiveColor;
        }
        
        private void SpeedButtonClicked()
        {
            _isSpeedActive = !_isSpeedActive;
            DataManager.Instance.EventData.ProjectileFireSpeed?.Invoke();
            speedButton.image.color = _isSpeedActive ? activeColor : inactiveColor;
        }

        private void RageButtonClicked()
        {
            _isRageActive = !_isRageActive;
            DataManager.Instance.EventData.RageMode?.Invoke();
            rageButton.image.color = _isRageActive ? activeColor : inactiveColor;
        }
    }
}
