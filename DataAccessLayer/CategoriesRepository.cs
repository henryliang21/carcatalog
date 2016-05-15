using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataModel;
using RepositoryFramework.GenericRepository;

namespace DataAccessLayer
{
    public class CategoriesRepository : NHGenericRepository<Category, int>
    {
    }
}
