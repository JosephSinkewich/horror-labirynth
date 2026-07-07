using Game.Gems;
using VContainer.Unity;

namespace Game
{
    public class SystemsInitializer : IStartable
    {
        private readonly GemsSpawner _gemsSpawner;

        public SystemsInitializer(GemsSpawner gemsSpawner)
        {
            _gemsSpawner = gemsSpawner;
        }

        public void Start()
        {
            _gemsSpawner.Initialize();
        }
    }
}
