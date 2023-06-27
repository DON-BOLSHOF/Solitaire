using System.Collections.Generic;
using System.Linq;
using Cards;
using DG.Tweening;
using UnityEngine;

namespace Handlers
{
    public class CardHandler: MonoBehaviour
    {
        [SerializeField] private CardDeck _cardDeck;
        [SerializeField] private Transform[] _destinationPoints;

        private List<MovableCard> _cards = new();
        private bool _isForwardAnimation = true;

        public void HandleCard()
        {
            if(TakeCardFromDeck()) MoveCard();
            else ReverseCards();
        }

        private void MoveCard()
        {
            _isForwardAnimation = true;

            var moveCard = _cards.Where(card => !card.Sequence.IsComplete()).ToList();
            foreach (var movableCard in moveCard)
            {
                movableCard.Sequence.PlayForward();
            }
        }

        private void ReverseCards()
        {
            _isForwardAnimation = false;

            _cards.Reverse();
            foreach (var movableCard in _cards)
            {
                movableCard.Sequence.PlayBackwards();
                movableCard.Sequence.OnPause(() => movableCard.Sequence.Kill());
                _cardDeck.PullCardBack(movableCard.Card);
            }
            
            _cards.Clear();
        }

        private bool TakeCardFromDeck()
        {
            if (!_cardDeck.TakeCard(out var card)) return false;
            
            var sequence = DOTween.Sequence();
            sequence //Тут всего 3 точки destination при надобности можно упростить и через методы расширения скармливать трансформы 
                .Join(card.GetComponent<RectTransform>().DOMove(_destinationPoints[0].position, 0.2f).SetEase(Ease.OutQuad))
                .Join(card.transform.DORotate(new Vector3(0, -90, 0), 0.12f).SetEase(Ease.OutBack))
                .AppendCallback(() => card.ActivateMainView(_isForwardAnimation))
                .Append(card.transform.DORotate(new Vector3(0, -180, 0), 0.12f).SetEase(Ease.OutBack))
                .AppendCallback(() =>      {
                    if (_isForwardAnimation) sequence.Pause();
                })
                .Append(card.GetComponent<RectTransform>().DOMove(_destinationPoints[1].position, 0.1f).SetEase(Ease.OutQuad))
                .AppendCallback(() =>
                {
                    if (_isForwardAnimation) sequence.Pause();
                })
                .Append(card.GetComponent<RectTransform>().DOMove(_destinationPoints[2].position, 0.1f).SetEase(Ease.OutQuad))
                .SetAutoKill(false)
                .Pause()
                ;
                
            _cards.Add(new MovableCard(card, sequence));

            return true;
        }

        private class MovableCard
        {
            public Card Card { get; }
            public Sequence Sequence { get; }

            public MovableCard(Card card, Sequence sequence)
            {
                Card = card;
                Sequence = sequence;
            }
        }
    }
}