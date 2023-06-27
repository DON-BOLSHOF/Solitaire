using System;
using System.Collections.Generic;
using System.Linq;
using Factories;
using UniRx;
using UnityEngine;

namespace Cards
{
    [Serializable]
    public class CardDeck: MonoBehaviour
    {
        [SerializeField] private int _cardCount;

        private ReactiveStack<Card> _cards = new();
        private ICollection<IDisposable> _cardStream = new List<IDisposable>();

        private CardFactory _cardFactory;

        private void Start()
        {
            _cardFactory = new CardFactory(_cardCount);

            _cards              //Если карты закончились поток подгрузки больше не нужен.
                .ObserveRemove
                .Subscribe(_ =>
                {
                    if(!TopUpDeck())
                        _cardStream.ToList().ForEach(disposable => disposable.Dispose());
                })
                .AddTo(_cardStream);

            TopUpDeck(); // Пусть при запуске будет чисто одна карта
        }

        public bool TakeCard(out Card card)
        {
            if (_cards.Count <= 0)
            {
                card = null;
                return false;
            }

            card =  _cards.Pop();
            
            return true;
        }

        public void PullCardBack(Card card)
        {
            _cards.Push(card);
        }

        private bool TopUpDeck()//Пусть инициализируется динамически, может пользователю не понадобятся все 52 инстанса.
        {
            if (!_cardFactory.TryCreateLimitCard(out var card, transform)) return false;
            
            _cards.Push(card);
            
            return true;
        }
    }
}