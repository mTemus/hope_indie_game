using _Prototype.Code.v002.GUI.PlayerTools;
using UnityEngine;
using Zenject;

namespace _Prototype.Code.v002.System.Installers
{
    public class PlayerGUIObjectsInstaller : MonoInstaller
    {
        [Header("Player Tools")]
        [SerializeField] private RadialToolsMenu toolsMenu;
        
        public override void InstallBindings()
        {
            Container.BindInstance(toolsMenu);
        }
    }
}