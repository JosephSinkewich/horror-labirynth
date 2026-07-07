using System.Collections.Generic;
using UnityEngine;

namespace Game.Gems
{
    public class GemPlaceholdersSystem
    {
        private readonly List<GemPlaceholder> _placeholders = new();

        public IReadOnlyList<GemPlaceholder> Placeholders => _placeholders;

        public void Register(GemPlaceholder placeholder)
        {
            if (placeholder == null || _placeholders.Contains(placeholder))
                return;

            _placeholders.Add(placeholder);
        }

        public void Unregister(GemPlaceholder placeholder)
        {
            _placeholders.Remove(placeholder);
        }
    }
}
