using Code.GUI.UIElements;
using Code.GUI.UIElements.SelectableElement;
using Code.System;
using Code.System.PlayerInput;
using Code.Villagers.Entity;
using UnityEngine;
using UnityEngine.UI;

namespace Code.GUI.Villagers.Selecting
{
    public class VillagerProfessionChangingPanel : UiSelectablePanel
    {
        [Header("Properties")] 
        [SerializeField] private int normalLabelHeight;
        
        [Header("Components")]
        [SerializeField] private UiAcceptancePanel acceptancePanel;
        [SerializeField] private UiVerticalGroup professionsGroup;
        
        [Header("Additional pointers")] 
        [SerializeField] private Image currentProfessionPointer;
        [SerializeField] private ProfessionPropertiesLabel propertiesLabel;
        [SerializeField] private GameObject leftArrow;
        [SerializeField] private GameObject rightArrow;

        private int workplacesIdx;
        private ProfessionLabelItem currentProfession;

        private void Awake()
        {
            currentElement = elementsToSelect[0];
            gameObject.SetActive(false);
        }

        private void UpdateCurrentWorkPointerPosition()
        {
            Villager villager = Managers.Instance.VillagerSelection.SelectedVillager;

            foreach (UiSelectableElement element in elementsToSelect) {
                ProfessionLabelItem currItem = (ProfessionLabelItem) element;
                
                if (currItem.ProfessionData.ProfessionType != villager.Profession.Type) continue;
                RectTransform pointerRect = currentProfessionPointer.GetComponent<RectTransform>();
                currentProfessionPointer.transform.SetParent(element.transform);
                
                float newY = element.GetComponent<RectTransform>().sizeDelta.y / 2;
                pointerRect.anchoredPosition = new Vector2(newY, -newY);
                break;
            }
        }

        public void OnPanelOpen()
        {
            InputManager.VillagerPropertiesInputState.SetToVillagerProfessionDisplayChildState(Managers.Instance.GUI.VillagerProfessionChangingPanel);
            Villager villager = Managers.Instance.VillagerSelection.SelectedVillager;
            
            //Initialize profession pointer pos
            UpdateCurrentWorkPointerPosition();
            
            //Initialize first selected item
            currentProfession = (ProfessionLabelItem) elementsToSelect[0];
            
            //Initialize profession properties panel
            propertiesLabel.AttachPanelToProfession(currentProfession.transform);
            professionsGroup.UpdateElementsPosition();
            
            //Initialize pointer
            pointer.SetPointerOnUiElementWithParent(currentProfession.transform);
            
            // Initialize workplaces
            rightArrow.SetActive(true);
            foreach (UiSelectableElement selectableElement in elementsToSelect) {
                ProfessionLabelItem labelItem = (ProfessionLabelItem) selectableElement;
                labelItem.LoadWorkplaces();
            }
            
            //Initialize profession label data
            if (currentProfession.Workplaces.Length <= 0) 
                propertiesLabel.ShowNotAvailableWorkplacesPanel(true);
            else {
                propertiesLabel.LoadProfessionData(currentProfession.ProfessionData, villager); 
                Managers.Instance.Cameras.FocusCameraOn(currentProfession.Workplaces[0].transform);
            }
        }

        public void OnPanelClose()
        {
            foreach (UiSelectableElement selectableElement in elementsToSelect) {
                ProfessionLabelItem labelItem = (ProfessionLabelItem) selectableElement;
                labelItem.ResetLabel(normalLabelHeight);
                labelItem.ClearWorkplaces();
            }
            
            gameObject.SetActive(false);
        }

        public void SetPointerOnProfession(int value)
        {
            GetNextElement(value);
            currentProfession.ResetLabel(normalLabelHeight);
            currentProfession = (ProfessionLabelItem) currentElement;

            if (currentProfession.Workplaces.Length > 0) {
                propertiesLabel.ShowNotAvailableWorkplacesPanel(false);
                propertiesLabel.LoadProfessionData(currentProfession.ProfessionData, Managers.Instance.VillagerSelection.SelectedVillager);
            }
            else {
                propertiesLabel.ShowNotAvailableWorkplacesPanel(true);
            }
            
            propertiesLabel.AttachPanelToProfession(currentProfession.transform);
            professionsGroup.UpdateElementsPosition();
            MovePointerWithParent();
        }

        public void ShowWorkplace(int value)
        {
            //TODO: switch arrows on available workplaces, but someday, maybe
            if (currentProfession.Workplaces.Length <= 0) return;
            workplacesIdx += value;
            
            if (workplacesIdx < 0) workplacesIdx = 0;
            else if (workplacesIdx >= currentProfession.Workplaces.Length) workplacesIdx = currentProfession.Workplaces.Length - 1;
            
            leftArrow.SetActive(workplacesIdx > 0);
            rightArrow.SetActive(workplacesIdx < currentProfession.Workplaces.Length -1);
            
            Managers.Instance.Cameras.FocusCameraOn(currentProfession.Workplaces[workplacesIdx].transform);
        }

        public void ShowAcceptancePanel()
        {
            acceptancePanel.gameObject.SetActive(true);
            InputManager.VillagerPropertiesInputState.SetToNewProfessionAcceptChildState(acceptancePanel);
        }
        
        public void TakeProfession()
        {
            Villager selectedVillager = Managers.Instance.VillagerSelection.SelectedVillager;

            if (selectedVillager.Profession.Type == currentProfession.ProfessionData.ProfessionType) {
                if (selectedVillager.Profession.Workplace != currentProfession.Workplaces[workplacesIdx]) 
                    selectedVillager.Profession.UpdateWorkplaceForProfession(currentProfession.Workplaces[workplacesIdx]);
            } else 
                Managers.Instance.Professions.SetVillagerProfession(selectedVillager, currentProfession.ProfessionData.ProfessionType, currentProfession.Workplaces[workplacesIdx]);
            
            CloseAcceptablePanel();
            UpdateCurrentWorkPointerPosition();
        }

        public void CloseAcceptablePanel()
        {
            acceptancePanel.gameObject.SetActive(false);
            InputManager.VillagerPropertiesInputState.SetToVillagerProfessionDisplayChildState(Managers.Instance.GUI.VillagerProfessionChangingPanel);
        }
    }
}
