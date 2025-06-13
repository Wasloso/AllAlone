using System;

namespace Items
{
    [Serializable]
    public class ItemDropEntry
    {
        public Item item;
        public int minQuantity = 1;
        public int maxQuantity = 1;
    }
}