using DataManager.Library.Internal.DataAccess;
using DataManager.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DataManager_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Cashier")]
    public class ProductController : ControllerBase
    {
        private IConfiguration _config;

        public ProductController(IConfiguration config)
        {
            _config = config;
        }
        [HttpGet]
        public List<ProductModel> Get()
        {
            ProductData data = new ProductData(_config);

            return data.GetProducts();
        }
    }
}
