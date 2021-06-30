using System;
using System.Linq;
using Code.GUI.UIElements;
using Code.Map.Resources;
using Code.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.GUI.BuildingSelecting
{
    public class BuildingSelectingMenu : MonoBehaviour
    {
        [Header("Scroll Areas")]
        [SerializeField] private UiScrollArea buildingTypesArea;
        [SerializeField] private UiScrollArea buildingObjectsArea;

        [Header("Building Types")] 
        [SerializeField] private Transform buildingTypesContent;

        [Header("Building Objects")] 
        [SerializeField] private UiBuildingElement[] resourcesBuildingObjects;
        [SerializeField] private UiBuildingElement[] villageBuildingObjects;
        [SerializeField] private UiBuildingElement[] industryBuildingObjects;


        [Header("Building Properties")] 
        [SerializeField] private Image miniature;
        [SerializeField] private TextMeshProUGUI buildingName;
        [SerializeField] private TextMeshProUGUI buildingDescription;
        [SerializeField] private UiRequiredResource[] requiredResources;
        
        private int buildingTypesIdx;
        private int buildingObjectsIdx;

        private UiBuildingElement currentBuilding;

        private UiBuildingElement[] buildingObjectsArray;

        private void Awake()
        {
            buildingObjectsArray = resourcesBuildingObjects;
            gameObject.SetActive(false);
        }

        private void UpdateBuildingProperties()
        {
            ResetResources();
            
            miniature.sprite = currentBuilding.Sprite;
            buildingName.text = currentBuilding.Data.BuildingName;
            buildingDescription.text = currentBuilding.Description;

            foreach (Resource resourceData in currentBuilding.Data.RequiredResources) {
                UiRequiredResource uiResource = requiredResources.FirstOrDefault(r => r.Type == resourceData.Type);

                if (uiResource != null) {
                    uiResource.gameObject.SetActive(true);
                    uiResource.SetAmount(resourceData.amount);
                }
                else {
                    throw new Exception("No ui resource for type: " + resourceData.Type);
                }
            }
        }

        private void UpdateBuildingObjectsArray()
        {
            switch (buildingTypesIdx) {
                case 0:
                    buildingObjectsArray = resourcesBuildingObjects;
                    break;
                
                case 1:
                    buildingObjectsArray = villageBuildingObjects;
                    break;
                
                case 2:
                    buildingObjectsArray = industryBuildingObjects;
                    break;
            }
        }

        private void ResetResources()
        {
            foreach (UiRequiredResource resource in requiredResources) 
                resource.gameObject.SetActive(false);
        }
        
        public void OnMenuOpen()
        {
            currentBuilding = buildingObjectsArray[buildingObjectsIdx];
            Systems.I.Building.SetBuilding(currentBuilding.Data);
            UpdateBuildingProperties();
        }

        public void OnMenuClose()
        {
            buildingObjectsIdx = 0;
            buildingTypesIdx = 0;
            
            ResetResources();
            UpdateBuildingObjectsArray();
        }

        public void ChangeBuildingType(int value)
        {
            buildingTypesIdx += value;
            buildingTypesIdx = Mathf.Clamp(buildingTypesIdx, 0, buildingTypesContent.childCount - 1);
            
            buildingTypesArea.ChangeValue(value);
            buildingObjectsArea.ChangeContent(buildingTypesIdx);
            UpdateBuildingObjectsArray();
            
            buildingObjectsIdx = 0;
            currentBuilding = buildingObjectsArray[buildingObjectsIdx];
            Systems.I.Building.SetBuilding(currentBuilding.Data);
            UpdateBuildingProperties();
        }

        public void ChangeBuilding(int value)
        {
            buildingObjectsIdx += value;
            buildingObjectsIdx = Mathf.Clamp(buildingObjectsIdx, 0, buildingObjectsArray.Length - 1);

            buildingObjectsArea.ChangeValue(-value);
            currentBuilding = buildingObjectsArray[buildingObjectsIdx];
            Systems.I.Building.SetBuilding(currentBuilding.Data);
            UpdateBuildingProperties();
        }
    }
}
