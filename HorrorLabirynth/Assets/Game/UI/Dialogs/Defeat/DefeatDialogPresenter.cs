using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Game.UI.Dialogs.Defeat
{
    public class DefeatDialogPresenter
    {
        private readonly DefeatDialogView _view;

        public DefeatDialogPresenter(DefeatDialogView view)
        {
            _view = view;
        }

        public async UniTask ShowAsync()
        {
            _view.gameObject.SetActive(true);
            await WaitForRestartClickAsync();
            RestartLevel();
        }

        private UniTask WaitForRestartClickAsync()
        {
            var completionSource = new UniTaskCompletionSource();

            void OnRestartClicked()
            {
                _view.RestartButton.onClick.RemoveListener(OnRestartClicked);
                completionSource.TrySetResult();
            }

            _view.RestartButton.onClick.AddListener(OnRestartClicked);
            return completionSource.Task;
        }

        private static void RestartLevel()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.buildIndex);
        }
    }
}
