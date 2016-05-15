using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using DataModel;

namespace DataAccessLayer.Mappings
{
    public class CategoriesMapping : ClassMap<Category>
    {
        public CategoriesMapping()
        {
            Table("Categories");
            Id(x => x.CategoryId).Column("CategoryID");
            Map(x => x.CategoryName).Column("CategoryName");
            Map(x => x.Description).Column("Description");
        }
    }
}
