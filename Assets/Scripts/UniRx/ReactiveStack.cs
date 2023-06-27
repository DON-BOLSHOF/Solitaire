using System;

namespace UniRx
{
    public class ReactiveStack<T>//Обертка, реализующая часть реализации, при надобности расширяемая.
    {
        private static ReactiveCollection<T> _reactiveCollection = new();

        public IObservable<CollectionRemoveEvent<T>> ObserveRemove = _reactiveCollection.ObserveRemove();
        public IObservable<CollectionAddEvent<T>> ObserveAdd = _reactiveCollection.ObserveAdd();

        public int Count => _reactiveCollection.Count;
        
        public void Push(T item)
        {
            _reactiveCollection.Add(item);
        }

        public T Pop()
        {
            var instance = _reactiveCollection[^1];
            _reactiveCollection.RemoveAt(_reactiveCollection.Count-1);

            return instance;
        }
    }
}