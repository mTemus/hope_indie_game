using System;
using Code.GUI.UIElements;
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
        [SerializeField] private UiBuildingElement[] industryBuildingObjects;
        [SerializeField] private UiBuildingElement[] villageBuildingObjects;

        [Header("Building Properties")] 
        [SerializeField] private Image miniature;
        [SerializeField] private TextMeshProUGUI buildingName;
        [SerializeField] private TextMeshProUGUI buildingDescription;
        [SerializeField] private Transform buildingResources;
        
        private int buildingTypesIdx;
        private int buildingObjectsIdx;

        private UiBuildingElement currentBuilding;

        private UiBuildingElement[] buildingObjectsArray;

        private void Awake()
        {
            buildingObjectsArray = industryBuildingObjects;
            gameObject.SetActive(false);
        }

        private void UpdateBuildingProperties()
        {
            miniature.sprite = currentBuilding.Sprite;
            buildingName.text = currentBuilding.Data.buildingName;
            buildingDescription.text = currentBuilding.Description;
            
            //Building resources
        }

        private void UpdateBuildingObjectsArray()
        {
            switch (buildingTypesIdx) {
                case 0:
                    buildingObjectsArray = industryBuildingObjects;
                    break;
                
                case 1:
                    buildingObjectsArray = villageBuildingObjects;
                    break;
                
                case 2:
                    break;
            }
        }
        
        public void OnMenuOpen()
        {
            currentBuilding = buildingObjectsArray[buildingObjectsIdx];
            Systems.Instance.Building.SetBuilding(currentBuilding.Data);
            UpdateBuildingProperties();
        }

        public void OnMenuClose()
        {
            buildingObjectsIdx = 0;
            buildingTypesIdx = 0;
            
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
            Systems.Instance.Building.SetBuilding(currentBuilding.Data);
            UpdateBuildingProperties();
        }

        public void ChangeBuilding(int value)
        {
            buildingObjectsIdx += value;
            buildingObjectsIdx = Mathf.Clamp(buildingObjectsIdx, 0, buildingObjectsArray.Length - 1);

            buildingObjectsArea.ChangeValue(-value);
            currentBuilding = buildingObjectsArray[buildingObjectsIdx];
            Systems.Instance.Building.SetBuilding(currentBuilding.Data);
            UpdateBuildingProperties();
        }
    }
}
