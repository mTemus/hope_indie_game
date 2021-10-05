using HopeMain.Code.GUI.UIElements;
using HopeMain.Code.GUI.UIElements.SelectableElement;
using HopeMain.Code.System;
using HopeMain.Code.System.GameInput;
using HopeMain.Code.World.Buildings.Workplace;
using UnityEngine;
using UnityEngine.UI;

namespace HopeMain.Code.GUI.Villager.Selecting
{
    public class ProfessionChangingPanel : UiSelectablePanel
    {
        [Header("Properties")] 
        [SerializeField] private float normalLabelHeight;
        
        [Header("Components")]
        [SerializeField] private UiAcceptancePanel acceptancePanel;
        [SerializeField] private UIVerticalGroup professionsGroup;
        
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
            Characters.Villagers.Entity.Villager villager = Managers.I.Selection.SelectedVillager;

            foreach (UiSelectableElement element in elementsToSelect) {
                ProfessionLabelItem currItem = (ProfessionLabelItem) element;
                
                if (currItem.Data.Type != villager.Profession.Data.Type) continue;
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
            InputManager.VillagerProperties.SetToVillagerProfessionDisplayChildState(Managers.I.GUI.ProfessionChangingPanel);
            Characters.Villagers.Entity.Villager villager = Managers.I.Selection.SelectedVillager;
            
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
                propertiesLabel.LoadProfessionData(currentProfession.Data, villager); 
                Managers.I.Cameras.FocusCameraOn(currentProfession.Workplaces[0].transform);
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
                propertiesLabel.LoadProfessionData(currentProfession.Data, Managers.I.Selection.SelectedVillager);
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
            
            Managers.I.Cameras.FocusCameraOn(CurrentWorkplace.transform);
        }

        public void ShowAcceptancePanel()
        {
            acceptancePanel.gameObject.SetActive(true);
            InputManager.VillagerProperties.SetToNewProfessionAcceptChildState(acceptancePanel);
        }
        
        public void TakeProfession()
        {
            Characters.Villagers.Entity.Villager selectedVillager = Managers.I.Selection.SelectedVillager;

            if (selectedVillager.Profession.Data.Type == currentProfession.Data.Type) {
                if (selectedVillager.Profession.Workplace == CurrentWorkplace) return;
                selectedVillager.Profession.Workplace.FireWorker(selectedVillager);
                CurrentWorkplace.HireWorker(selectedVillager);
                
            } else {
                Managers.I.Professions.FireVillagerFromOldProfession(selectedVillager);
                Managers.I.Professions.SetVillagerProfession(selectedVillager,
                    currentProfession.Data, CurrentWorkplace);
            }
        }

        public bool AreThereAnyWorkplaces() =>
            currentProfession.Workplaces.Length != 0;

        public WorkplaceBase CurrentWorkplace =>
            currentProfession.Workplaces[workplacesIdx];

        public void CloseAcceptablePanel()
        {
            acceptancePanel.gameObject.SetActive(false);
            InputManager.VillagerProperties.SetToVillagerProfessionDisplayChildState(Managers.I.GUI.ProfessionChangingPanel);
        }
    }
}
