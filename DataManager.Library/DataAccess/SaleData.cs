using DataManager.Library.Internal.DataAccess;
using DataManager.Library.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Library.DataAccess
{
    public class SaleData : ISaleData
    {

        private IProductData _productData;
        private ISQLDataAccess _sql;

        public SaleData(IProductData productData, ISQLDataAccess sql)
        {

            _productData = productData;
            _sql = sql;
        }
        public void SaveSale(SaleModel SaleInfo, string cashierId)
        {
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();

            double a = 0.17;
            decimal taxRate = (decimal)a;
            foreach (var item in SaleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                var productInfo = _productData.GetProductById(item.ProductId);
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




            try
            {
                _sql.StartTransaction("Sistem-Za-Maloprodaju");


                _sql.SaveDataInTransaction("dbo.spSale_Insert", sale);



                //Get the ID from the sale model
                sale.Id = _sql.LoadDataInTransaction<int, dynamic>("spSale_Lookup", new { sale.CashierId, sale.SaleDate }).FirstOrDefault();

                foreach (var item in details)
                {
                    item.SaleId = sale.Id;
                    _sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                }
                //sql.CommitTransaction();
            }
            catch
            {

                _sql.RollbackTransaction();
                throw;
            }

        }

        public List<SaleReportModel> GetSaleReport()
        {


            var output = _sql.LoadData<SaleReportModel, dynamic>("dbo.spSale_SaleReport", new { }, "Sistem-Za-Maloprodaju-DB");

            return output;
        }
    }
}
