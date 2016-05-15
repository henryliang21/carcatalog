using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class OrderDetail
    {
        public virtual int OrderDetailId
        {
            get; set;
        }

        public virtual Order Order
        {
            get; set;
        }

        public virtual string Username
        {
            get; set;
        }

        public virtual int ProductId
        {
            get; set;
        }

        public virtual int Quantity
        {
            get; set;
        }

        public virtual decimal UnitPrice
        {
            get; set;
        }
    }
}
