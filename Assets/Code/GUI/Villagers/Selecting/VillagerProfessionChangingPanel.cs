using Code.GUI.UIElements;
using Code.GUI.UIElements.SelectableElement;
using Code.Map.Building;
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
        [SerializeField] private float normalLabelHeight;
        
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

        public void UpdateCurrentWorkPointerPosition()
        {
            Villager villager = Managers.Instance.VillagerSelection.SelectedVillager;

            foreach (UiSelectableElement element in elementsToSelect) {
                ProfessionLabelItem currItem = (ProfessionLabelItem) element;
                
                if (currItem.ProfessionData.ProfessionType != villager.Profession.Type) continue;
                RectTransform pointerRect = currentProfessionPointer.GetComponent<RectTransform>();
                currentProfessionPointer.transform.SetParent(element.transform);
                
                float newY = normalLabelHeight / 2;
                pointerRect.anchoredPosition = new Vector2(newY, -newY);
                break;
            }
        }

        public void ReloadProfessionWorkplaces()
        {
            foreach (UiSelectableElement selectableElement in elementsToSelect) 
                selectableElement.GetComponent<ProfessionLabelItem>().LoadWorkplaces();
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
            ReloadProfessionWorkplaces();
            
            //Initialize profession label data
            if (AreThereAnyWorkplaces()) {
                propertiesLabel.LoadProfessionData(currentProfession.ProfessionData, villager); 
                Managers.Instance.Cameras.FocusCameraOn(currentProfession.Workplaces[0].transform);
            }
            else {
                propertiesLabel.ShowNotAvailableWorkplacesPanel(true);
            }
        }

        public void OnPanelClose()
        {
            foreach (UiSelectableElement selectableElement in elementsToSelect) {
                ProfessionLabelItem labelItem = (ProfessionLabelItem) selectableElement;
                labelItem.ResetLabel(normalLabelHeight);
                labelItem.ClearWorkplaces();
            }

            selectionIdx = 0;
            gameObject.SetActive(false);
        }

        public void SetPointerOnProfession(int value)
        {
            GetNextElement(value);
            currentProfession.ResetLabel(normalLabelHeight);
            currentProfession = (ProfessionLabelItem) currentElement;

            if (AreThereAnyWorkplaces()) {
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
            //TODO: arrows are buggy
            if (!AreThereAnyWorkplaces()) return;
            workplacesIdx += value;
            
            if (workplacesIdx < 0) workplacesIdx = 0;
            else if (workplacesIdx >= currentProfession.Workplaces.Length) workplacesIdx = currentProfession.Workplaces.Length - 1;
            
            leftArrow.SetActive(workplacesIdx > 0);
            rightArrow.SetActive(workplacesIdx < currentProfession.Workplaces.Length -1);
            
            Managers.Instance.Cameras.FocusCameraOn(CurrentWorkplace.transform);
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
                if (selectedVillager.Profession.Workplace != CurrentWorkplace) 
                    selectedVillager.Profession.UpdateWorkplaceForProfession(CurrentWorkplace);
            } else 
                Managers.Instance.Professions.SetVillagerProfession(selectedVillager, currentProfession.ProfessionData.ProfessionType, CurrentWorkplace);
        }

        public bool AreThereAnyWorkplaces() =>
            currentProfession.Workplaces.Length != 0;

        public Workplace CurrentWorkplace =>
            currentProfession.Workplaces[workplacesIdx];

        public void CloseAcceptablePanel()
        {
            acceptancePanel.gameObject.SetActive(false);
            InputManager.VillagerPropertiesInputState.SetToVillagerProfessionDisplayChildState(Managers.Instance.GUI.VillagerProfessionChangingPanel);
        }
    }
}
