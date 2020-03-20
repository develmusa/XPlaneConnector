using System;
using System.Collections.Generic;

namespace XPlaneConnector
{
    public class FloatArrayDataRefElement
    {
        private static readonly object lockElement = new object();
        public string DataRef { get; set; }
        public int Frequency { get; set; }
        public int ArrayLength { get; set; }
        public string Units { get; set; }
        public string Description { get; set; }
        public List<float> Values { get; set; }

        private int ValuesInitialized;

        public bool IsCompletelyInitialized => ValuesInitialized >= ArrayLength;

        public delegate void NotifyChangeHandler(FloatArrayDataRefElement sender, List<float> newValue);
        public event NotifyChangeHandler OnValueChange;

        public void Update(int index, float value)
        {
            lock (lockElement)
            {
                var fireEvent = !IsCompletelyInitialized;

                if (!IsCompletelyInitialized)
                    ValuesInitialized++;

                //Values.Add(value);
                //fireEvent = true;

                if (Values.Count <= index)
                {
                    Values.Add(value);
                }

                var current = Values[index];
                if (Math.Abs(current - value) > 0.001f)
                {
                    Values[index] = value;
                    fireEvent = true;
                }

                if (IsCompletelyInitialized && fireEvent)
                {
                    OnValueChange?.Invoke(this, Values);
                    ValuesInitialized = 0;
                }
            }
        }

        public FloatArrayDataRefElement()
        {
            ValuesInitialized = 0;
            Values = new List<float>();
        }
    }
}
