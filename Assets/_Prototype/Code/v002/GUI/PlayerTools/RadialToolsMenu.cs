using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Prototype.Code.v002.GUI.PlayerTools
{
    /// <summary>
    /// UI menu responsible for handling user tools selection logic
    /// </summary>
    public class RadialToolsMenu : MonoBehaviour
    {
        [Header("General")] 
        [SerializeField] private GameObject backgroundPanel;
        [SerializeField] private GameObject circleMenuButtonPrefab;

        [Header("Buttons")] 
        [SerializeField] private Color normalButtonColor;
        [SerializeField] private Color highlightedButtonColor;

        [Header("Informal center")] 
        [SerializeField] private Text toolName;
        [SerializeField] private Text toolDescription;
        [SerializeField] private Image toolIcon;

        [Header("Buttons")] 
        [SerializeField] private List<CircleMenuButtonData> menuButtonsData;

        private List<CircleMenuButton> _menuButtons;

        [Inject]
        private Player.Tools.PlayerTools _playerTools;
        
        private int _currentMenuToolIndex;
        private int _previousMenuToolIndex;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            float rotationalIncrementalValue = 360f / menuButtonsData.Count;
            float currentRotationValue = 0f;
            _menuButtons = new List<CircleMenuButton>();
            
            for (int i = 0; i < menuButtonsData.Count; i++) {
                GameObject menuElementGO = Instantiate(circleMenuButtonPrefab, backgroundPanel.transform, true);
            
                menuElementGO.name = i + "_" + menuButtonsData[i].ToolName;
                menuElementGO.transform.localScale = Vector3.one;
                menuElementGO.transform.localPosition = Vector3.zero;
                menuElementGO.transform.rotation = Quaternion.Euler(0f, 0f, currentRotationValue);
            
                currentRotationValue += rotationalIncrementalValue;

                CircleMenuButton circleMenuButton = menuElementGO.GetComponent<CircleMenuButton>();
                circleMenuButton.IconImage.sprite = menuButtonsData[i].ToolIcon;
                circleMenuButton.IconRectTransform.rotation = Quaternion.identity;
                
                _menuButtons.Add(circleMenuButton);
            }
            _currentMenuToolIndex = 0;
            _menuButtons[_currentMenuToolIndex].BackgroundImage.color = highlightedButtonColor;
            circleMenuButtonPrefab = null;
        }

        private void RefreshInformalCenter()
        {
            toolName.text = menuButtonsData[_currentMenuToolIndex].ToolName;
            toolDescription.text = menuButtonsData[_currentMenuToolIndex].ToolDescription;
            toolIcon.sprite = menuButtonsData[_currentMenuToolIndex].ToolIcon;
        }
        
        /// <summary>
        /// Set pointer on other button and highlight its background
        /// </summary>
        /// <param name="value">Index of a button</param>
        public void ChangeCurrentMenuElement(int value)
        {
            _currentMenuToolIndex += value;

            if (_currentMenuToolIndex >= menuButtonsData.Count) {
                _currentMenuToolIndex = 0;
            } else if (_currentMenuToolIndex < 0) {
                _currentMenuToolIndex = menuButtonsData.Count - 1;
            }

            if (_currentMenuToolIndex == _previousMenuToolIndex) return;
            
            _menuButtons[_currentMenuToolIndex].BackgroundImage.color = normalButtonColor;
            _previousMenuToolIndex = _currentMenuToolIndex;
            _menuButtons[_currentMenuToolIndex].BackgroundImage.color = highlightedButtonColor;
            RefreshInformalCenter();
        }

        /// <summary>
        /// Tell tools logic which tool is selected in the UI
        /// </summary>
        public void SelectTool()
        {
           _playerTools.SelectTool(_currentMenuToolIndex);
        }
        
        /// <summary>
        /// Activate the UI
        /// </summary>
        public void Activate()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
            backgroundPanel.SetActive(true);
            RefreshInformalCenter();
        }

        /// <summary>
        /// Deactivate the UI
        /// </summary>
        public void Deactivate()
        {
            backgroundPanel.SetActive(false);
        }
    }
}
