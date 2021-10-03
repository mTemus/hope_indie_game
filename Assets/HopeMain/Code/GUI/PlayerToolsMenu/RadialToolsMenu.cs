using System.Collections.Generic;
using Code.System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.GUI.PlayerToolsMenu
{
    public class RadialToolsMenu : MonoBehaviour
    {
        [Header("General")] 
        [SerializeField] private Camera UICamera;
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
    
        private int currentMenuToolIndex;
        private int previousMenuToolIndex;

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
            currentMenuToolIndex = 0;
            menuElements[currentMenuToolIndex].ButtonBackground.color = highlightedButtonColor;
        }

        public void ChangeCurrentMenuElement(int value)
        {
            currentMenuToolIndex += value;

            if (currentMenuToolIndex >= menuElements.Count) {
                currentMenuToolIndex = 0;
            } else if (currentMenuToolIndex < 0) {
                currentMenuToolIndex = menuElements.Count - 1;
            }

            if (currentMenuToolIndex == previousMenuToolIndex) return;
            
            menuElements[previousMenuToolIndex].ButtonBackground.color = normalButtonColor;
            previousMenuToolIndex = currentMenuToolIndex;
            menuElements[currentMenuToolIndex].ButtonBackground.color = highlightedButtonColor;
            RefreshInformalCenter();
        }

        public void SelectTool()
        {
            Managers.I.Tools.SelectTool(currentMenuToolIndex);
        }

        private void RefreshInformalCenter()
        {
            toolName.text = menuElements[currentMenuToolIndex].ToolName;
            toolDescription.text = menuElements[currentMenuToolIndex].ToolDescription;
            toolIcon.sprite = menuElements[currentMenuToolIndex].ToolIcon;
        }

        public void Activate()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
            backgroundPanel.SetActive(true);
            RefreshInformalCenter();
        }

        public void Deactivate()
        {
            backgroundPanel.SetActive(false);
        }
    }
}
