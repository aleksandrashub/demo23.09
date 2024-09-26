using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Metsys.Bson;
using Microsoft.EntityFrameworkCore;
using Order.Models;
using System.Collections.Generic;
using System.Linq;

namespace Order
{
    
    public partial class MainWindow : Window
    {
        public bool vis;
        public List<Product> prods = Helper.user724Context.Products.Include(x => x.IdManufacturerNavigation).Include(x => x.IdDiscountNavigation).ToList();
        public MainWindow()
        {
            InitializeComponent();
            loadProds();
        }
        /*
       public void Right_Click (object? sender, Avalonia.Interactivity.RoutedEventArgs e)
       {
           //   int ind = (int)((sender as MenuItem)!).Tag!;


               var item = listbox.SelectedItem as Product;

               Helper.Zakaz.Add(item);





       }*/

        private void loadProds()
        {

            var list = prods;

            listbox.ItemsSource = list;
            if (Helper.productsKorz.Count > 0)
            {
                prosmotr.IsVisible = true;
            }
            else
            {
                prosmotr.IsVisible = false;
            }
           

        }

       



        private void VKorzinu_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Window1 window2 = new Window1();
            window2.Show();
            this.Close();

        }

      

        private void MenuItem_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {

            var product =  listbox.SelectedItem as Product;
            ZakazProduct zakazProduct = new ZakazProduct();
            zakazProduct.IdProduct = product.IdProduct;
            if (Helper.zakazItem != null)
            {
                zakazProduct.IdZakazNavigation = Helper.zakazItem;
                zakazProduct.IdZakaz = Helper.zakazItem.IdZakaz;
                zakazProduct.Amount = 1;
            }
            else
            {
                Zakaz zakaz = new Zakaz();
                if (Helper.user724Context.Zakazs.Count() == 0)
                {
                    zakaz.IdZakaz = 1;
                }
                else
                {
                    zakaz.IdZakaz = Helper.user724Context.Zakazs.OrderBy(x => x.IdZakaz).Last().IdZakaz + 1;
                }
                zakaz.IdStatus = 1;
                zakazProduct.Amount = 1;
                zakazProduct.IdZakaz = zakaz.IdZakaz;
                zakazProduct.IdZakazNavigation = zakaz;
                Helper.user724Context.Zakazs.Add(zakaz);
                Helper.user724Context.SaveChanges();
                Helper.zakazItem = zakaz;

            }


            if (!Helper.productsKorz.Contains(product))
            {
                product.ZakazProducts.Add(zakazProduct);
                Helper.zakazProduct.Add(zakazProduct);
                Helper.productsKorz.Add(product);
                zakazProduct.IdProductNavigation = product;
                Helper.user724Context.ZakazProducts.Add(zakazProduct);
                Helper.user724Context.SaveChanges();

            }
           
           
            loadProds();
        }
    }
}
