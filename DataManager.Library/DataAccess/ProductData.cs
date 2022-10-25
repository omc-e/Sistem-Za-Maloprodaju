using DataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Library.Internal.DataAccess
{
    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            SQLDataAccess sql = new SQLDataAccess();

            

            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_getAll", new { }, "Sistem-Za-Maloprodaju-DB");

            return output;
        }

        public ProductModel GetProductById (int productId)
        {
            SQLDataAccess sql = new SQLDataAccess();



            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_getById", new { Id = productId }, "Sistem-Za-Maloprodaju-DB").FirstOrDefault();

            return output;

        }
    }
}
