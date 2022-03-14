using System.Collections.Generic;
using _Prototype.Code.v001.System;
using UnityEngine;
using UnityEngine.UI;

namespace _Prototype.Code.v001.GUI.Player.ToolsMenu
{
    /// <summary>
    /// 
    /// </summary>
    public class RadialToolsMenu : MonoBehaviour
    {
        [Header("General")] 
        [SerializeField] private Camera uiCamera;
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
        [SerializeField] private List<CircleMenuElement> menuElements;
    
        private int _currentMenuToolIndex;
        private int _previousMenuToolIndex;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            float rotationalIncrementalValue = 360f / menuElements.Count;
            float currentRotationValue = 0f;
        
            for (int i = 0; i < menuElements.Count; i++) {
                GameObject menuElementGO = Instantiate(circleMenuButtonPrefab, backgroundPanel.transform, true);
            
                menuElementGO.name = i + "_" + menuElements[i].ToolName;
                menuElementGO.transform.localScale = Vector3.one;
                menuElementGO.transform.localPosition = Vector3.zero;
                menuElementGO.transform.rotation = Quaternion.Euler(0f, 0f, currentRotationValue);
            
                currentRotationValue += rotationalIncrementalValue;

                CircleMenuButton circleMenuButton = menuElementGO.GetComponent<CircleMenuButton>();
                menuElements[i].ButtonBackground = circleMenuButton.BackgroundImage;
            
                circleMenuButton.IconImage.sprite = menuElements[i].ToolIcon;
                circleMenuButton.IconRectTransform.rotation = Quaternion.identity;
            }
            _currentMenuToolIndex = 0;
            menuElements[_currentMenuToolIndex].ButtonBackground.color = highlightedButtonColor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void ChangeCurrentMenuElement(int value)
        {
            _currentMenuToolIndex += value;

            if (_currentMenuToolIndex >= menuElements.Count) {
                _currentMenuToolIndex = 0;
            } else if (_currentMenuToolIndex < 0) {
                _currentMenuToolIndex = menuElements.Count - 1;
            }

            if (_currentMenuToolIndex == _previousMenuToolIndex) return;
            
            menuElements[_previousMenuToolIndex].ButtonBackground.color = normalButtonColor;
            _previousMenuToolIndex = _currentMenuToolIndex;
            menuElements[_currentMenuToolIndex].ButtonBackground.color = highlightedButtonColor;
            RefreshInformalCenter();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SelectTool()
        {
            Managers.I.Tools.SelectTool(_currentMenuToolIndex);
        }

        private void RefreshInformalCenter()
        {
            toolName.text = menuElements[_currentMenuToolIndex].ToolName;
            toolDescription.text = menuElements[_currentMenuToolIndex].ToolDescription;
            toolIcon.sprite = menuElements[_currentMenuToolIndex].ToolIcon;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Activate()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
            backgroundPanel.SetActive(true);
            RefreshInformalCenter();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Deactivate()
        {
            backgroundPanel.SetActive(false);
        }
    }
}
