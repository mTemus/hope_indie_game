using RadialMenu.Scripts.UI.Manager;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private RadialToolsMenu playerToolsMenu;
    
    public RadialToolsMenu PlayerToolsMenu => playerToolsMenu;
}
