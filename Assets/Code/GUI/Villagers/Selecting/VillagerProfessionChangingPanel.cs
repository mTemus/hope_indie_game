using Code.GUI.UIElements.SelectableElement;
using Code.System;
using Code.System.PlayerInput;
using Code.Utilities;
using Code.Villagers.Entity;
using UnityEngine;
using UnityEngine.UI;

namespace Code.GUI.Villagers.Selecting
{
    public class VillagerProfessionChangingPanel : UiSelectablePanel
    {
        [Header("Properties")] 
        [SerializeField] private int normalLabelHeight;
        [SerializeField] private UiAcceptancePanel acceptancePanel;
        
        [Header("Additional pointers")] 
        [SerializeField] private Image currentProfessionPointer;
        [SerializeField] private ProfessionPropertiesLabel propertiesLabel;

        private int workplacesIdx;
        private ProfessionLabelItem currentProfession;

        private void Awake()
        {
            currentElement = elementsToSelect[0];
            gameObject.SetActive(false);
        }

        public void OnPanelOpen()
        {
            InputManager.VillagerPropertiesInputState.SetToVillagerProfessionDisplayChildState(Managers.Instance.GUI.VillagerProfessionChangingPanel);
            Villager villager = Managers.Instance.VillagerSelection.SelectedVillager;
            
            foreach (UiSelectableElement element in elementsToSelect) {
                ProfessionLabelItem currItem = element.GetComponent<ProfessionLabelItem>();
                
                if (currItem.ProfessionData.ProfessionType != villager.Profession.Type) continue;
                Transform pointerTransform = currentProfessionPointer.transform;
                Vector3 pointerPos = pointerTransform.localPosition;
                
                pointerTransform.SetParent(element.transform);
                Vector3 newPointerPos = new Vector3(pointerPos.x, 0, pointerPos.z);
                pointerTransform.localPosition = newPointerPos;
                break;
            }
            
            currentProfession = (ProfessionLabelItem) elementsToSelect[0];
            pointer.SetPointerOnUiElementWithParent(currentProfession.transform);
            
            //
            // if (currentProfession.Workplaces.Length <= 0) 
            //     propertiesLabel.ShowAvailableWorkplacesPanel(true);
            // else {
            //     propertiesLabel.AttachPanelToProfession(currentProfession.transform); 
            //     propertiesLabel.LoadProfessionData(currentProfession.ProfessionData, villager);
            // }
            
            
            foreach (UiSelectableElement selectableElement in elementsToSelect) {
                ProfessionLabelItem labelItem = (ProfessionLabelItem) selectableElement;
                labelItem.LoadWorkplaces();
            }

            currentProfession = (ProfessionLabelItem) elementsToSelect[0];
            pointer.SetPointerOnUiElement(currentProfession.transform);

            if (currentProfession.Workplaces.Length <= 0) 
                propertiesLabel.ShowAvailableWorkplacesPanel(true);
            else {
                propertiesLabel.AttachPanelToProfession(currentProfession.transform); 
                propertiesLabel.LoadProfessionData(currentProfession.ProfessionData, villager);
            }
        }

        public void OnPanelClose()
        {
            foreach (UiSelectableElement selectableElement in elementsToSelect) {
                ProfessionLabelItem labelItem = (ProfessionLabelItem) selectableElement;
                labelItem.ClearWorkplaces();
            }
            
            InputManager.VillagerPropertiesInputState.SetToVillagerPropertiesDisplayChildState();
        }

        public void SetPointerOnProfession(int value)
        {
            MovePointerWithParent(value);
            currentProfession.ResetSize(normalLabelHeight);
            currentProfession = (ProfessionLabelItem) currentElement;
            propertiesLabel.AttachPanelToProfession(currentProfession.transform);
        }

        public void ShowWorkplace(int value)
        {
            //TODO: switch arrows on available workplaces, but someday, maybe
            
            workplacesIdx = GlobalUtilities.IncrementIdx(workplacesIdx, value, currentProfession.Workplaces.Length);
            Managers.Instance.Cameras.FocusCameraOn(currentProfession.Workplaces[workplacesIdx].transform);
        }

        public void ShowAcceptancePanel()
        {
            if (currentProfession.ProfessionData.ProfessionType == Managers.Instance.VillagerSelection.SelectedVillager.Profession.Type) return;
            
            acceptancePanel.gameObject.SetActive(true);
            InputManager.VillagerPropertiesInputState.SetToNewProfessionAcceptChildState(acceptancePanel);
        }
        
        public void TakeProfession()
        {
            Villager selectedVillager = Managers.Instance.VillagerSelection.SelectedVillager;
            Managers.Instance.Professions.SetVillagerProfession(selectedVillager, currentProfession.ProfessionData.ProfessionType, currentProfession.Workplaces[workplacesIdx]);
        }

        public void CancelTakingProfession()
        {
            acceptancePanel.gameObject.SetActive(false);
            InputManager.VillagerPropertiesInputState.SetToVillagerProfessionDisplayChildState(Managers.Instance.GUI.VillagerProfessionChangingPanel);
        }
    }
}
