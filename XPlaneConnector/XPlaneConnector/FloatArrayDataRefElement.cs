using System;
using System.Collections.Generic;

namespace XPlaneConnector
{
    public class FloatArrayDataRefElement : IDataRefElement
    {
        private static readonly object lockElement = new object();
        public string DataRef { get; set; }
        public int Frequency { get; set; }
        public int ArrayLength { get; set; }
        public string Units { get; set; }
        public string Description { get; set; }
        public List<float> Values { get; set; }
        public int IsInitialized { get; set; }

        public bool IsCompletelyInitialized => IsInitialized >= ArrayLength;

        public delegate void NotifyChangeHandler(FloatArrayDataRefElement sender, List<float> newValue);
        public event NotifyChangeHandler OnValueChange;

        public bool Update(int index, float value)
        {
            lock (lockElement)
            {
                var fireEvent = !IsCompletelyInitialized;

                if (!IsCompletelyInitialized)
                    IsInitialized++;

                //Values.Add(value);
                //fireEvent = true;

                if (Values.Count <= index)
                {
                    Values.Add(value);
                }

                if (index < Values.Count)
                {
                    var current = Values[index];
                    if (Math.Abs(current - value) > 0.001f)
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

        public FloatArrayDataRefElement()
        {
            IsInitialized = 0;
            Values = new List<float>();
        }
    }
}
