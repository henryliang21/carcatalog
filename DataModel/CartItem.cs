using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class CartItem
    {
        public virtual Guid ItemId
        {
            get; set;
        }

        public virtual Guid CartId
        {
            get; set;
        }

        public virtual int Quantity
        {
            get; set;
        }

        public virtual DateTime DateCreated
        {
            get; set;
        }

        public virtual Product Product
        {
            get; set;
        }
    }
}
