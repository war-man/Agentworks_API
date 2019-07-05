using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoneyGram.Common.Attributes
{
    public class EnumMetadataAttribute : Attribute
    {
        public string ResourceItemName { get; set; }
        public int DisplayOrder { get; set; }
        public string FilterString { get; set; }

        public EnumMetadataAttribute()
        {
        }

        public EnumMetadataAttribute(string resourceItemName, int displayOrder, string filterString = "")
        {
            ResourceItemName = resourceItemName;
            DisplayOrder = displayOrder;
            FilterString = filterString;
        }
    }
}
