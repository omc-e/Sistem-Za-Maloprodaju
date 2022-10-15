using Caliburn.Micro;
using DesktopUI.Library.API;
using DesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistem_Za_Maloprodaju_WPFUserInterface.ViewModels
{
    public class SalesViewModel : Screen
    {
        IProductEndPoint _productEndPoint;
        public  SalesViewModel(IProductEndPoint productEndPoint)
        {
            _productEndPoint = productEndPoint;      
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

        private int _itemQuantity;

        public  int ItemQuantity
        {
            get { return _itemQuantity; }
            set { _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                    }
        }

        

        public string  SubTotal
        {
            get {
                return "$0.00";
            }
            
        }

        public string Tax
        {
            get
            {
                return "$0.00";
            }

        }
        public string Total
        {
            get
            {
                return "$0.00";
            }

        }


        private BindingList<ProductModel> _cart;

        public  BindingList<ProductModel> Cart
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
                
                //Make sure something is selected
                //Make sure there is an item quantity
                
                return output;
            }
        }

        public void AddToCart()
        {

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

        }

        public bool CanCheckOut
        {
            get
            {
                bool output = false;

                //Make there is something in the cart


                return output;
            }
        }

        public void CheckOut()
        {

        }



    }
}
