using DataManager.Library.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Library.Internal.DataAccess
{
    public class ProductData : IProductData
    {

        private readonly ISQLDataAccess _sql;

        public ProductData(ISQLDataAccess sql)
        {

            _sql = sql;
        }
        public List<ProductModel> GetProducts()
        {




            var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProduct_getAll", new { }, "Sistem-Za-Maloprodaju");

            return output;
        }

        public ProductModel GetProductById(int productId)
        {




            var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProduct_getById", new { Id = productId }, "Sistem-Za-Maloprodaju").FirstOrDefault();

            return output;

        }
    }
}
