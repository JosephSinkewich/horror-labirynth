using Cysharp.Threading.Tasks;
using Game.Player;
using Game.UI.Dialogs;

namespace Game
{
    public class DefeatScenario
    {
        private readonly DialogsService _dialogsService;
        private readonly PlayerController _playerController;
        private bool _isRunning;

        public DefeatScenario(DialogsService dialogsService, PlayerController playerController)
        {
            _dialogsService = dialogsService;
            _playerController = playerController;
        }

        public async UniTask RunAsync()
        {
            if (_isRunning || _dialogsService.IsDialogShowing)
                return;

            _isRunning = true;
            _playerController.SetControlEnabled(false);
            await _dialogsService.ShowDefeatDialogAsync();
        }
    }
}
