using UnityEngine;
using Zenject;

namespace CardRPG.UI.Features.Gameplay
{
    public class DependencyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<string>().FromInstance("Hello World!");
            Container.Bind<Greeter>().AsSingle().NonLazy();

            Debug.Log("Install Bindings");
        }

        public class Greeter
        {
            public Greeter(string message)
            {
                Debug.Log(message);
            }
        }
    }
}
