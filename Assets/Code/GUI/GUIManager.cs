using System.Collections.Generic;
using Code.GUI.BuildingSelecting;
using Code.GUI.PlayerToolsMenu;
using Code.Resources;
using UnityEngine;

namespace Code.GUI
{
    public class GUIManager : MonoBehaviour
    {
        [SerializeField] private RadialToolsMenu playerToolsMenu;
        [SerializeField] private BuildingSelectingMenu buildingSelectingMenu;

        [SerializeField] private List<Sprite> resourceIcons;
        public Sprite GetResourceIcon(ResourceType resourceType)
        {
            //TODO: this should be changed to addressables!
            return resourceIcons.Find(sprite => sprite.name == resourceType.ToString().ToLower());
        }
        
        public BuildingSelectingMenu BuildingSelectingMenu => buildingSelectingMenu;

        public RadialToolsMenu PlayerToolsMenu => playerToolsMenu;
    }
}
