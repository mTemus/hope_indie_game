using HopeMain.Code.GUI.Player.BuildingSelecting;
using HopeMain.Code.GUI.Player.ToolsMenu;
using HopeMain.Code.GUI.Villager.Selecting;
using UnityEngine;

namespace HopeMain.Code.GUI
{
    public class GUIManager : MonoBehaviour
    {
        [Header("Player GUI")]
        [SerializeField] private RadialToolsMenu playerToolsMenu;
        [SerializeField] private BuildingSelectingMenu buildingSelectingMenu;
        [SerializeField] private RequiredResourcesPanel requiredResourcesPanel;

        [Header("Villager GUI")] 
        [SerializeField] private VillagerPropertiesPanel villagerPropertiesPanel;
        [SerializeField] private VillagerProfessionChangingPanel villagerProfessionChangingPanel;

        public RequiredResourcesPanel RequiredResourcesPanel => requiredResourcesPanel;

        public VillagerProfessionChangingPanel VillagerProfessionChangingPanel => villagerProfessionChangingPanel;

        public VillagerPropertiesPanel VillagerPropertiesPanel => villagerPropertiesPanel;

        public BuildingSelectingMenu BuildingSelectingMenu => buildingSelectingMenu;

        public RadialToolsMenu PlayerToolsMenu => playerToolsMenu;
    }
}
