using System;

namespace MoneyGram.Common.Models
{
    [Serializable]
    public class KeyValueItem<T>
    {
        public string Key { get; set; }
        public T Val { get; set; }
    }
}
