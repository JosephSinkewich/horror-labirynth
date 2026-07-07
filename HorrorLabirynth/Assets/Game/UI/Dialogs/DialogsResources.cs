using Game.UI.Dialogs.Defeat;
using UnityEngine;

namespace Game.UI.Dialogs
{
    [CreateAssetMenu(fileName = "DialogsResources", menuName = "Game/Dialogs Resources")]
    public class DialogsResources : ScriptableObject
    {
        [SerializeField] private DefeatDialogView _defeatDialogPrefab;

        public DefeatDialogView DefeatDialogPrefab => _defeatDialogPrefab;
    }
}
