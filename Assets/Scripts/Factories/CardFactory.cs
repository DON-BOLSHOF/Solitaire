using System.Collections.Generic;
using System.Linq;
using Cards;
using Definitions;
using UnityEngine;

namespace Factories
{
    public class CardFactory
    {
        private List<Sprite> _cardBacks;

        public CardFactory(int cardCount)
        {
            _cardBacks = DefsFacade.I.CardBackDefs.GetAllCardsDefs().Take(cardCount).ToList();
        }

        public Card Create(Transform parent, FactoryLimitedType limitedType = FactoryLimitedType.UnLimit)
        {
            if (limitedType == FactoryLimitedType.Limit && _cardBacks.Count <= 0) return null;

            var card = Object.Instantiate(Resources.Load<Card>("PrefabsToInstantiate/Card"), parent);

            var cardBack = _cardBacks[Random.Range(0, _cardBacks.Count)];
            if (limitedType == FactoryLimitedType.Limit)
                _cardBacks.Remove(cardBack);

            card.DynamicInstantiate(cardBack);

            return card;
        }

        public bool TryCreateLimitCard(out Card card, Transform parent)
        {
            card = Create(parent, FactoryLimitedType.Limit);
            return card != null;
        }
    }

    public enum FactoryLimitedType
    {
        Limit,
        UnLimit
    }
}