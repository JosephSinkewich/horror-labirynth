using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Dialogs.Defeat
{
    public class DefeatDialogView : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;

        public Button RestartButton => _restartButton;
    }
}
