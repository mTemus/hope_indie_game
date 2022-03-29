using _Prototype.Code.v002.Player;
using _Prototype.Code.v002.Player.Tools;
using UnityEngine;
using Zenject;

namespace _Prototype.Code.v002.System.Installers
{
    public class PlayerComponentsInstaller : MonoInstaller<PlayerComponentsInstaller>
    {
        [SerializeField] private PlayerCharacter playerCharacter;
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private PlayerAnimations animations;
        [SerializeField] private PlayerTools tools;
        
        public override void InstallBindings()
        {
            Container.BindInstance(playerCharacter);
            Container.BindInstance(movement);
            Container.BindInstance(animations);
            Container.BindInstance(tools);
        }
    }
}