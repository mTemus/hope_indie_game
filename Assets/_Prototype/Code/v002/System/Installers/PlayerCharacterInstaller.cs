using _Prototype.Code.v002.Player;
using UnityEngine;
using Zenject;

namespace _Prototype.Code.v002.System.Installers
{
    public class PlayerCharacterInstaller : MonoInstaller<PlayerCharacterInstaller>
    {
        [SerializeField] private PlayerCharacter _playerCharacter;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_playerCharacter);
        }
    }
}