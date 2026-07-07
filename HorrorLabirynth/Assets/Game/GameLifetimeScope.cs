using Game.Gems;
using Game.UI.Dialogs;
using Game.UI.Dialogs.Defeat;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private GemsBalance _gemsBalance;
        [SerializeField] private GemResources _gemResources;
        [SerializeField] private DialogsResources _dialogsResources;
        [SerializeField] private Transform _gemsRoot;
        [SerializeField] private Transform _dialogsRoot;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_gemsBalance);
            builder.RegisterInstance(_gemResources);
            builder.RegisterInstance(_dialogsResources);
            builder.Register<GemsSystem>(Lifetime.Singleton);
            builder.Register<GemsCollectionProgress>(Lifetime.Singleton);
            builder.Register<GemPlaceholdersSystem>(Lifetime.Singleton);

            builder.RegisterInstance(_gemsRoot).Keyed(TransformContainerId.GemsRoot);
            builder.RegisterInstance(_dialogsRoot).Keyed(TransformContainerId.DialogsRoot);

            builder.RegisterFactory<DefeatDialogView, DefeatDialogPresenter>(
                _ => view => new DefeatDialogPresenter(view),
                Lifetime.Transient);
            builder.Register<GemsSpawner>(Lifetime.Singleton);
            builder.Register<DialogsService>(Lifetime.Singleton);
            builder.RegisterEntryPoint<SystemsInitializer>();

            builder.RegisterBuildCallback(InjectGemPlaceholders);
        }

        private void InjectGemPlaceholders(IObjectResolver resolver)
        {
            GemPlaceholder[] placeholders = Object.FindObjectsByType<GemPlaceholder>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

            for (int i = 0; i < placeholders.Length; i++)
                resolver.Inject(placeholders[i]);
        }
    }
}
