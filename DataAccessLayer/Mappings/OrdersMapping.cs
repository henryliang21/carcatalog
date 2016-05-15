using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using DataModel;

namespace DataAccessLayer.Mappings
{
    public class OrdersMapping : ClassMap<Order>
    {
        public OrdersMapping()
        {
            Id(x => x.OrderId).Column("OrderId");
            Map(x => x.OrderDate).Column("OrderDate");
            Map(x => x.Username).Column("Username");
            Map(x => x.FirstName).Column("FirstName");
            Map(x => x.LastName).Column("LastName");
            Map(x => x.Address).Column("Address");
            Map(x => x.City).Column("City");
            Map(x => x.State).Column("State");
            Map(x => x.PostalCode).Column("PostalCode");
            Map(x => x.Country).Column("Country");
            Map(x => x.Phone).Column("Phone");
            Map(x => x.Email).Column("Email");
            Map(x => x.Total).Column("Total");
            Map(x => x.PaymentTransactionId).Column("PaymentTransactionId");
            Map(x => x.HasBeenShipped).Column("HasBeenShipped");
        }
    }
}
