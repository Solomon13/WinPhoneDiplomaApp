using System;
using System.Collections.Generic;

namespace WinPhoneClient.Common
{
    public class LimitedQueue<T> : Queue<T>
    {
        #region Fields

        private int _maxSize;

        #endregion
        #region Constructor
        public LimitedQueue(int maxSize)
        {
            if (maxSize <= 0)
                throw new ArgumentException();

            _maxSize = maxSize;
        }

        public LimitedQueue() : this(50)
        {
        }
        #endregion
        #region Methods

        public void Add(T newElement)
        {
            if (ReferenceEquals(newElement, null))
                throw new ArgumentException();

            if (Count == MaxSize)
                Dequeue();

            Enqueue(newElement);
        }

        public void AddRange(IEnumerable<T> range)
        {
            if (range != null)
            {
                foreach (var item in range)
                    Add(item);
            }
        }
        #endregion

        #region Properties

        public int MaxSize
        {
            get { return _maxSize; }
            set
            {
                if (value <= 0)
                    return;

                if (_maxSize != value)
                {
                    if (value < _maxSize && Count > value)
                    {
                        while (Count > value)
                            Dequeue();
                    }

                    _maxSize = value;
                }
            }
        }
        #endregion
    }
}
