using _Prototype.Code.v002.Player;
using _Prototype.Code.v002.Player.Tools;
using UnityEngine;
using Zenject;

namespace _Prototype.Code.v002.System.Installers
{
    public class PlayerComponentsInstaller : MonoInstaller<PlayerComponentsInstaller>
    {
        [SerializeField] private PlayerCharacter _playerCharacter;
        [SerializeField] private PlayerTools _playerTools;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_playerCharacter);
            Container.BindInstance(_playerTools);
        }
    }
}