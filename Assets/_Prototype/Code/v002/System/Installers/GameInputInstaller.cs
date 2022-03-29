using Zenject;

namespace _Prototype.Code.v002.System.Installers
{
    public class GameInputInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<global::GameInput>().AsSingle().NonLazy();
        }
    }
}