
using System.Linq;
using System.Web.Http;

using DataAccessLayer;
using DataModel;
using System.Web.Mvc;
using System.Net.Http;

namespace CarCatalog.Controllers
{
    public class ProductController : ApiController
    {
        private ProductsRepository ProductsRepo;

        [System.Web.Http.HttpGet]
        public IHttpActionResult Index()
        {
            var queryStringPairs = Request.GetQueryNameValuePairs();
            var searchQuery = from q in queryStringPairs where string.Compare(q.Key, "Search", true) == 0 select q;
            if (searchQuery.Any())
            {
                var param = searchQuery.First().Value; //TODO: Now only support 1 keyword, can be extended to support multiple keyword, seperated by comma.
                ProductsRepo = new ProductsRepository();
                var productList = from p in ProductsRepo.List<Product>()
                                  where p.Category.CategoryName == "Cars"
                                    && (p.ProductName.Contains(param) || p.Description.Contains(param))
                                  select new { p.ProductId, p.ProductName, p.UnitPrice, p.ImagePath };
                return Json(productList);

            }
            else
            {
                ProductsRepo = new ProductsRepository();
                var productList = from p in ProductsRepo.List<Product>()
                                  where p.Category.CategoryName == "Cars"
                                  select new { p.ProductId, p.ProductName, p.UnitPrice, p.ImagePath };
                return Json(productList);
            }
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult Details(int param)
        {
            ProductsRepo = new ProductsRepository();
            var product = ProductsRepo.Get<Product>(param);
            return Json(product);
        }

        //[System.Web.Http.HttpGet]
        //public IHttpActionResult Search(string param)
        //{
        //    ProductsRepo = new ProductsRepository();
        //    var productList = from p in ProductsRepo.List<Product>()
        //                      where p.Category.CategoryName == "Cars" 
        //                        && (p.ProductName.Contains(param) || p.Description.Contains(param))
        //                      select new { p.ProductId, p.ProductName, p.UnitPrice, p.ImagePath };
        //    return Json(productList);
        //}
    }
}
