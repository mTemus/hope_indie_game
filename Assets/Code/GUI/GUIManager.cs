using System.Collections.Generic;
using Code.GUI.BuildingSelecting;
using Code.GUI.PlayerToolsMenu;
using Code.GUI.Villagers;
using Code.Resources;
using UnityEngine;

namespace Code.GUI
{
    public class GUIManager : MonoBehaviour
    {
        [Header("Player GUI")]
        [SerializeField] private RadialToolsMenu playerToolsMenu;
        [SerializeField] private BuildingSelectingMenu buildingSelectingMenu;

        [Header("Villager GUI")] 
        [SerializeField] private VillagerPropertiesPanel villagerPropertiesPanel;
        
        [Header("Other")]
        [SerializeField] private List<Sprite> resourceIcons;

        public Sprite GetResourceIcon(ResourceType resourceType)
        {
            //TODO: this should be changed to addressables!
            return resourceIcons.Find(sprite => sprite.name == resourceType.ToString().ToLower());
        }


        public VillagerPropertiesPanel VillagerPropertiesPanel => villagerPropertiesPanel;

        public BuildingSelectingMenu BuildingSelectingMenu => buildingSelectingMenu;

        public RadialToolsMenu PlayerToolsMenu => playerToolsMenu;
    }
}
