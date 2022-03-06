using _Prototype.Code.GUI.Player.BuildingSelecting;
using _Prototype.Code.GUI.Player.ToolsMenu;
using _Prototype.Code.GUI.Villager.Selecting;
using UnityEngine;

namespace _Prototype.Code.GUI
{
    /// <summary>
    /// 
    /// </summary>
    public class GUIManager : MonoBehaviour
    {
        [Header("Player GUI")]
        [SerializeField] private RadialToolsMenu playerToolsMenu;
        [SerializeField] private BuildingSelectingMenu buildingSelectingMenu;
        [SerializeField] private RequiredResourcesPanel requiredResourcesPanel;

        [Header("Villager GUI")] 
        [SerializeField] private PropertiesPanel villagerPropertiesPanel;
        [SerializeField] private ProfessionChangingPanel professionChangingPanel;

        public RequiredResourcesPanel RequiredResourcesPanel => requiredResourcesPanel;

        public ProfessionChangingPanel ProfessionChangingPanel => professionChangingPanel;

        public PropertiesPanel VillagerPropertiesPanel => villagerPropertiesPanel;

        public BuildingSelectingMenu BuildingSelectingMenu => buildingSelectingMenu;

        public RadialToolsMenu PlayerToolsMenu => playerToolsMenu;
    }
}
