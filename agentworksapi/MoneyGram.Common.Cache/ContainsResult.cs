namespace MoneyGram.Common.Cache
{
    public class ContainsResult<T>
    {
        public T CachedObj { get; set; }
        public bool Exists { get; set; }
    }
}
