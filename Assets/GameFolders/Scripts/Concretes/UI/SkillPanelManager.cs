using HHGArchero.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace HHGArchero.UI
{
    public class SkillPanelManager : MonoBehaviour
    {
        [Header("Panel & UI Elements")]
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject pauseText;
        [SerializeField] private Button panelButton;

        [Header("Skill Buttons")]
        [SerializeField] private Button multiplicationButton;
        [SerializeField] private Button bounceButton;
        [SerializeField] private Button burnButton;
        [SerializeField] private Button speedButton;
        [SerializeField] private Button rageButton;

        [Header("Colors")]
        [SerializeField] private Color activeColor = Color.green;
        [SerializeField] private Color inactiveColor = Color.white;

        private bool _isPanelOpen;
        private bool _isPaused;

        private bool _isMultiplicationActive;
        private bool _isBounceActive;
        private bool _isBurnActive;
        private bool _isSpeedActive;
        private bool _isRageActive;

        private void Start()
        {
            panelButton.onClick.AddListener(TogglePanel);
            multiplicationButton.onClick.AddListener(() => ToggleSkill(ref _isMultiplicationActive, multiplicationButton, DataManager.Instance.EventData.ProjectileMultiplication));
            bounceButton.onClick.AddListener(() => ToggleSkill(ref _isBounceActive, bounceButton, DataManager.Instance.EventData.ProjectileBounce));
            burnButton.onClick.AddListener(() => ToggleSkill(ref _isBurnActive, burnButton, DataManager.Instance.EventData.ProjectileBurn));
            speedButton.onClick.AddListener(() => ToggleSkill(ref _isSpeedActive, speedButton, DataManager.Instance.EventData.ProjectileFireSpeed));
            rageButton.onClick.AddListener(() => ToggleSkill(ref _isRageActive, rageButton, DataManager.Instance.EventData.RageMode));
        }

        private void TogglePanel()
        {
            _isPanelOpen = !_isPanelOpen;
            _isPaused = !_isPaused;

            panel.SetActive(_isPanelOpen);
            pauseText.SetActive(_isPaused);
            panelButton.image.color = _isPanelOpen ? activeColor : inactiveColor;

            GameManager.Instance.IsPaused = _isPanelOpen;
        }

        /// <summary>
        /// Toggles a skill on/off, changes button color, and invokes the relevant event.
        /// </summary>
        private void ToggleSkill(ref bool skillFlag, Button skillButton, System.Action skillEvent)
        {
            skillFlag = !skillFlag;
            skillButton.image.color = skillFlag ? activeColor : inactiveColor;
            skillEvent?.Invoke();
        }
    }
}
