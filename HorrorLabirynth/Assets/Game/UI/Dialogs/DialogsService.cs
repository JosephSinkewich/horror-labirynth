using System;
using Cysharp.Threading.Tasks;
using Game;
using Game.UI.Dialogs.Defeat;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.UI.Dialogs
{
    public class DialogsService
    {
        private readonly IObjectResolver _objectResolver;
        private readonly Func<DefeatDialogView, DefeatDialogPresenter> _createDefeatDialogPresenter;
        private readonly Transform _dialogsRoot;
        private readonly DefeatDialogView _defeatDialogPrefab;
        private int _activeDialogsCount;

        public DialogsService(
            IObjectResolver objectResolver,
            Func<DefeatDialogView, DefeatDialogPresenter> createDefeatDialogPresenter,
            [Key(TransformContainerId.DialogsRoot)] Transform dialogsRoot,
            DialogsResources dialogsResources)
        {
            _objectResolver = objectResolver;
            _createDefeatDialogPresenter = createDefeatDialogPresenter;
            _dialogsRoot = dialogsRoot;
            _defeatDialogPrefab = dialogsResources.DefeatDialogPrefab;
        }

        public bool IsDialogShowing => _activeDialogsCount > 0;

        public async UniTask ShowDefeatDialogAsync()
        {
            DefeatDialogView view = _objectResolver.Instantiate(_defeatDialogPrefab, _dialogsRoot);
            DefeatDialogPresenter presenter = _createDefeatDialogPresenter(view);

            _activeDialogsCount++;
            try
            {
                await presenter.ShowAsync();
            }
            finally
            {
                _activeDialogsCount--;
                if (view != null)
                    UnityEngine.Object.Destroy(view.gameObject);
            }
        }
    }
}
