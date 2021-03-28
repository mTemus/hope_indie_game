using System.Collections.Generic;
using Code.GUI.BuildingSelecting;
using Code.GUI.PlayerToolsMenu;
using Code.GUI.Villagers.Selecting;
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
        [SerializeField] private VillagerProfessionChangingPanel villagerProfessionChangingPanel;
        
        [Header("Other")]
        [SerializeField] private List<Sprite> resourceIcons;

        public Sprite GetResourceIcon(ResourceType resourceType)
        {
            //TODO: this should be changed to addressables!
            return resourceIcons.Find(sprite => sprite.name == resourceType.ToString().ToLower());
        }
        
        public VillagerProfessionChangingPanel VillagerProfessionChangingPanel => villagerProfessionChangingPanel;

        public VillagerPropertiesPanel VillagerPropertiesPanel => villagerPropertiesPanel;

        public BuildingSelectingMenu BuildingSelectingMenu => buildingSelectingMenu;

        public RadialToolsMenu PlayerToolsMenu => playerToolsMenu;
    }
}
