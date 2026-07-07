using Game.Gems;
using Game.Player;
using TMPro;
using UnityEngine;
using VContainer;

namespace Game.Hud
{
    public class GemsCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _counterText;
        [SerializeField] private string _format = "{0}/{1}";

        private PlayerResources _playerResources;
        private GemsBalance _gemsBalance;

        [Inject]
        public void Construct(PlayerResources playerResources, GemsBalance gemsBalance)
        {
            _playerResources = playerResources;
            _gemsBalance = gemsBalance;
            _playerResources.OnGemCollected += OnGemCollected;
        }
        private void Start()
        {
            RefreshCounterText();
        }

        private void OnDestroy()
        {
            if (_playerResources != null)
                _playerResources.OnGemCollected -= OnGemCollected;
        }

        private void OnGemCollected()
        {
            RefreshCounterText();
        }

        private void RefreshCounterText()
        {
            _counterText.text = string.Format(
                _format,
                _playerResources.CollectedGemsCount,
                _gemsBalance.GemsRequiredToExit);
        }
    }
}
