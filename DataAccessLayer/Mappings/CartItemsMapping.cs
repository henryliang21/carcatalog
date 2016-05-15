using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using DataModel;

namespace DataAccessLayer.Mappings
{
    public class CartItemsMapping : ClassMap<CartItem>
    {
        public CartItemsMapping()
        {
            Id(x => x.ItemId).Column("ItemId");
            Map(x => x.CartId).Column("CartId");
            Map(x => x.Quantity).Column("Quantity");
            Map(x => x.DateCreated).Column("DateCreated");
            References(x => x.Product).Column("ProductId");
        }
    }
}
