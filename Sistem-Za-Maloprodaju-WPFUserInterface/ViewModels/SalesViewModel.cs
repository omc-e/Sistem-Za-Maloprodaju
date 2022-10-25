using Caliburn.Micro;
using DesktopUI.Library.API;
using DesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sistem_Za_Maloprodaju_WPFUserInterface.ViewModels
{
    public class SalesViewModel : Screen
    {
        IProductEndPoint _productEndPoint;
        ISaleEndpoint _saleEndpoint;
       
        public  SalesViewModel(IProductEndPoint productEndPoint, ISaleEndpoint saleEndpoint)
        {
            _productEndPoint = productEndPoint;  
            _saleEndpoint = saleEndpoint;
          
        }
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducs();
        }
        private async Task LoadProducs()
        {
            var productList = await _productEndPoint.GetAll();
            Products = new BindingList<ProductModel>(productList);
        }

        private BindingList<ProductModel> _products;
        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set { _products = value; 
            NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductModel _selectedProduct;

        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set { 
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }




        private int _itemQuantity = 1;

        public  int ItemQuantity
        {
            get { return _itemQuantity; }
            set { _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
                    }
        }

        

        public string  SubTotal
        {
            get {
                decimal subTotal = CalculateSubTotal();
                
                return subTotal.ToString("C", CultureInfo.CurrentCulture);
            }
            
        }

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;
            subTotal = Cart.Sum(x => x.QuantityInCart * x.Product.RetailPrice);
            //foreach (var item in Cart)
            //{
            //    subTotal += (item.Product.RetailPrice * item.QuantityInCart);
            //}

            return subTotal;
        }
        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            double a = 0.17;
            decimal tempTax = (decimal)a;

        taxAmount = Cart.Sum(x => x.Product.RetailPrice * x.QuantityInCart * tempTax);
            //foreach (var item in Cart)
            //{
            //    taxAmount += (item.Product.RetailPrice * item.QuantityInCart * tempTax);
            //}
            return taxAmount;
        }

        public string Tax
        {
            get
            {
                decimal taxAmount = CalculateTax();

                return taxAmount.ToString("C");
            }

        }
        public string Total
        {
            get
            {
                decimal total = CalculateSubTotal() + CalculateTax();
                return total.ToString("C");
            }

        }


        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>() ;

        public  BindingList<CartItemModel> Cart
        {
            get { return _cart; }
            set { _cart = value;
                NotifyOfPropertyChange(() => Cart);

            }
        }


        public bool CanAddToCart
        {
            get
            {
                bool output = false;
                
                //Check quantity

                if(ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    output = true;
                }
                
                return output;
            }
        }

        public void AddToCart()
        {
            CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
                Cart.Remove(existingItem);
                Cart.Add(existingItem);
            }
            else
            {
                CartItemModel item = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(item);
            }
            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => existingItem.DisplayText);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;

                //Make sure something is selected
                

                return output;
            }
        }

        public void RemoveFromCart()
        {
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public bool CanCheckOut
        {
            get
            {
                bool output = false;

                //Make sure there is something in the cart
                if (Cart.Count > 0)
                {
                    output = true;
                }


                return output;
            }
        }

        public async Task CheckOut()
        {
            SaleModel sale = new SaleModel();

            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
                

            }

            await _saleEndpoint.PostSale(sale);
        }
        

    }
}
