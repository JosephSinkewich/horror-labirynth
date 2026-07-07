using Game.Gems;
using TMPro;
using UnityEngine;
using VContainer;

namespace Game.Hud
{
    public class GemsCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _counterText;
        [SerializeField] private string _format = "{0}/{1}";

        private GemsCollectionProgress _gemsCollectionProgress;

        [Inject]
        public void Construct(GemsCollectionProgress gemsCollectionProgress)
        {
            _gemsCollectionProgress = gemsCollectionProgress;
            _gemsCollectionProgress.OnGemCollected += OnGemCollected;
        }
        private void Start()
        {
            RefreshCounterText();
        }

        private void OnDestroy()
        {
            if (_gemsCollectionProgress != null)
                _gemsCollectionProgress.OnGemCollected -= OnGemCollected;
        }

        private void OnGemCollected()
        {
            RefreshCounterText();
        }

        private void RefreshCounterText()
        {
            _counterText.text = string.Format(
                _format,
                _gemsCollectionProgress.CollectedGemsCount,
                _gemsCollectionProgress.GemsRequiredToExit);
        }
    }
}
