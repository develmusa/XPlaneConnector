using System;
using System.Collections.Generic;

namespace XPlaneConnector
{
    public class IntegerArrayDataRefElement
    {
        private static readonly object lockElement = new object();
        public string DataRef { get; set; }
        public int Frequency { get; set; }
        public int ArrayLength { get; set; }
        public string Units { get; set; }
        public string Description { get; set; }
        public List<int> Values { get; set; }

        private int ValuesInitialized;

        public bool IsCompletelyInitialized => ValuesInitialized >= ArrayLength;

        public delegate void NotifyChangeHandler(IntegerArrayDataRefElement sender, List<int> newValue);
        public event NotifyChangeHandler OnValueChange;

        public void Update(int index, int value)
        {
            lock (lockElement)
            {
                var fireEvent = !IsCompletelyInitialized;

                if (!IsCompletelyInitialized)
                    ValuesInitialized++;

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
                    ValuesInitialized = 0;
                }
            }
        }

        public IntegerArrayDataRefElement()
        {
            ValuesInitialized = 0;
            Values = new List<int>();
        }
    }
}
