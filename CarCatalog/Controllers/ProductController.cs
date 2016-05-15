
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
        [System.Web.Http.ActionName("DefaultAction")]
        public IHttpActionResult Index()
        {
            ProductsRepo = new ProductsRepository();
            var productList = from p in ProductsRepo.List<Product>()
                              where p.Category.CategoryName == "Cars"
                              select new { p.ProductId, p.ProductName, p.UnitPrice, p.ImagePath };
            return Json(productList);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("DefaultAction")]
        public IHttpActionResult Details(int param)
        {
            ProductsRepo = new ProductsRepository();
            var product = ProductsRepo.Get<Product>(param);
            return Json(product);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("Search")]
        public IHttpActionResult Search(string param)
        {
            ProductsRepo = new ProductsRepository();
            var productList = from p in ProductsRepo.List<Product>()
                              where p.Category.CategoryName == "Cars" 
                                && (p.ProductName.Contains(param) || p.Description.Contains(param))
                              select new { p.ProductId, p.ProductName, p.UnitPrice, p.ImagePath };
            return Json(productList);
        }
    }
}
