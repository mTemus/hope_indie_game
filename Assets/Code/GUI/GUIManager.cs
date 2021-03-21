using Code.GUI.PlayerToolsMenu;
using UnityEngine;

namespace Code.GUI
{
    public class GUIManager : MonoBehaviour
    {
        [SerializeField] private RadialToolsMenu playerToolsMenu;
    
        public RadialToolsMenu PlayerToolsMenu => playerToolsMenu;
    }
}
