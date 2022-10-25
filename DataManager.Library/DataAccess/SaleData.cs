using DataManager.Library.Internal.DataAccess;
using DataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Library.DataAccess
{
    public class SaleData
    {
        public void SaveSale(SaleModel SaleInfo, string cashierId)
        {
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            ProductData products = new ProductData();
            double a = 0.17;
            decimal taxRate = (decimal)a;
            foreach (var item in SaleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                var productInfo = products.GetProductById(item.ProductId);
                if (productInfo == null)
                {
                    throw new Exception($"The product Id of {item.ProductId} could not be found in the database.");
                }

                detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);
                detail.Tax = (detail.PurchasePrice * taxRate);

                details.Add(detail);
            }

            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId
                
            };
            sale.Total = sale.SubTotal + sale.Tax;

            SQLDataAccess sql = new SQLDataAccess();
            sql.SaveData("dbo.spSale_Insert", sale, "Sistem-Za-Maloprodaju-DB");

            //Get the ID from the sale model
          sale.Id =  sql.LoadData<int, dynamic>("spSale_Lookup",new { sale.CashierId, sale.SaleDate } , "Sistem-Za-Maloprodaju-DB").FirstOrDefault();

            foreach (var item in details)
            {
                item.SaleId = sale.Id;
            sql.SaveData("dbo.spSaleDetail_Insert", item, "Sistem-Za-Maloprodaju-DB");
            }

        }

        //public List<ProductModel> GetProducts()
        //{
        //    SQLDataAccess sql = new SQLDataAccess();



        //    var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_getAll", new { }, "Sistem-Za-Maloprodaju-DB");

        //    return output;
        //}
    }
}
