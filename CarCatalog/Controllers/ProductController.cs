
using System.Linq;
using System.Web.Http;

using DataAccessLayer;
using DataModel;
using System.Web.Mvc;

namespace CarCatalog.Controllers
{
    public class ProductController : ApiController
    {
        private ProductsRepository ProductsRepo;

        [System.Web.Http.HttpGet]
        public IHttpActionResult Index()
        {
            ProductsRepo = new ProductsRepository();
            var productList = from p in ProductsRepo.List<Product>()
                              where p.Category.CategoryName == "Cars"
                              select new { p.ProductId, p.ProductName, p.UnitPrice, p.ImagePath };
            return Json(productList);
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult Details(int id)
        {
            ProductsRepo = new ProductsRepository();
            var product = ProductsRepo.Get<Product>(id);
            return Json(product);
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult Search(string keyword)
        {
            ProductsRepo = new ProductsRepository();
            var productList = from p in ProductsRepo.List<Product>()
                              where p.Category.CategoryName == "Cars" 
                                && (p.ProductName.Contains(keyword) || p.Description.Contains(keyword))
                              select new { p.ProductId, p.ProductName, p.UnitPrice, p.ImagePath };
            return Json(productList);
        }
    }
}
