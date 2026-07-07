using Game.Gems;
using Game.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private GemsBalance _gemsBalance;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_gemsBalance);
            builder.Register<GemsSystem>(Lifetime.Singleton);
            builder.Register<PlayerResources>(Lifetime.Singleton);
        }
    }
}
