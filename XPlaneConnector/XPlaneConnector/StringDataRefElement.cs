namespace XPlaneConnector
{
    public class StringDataRefElement : IDataRefElement
    {
        private static readonly object lockElement = new object();
        public string DataRef { get; set; }
        public int Frequency { get; set; }
        public int StringLenght { get; set; }
        public string Value { get; set; }
        public int IsInitialized { get; set; }
        public string Units { get; set; }
        public string Description { get; set; }

        public bool IsCompletelyInitialized
        {
            get
            {
                return IsInitialized >= StringLenght;
            }
        }

        public delegate void NotifyChangeHandler(StringDataRefElement sender, string newValue);
        public event NotifyChangeHandler OnValueChange;

        public bool Update(int index, char character)
        {
            lock (lockElement)
            {
                var fireEvent = !IsCompletelyInitialized;

                if (!IsCompletelyInitialized)
                    IsInitialized++;

                if (character > 0)
                {
                    if (Value.Length <= index)
                        Value = Value.PadRight(index + 1, ' ');

                    var current = Value[index];
                    if (current != character)
                    {
                        Value = Value.Remove(index, 1).Insert(index, character.ToString());
                        fireEvent = true;
                    }
                }

                if (IsCompletelyInitialized && fireEvent)
                {
                    OnValueChange?.Invoke(this, Value);
                    IsInitialized = 0;
                    return true;
                }

                return false;
            }
        }

        public bool Update(int id, float value)
        {
            throw new System.NotImplementedException();
        }

        public StringDataRefElement()
        {
            IsInitialized = 0;
            Value = "";
        }
    }
}
