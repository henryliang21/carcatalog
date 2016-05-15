using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using DataModel;

namespace DataAccessLayer.Mappings
{
    public class ProductsMapping : ClassMap<Product>
    {
        public ProductsMapping()
        {
            Table("Products");
            Id(x => x.ProductId).Column("ProductId");
            Map(x => x.ProductName).Column("ProductName");
            Map(x => x.Description).Column("Description");
            Map(x => x.ImagePath).Column("ImagePath");
            Map(x => x.UnitPrice).Column("UnitPrice");
            References(x => x.Category).Column("CategoryID");
        }
    }
}