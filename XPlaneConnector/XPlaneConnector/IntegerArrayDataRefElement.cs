using System;
using System.Collections.Generic;

namespace XPlaneConnector
{
    public class IntegerArrayDataRefElement : IDataRefElement
    {
        private static readonly object lockElement = new object();
        public string DataRef { get; set; }
        public int Frequency { get; set; }
        public int ArrayLength { get; set; }
        public string Units { get; set; }
        public string Description { get; set; }
        public List<int> Values { get; set; }
        public int IsInitialized { get; set; }
        public bool IsCompletelyInitialized => IsInitialized >= ArrayLength;

        public delegate void NotifyChangeHandler(IntegerArrayDataRefElement sender, List<int> newValue);
        public event NotifyChangeHandler OnValueChange;

        public bool Update(int index, int value)
        {
            lock (lockElement)
            {
                var fireEvent = !IsCompletelyInitialized;

                if (!IsCompletelyInitialized)
                    IsInitialized++;

                if (Values.Count <= index)
                {
                    Values.Add(value);
                }

                if (index < Values.Count)
                {
                    var current = Values[index];
                    if (current != value)
                    {
                        Values[index] = value;
                        fireEvent = true;
                    }
                }

                if (IsCompletelyInitialized && fireEvent)
                {
                    OnValueChange?.Invoke(this, Values);
                    IsInitialized = 0;
                    return true;
                }

                return false;
            }
        }

        public bool Update(int id, float value)
        {
            throw new NotImplementedException();
        }

        public IntegerArrayDataRefElement()
        {
            IsInitialized = 0;
            Values = new List<int>();
        }
    }
}
