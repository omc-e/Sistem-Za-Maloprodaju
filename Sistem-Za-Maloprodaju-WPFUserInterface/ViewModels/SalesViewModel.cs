﻿using AutoMapper;
using Caliburn.Micro;
using DesktopUI.Library.API;
using DesktopUI.Library.Models;
using DesktopUI;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DesktopUI.Models;

namespace DesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        IProductEndPoint _productEndPoint;
        ISaleEndpoint _saleEndpoint;
        IMapper _mapper;
        StatusInfoViewModel _status;
         IWindowManager _window;

        public  SalesViewModel(IProductEndPoint productEndPoint, ISaleEndpoint saleEndpoint, IMapper mapper, StatusInfoViewModel status,
            IWindowManager window)
        {
            _productEndPoint = productEndPoint;  
            _saleEndpoint = saleEndpoint;
            _mapper = mapper;
            _status = status;
            _window = window;
        }
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadProducs();
            }
            catch(Exception ex)
            {

                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                //var info = IoC.Get<StatusInfoViewModel>();

                if(ex.Message == "Unauthorized")
                {
                    _status.UpdateMessage("Unauthorized Access", "You do not have permission to interact with the Sales Form");
                    await _window.ShowDialogAsync(_status, null, settings);
                }
                else
                {
                    _status.UpdateMessage("Fatal Exceptiom", ex.Message);
                    await _window.ShowDialogAsync(_status, null, settings);
                }
                
                TryCloseAsync();
            }
        }
        private async Task LoadProducs()
        {
            var productList = await _productEndPoint.GetAll();
            var products = _mapper.Map<List<ProductDisplayModel>>(productList);
            Products = new BindingList<ProductDisplayModel>(products);
        }

        private BindingList<ProductDisplayModel> _products;
        public BindingList<ProductDisplayModel> Products
        {
            get { return _products; }
            set { _products = value; 
            NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductDisplayModel _selectedProduct;

        public ProductDisplayModel SelectedProduct
        {
            get { return _selectedProduct; }
            set { 
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        private CartItemDisplayModel _selectedCartItem;

        public CartItemDisplayModel SelectedCartItem
        {
            get { return _selectedCartItem; }
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
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

        private async Task ResetSalesViewModel()
        {
            Cart = new BindingList<CartItemDisplayModel>();
            await LoadProducs();
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
          
            NotifyOfPropertyChange(() => CanCheckOut);

        }

        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>() ;

        public  BindingList<CartItemDisplayModel> Cart
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
            CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
              

            }
            else
            {
                CartItemDisplayModel item = new CartItemDisplayModel
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
                if (SelectedCartItem != null && SelectedCartItem?.QuantityInCart > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        public void RemoveFromCart()
        {


            SelectedCartItem.Product.QuantityInStock += 1;

            if (SelectedCartItem.QuantityInCart > 1)
            {
                SelectedCartItem.QuantityInCart -= 1;
                
            }
            else
            {
                
                Cart.Remove(SelectedCartItem);
            }
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
            NotifyOfPropertyChange(() => CanAddToCart);  
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

            await ResetSalesViewModel();
        }
        

    }
}
