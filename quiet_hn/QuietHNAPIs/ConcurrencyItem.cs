namespace quiet_hn
{
    public class ConcurrencyItem
    {
        public ConcurrencyItem(int order, HackerNewsEntry hnEntry)
        {
            Order = order;
            HnEntry = hnEntry;
        }

        public HackerNewsEntry HnEntry { get; set; }
        public int Order { get; set; }
    }
}

