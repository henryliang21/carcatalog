using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Product
    {
        public virtual int ProductId
        {
            get; set;
        }

        public virtual string ProductName
        {
            get; set;
        }

        public virtual string Description
        {
            get; set;
        }

        public virtual string ImagePath
        {
            get; set;
        }

        public virtual decimal UnitPrice
        {
            get; set;
        }

        public virtual Category Category
        {
            get; set;
        }
    }
}
