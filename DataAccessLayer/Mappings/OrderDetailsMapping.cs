using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using DataModel;

namespace DataAccessLayer.Mappings
{
    public class OrderDetailsMapping : ClassMap<OrderDetail>
    {
        public OrderDetailsMapping()
        {
            Id(x => x.OrderDetailId).Column("OrderDetailId");
            Map(x => x.Username).Column("Username");
            Map(x => x.Quantity).Column("Quantity");
            Map(x => x.UnitPrice).Column("UnitPrice");
            Map(x => x.ProductId).Column("ProductId");
            References(x => x.Order).Column("OrderId");
        }
    }
}
