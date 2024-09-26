using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using MsBox.Avalonia;
using Order.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace Order;

public partial class Window1 : Window
{
    public float? costOut;
    public Window1()
    {
        InitializeComponent();
        talon.IsVisible = false;
        srok.IsVisible = false;
        codeTalon.IsVisible = false;
        punkt.IsVisible = false;
        cost.IsVisible = false;
        loadKorzProds();
    }

    private void loadKorzProds()
    {
        punktCmb.ItemsSource = Helper.user724Context.PunktVydahis.Select(x => x.NamePunkt).ToList();
        var list = Helper.productsKorz;
        

        listboxKorz.ItemsSource = Helper.productsKorz.Select(
            x => new
            {
                x.IdProduct,
                x.NameProduct,
                Photo = new Bitmap($"Assets" + "\\" + x.Image.ToString()),
                x.Cost,
                x.Descriprion,
                x.IdDiscountNavigation.ValueDiscount,
                x.IdManufacturerNavigation.NameManufacturer,
                amount = amountOutput(x.IdProduct),
                x.bitmap
            });
        costOut = 0;
        foreach (Product pr in Helper.productsKorz)
        {
            costOut += pr.Cost * amountOutput(pr.IdProduct);


        }

        cost.Text = "Общая стоимость составила: "+costOut.ToString();
    }

    private int amountOutput(int id)
    {
        int am = Helper.user724Context.ZakazProducts.Where(x => x.IdProduct == id && x.IdZakaz ==Helper.zakazItem.IdZakaz).FirstOrDefault().Amount.Value;
        return am;
    }
    

        private void CreateZakaz_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
       
            bool result = true;
            Random rand = new Random();
            int code = 0;
            while (code.ToString().Length != 3)
            {
                code = rand.Next();
            }
            Helper.zakazItem.Code = code;
            foreach (ZakazProduct pr in Helper.zakazProduct)
            {
                Product prod = Helper.user724Context.Products.Where(x => x.IdProduct == pr.IdProduct).FirstOrDefault();
            
                if (pr.Amount <= prod.AmountManufacturer &&
                       prod.AmountManufacturer > 3)
                {
                    Helper.zakazItem.IdSrok = 1;
                    Helper.zakazItem.IdSrokNavigation = Helper.user724Context.SrokDostavkis.Where(x => x.IdSrokDost == Helper.zakazItem.IdSrok).FirstOrDefault();
                }
                else
                {
                    Helper.zakazItem.IdSrok = 2;
                    Helper.zakazItem.IdSrokNavigation = Helper.user724Context.SrokDostavkis.Where(x => x.IdSrokDost == Helper.zakazItem.IdSrok).FirstOrDefault();
                }
            
               
            }
            talon.IsVisible = true;
            srok.IsVisible = true;
            codeTalon.IsVisible = true;
            foreach (Product pr in Helper.productsKorz)
            {
                costOut += pr.Cost * amountOutput(pr.IdProduct);

            }
            cost.IsVisible = true;
            cost.Text = "Общая стоимость составила: " + costOut.ToString();
            punkt.IsVisible = true;
            punkt.Text = "Пункт выдачи: ";
            if (Helper.zakazItem.IdPunkt != null)
            {
                punkt.Text = "Пункт выдачи: " + Helper.zakazItem.IdPunktNavigation.NamePunkt.ToString();
            }

            Helper.zakazItem.Date = DateOnly.FromDateTime(DateTime.Now.Date);
            date.Text ="Дата формирования заказа: " + Helper.zakazItem.Date.ToString();
            srok.Text = "Срок доставки: " + Helper.zakazItem.IdSrokNavigation.ValueSrok;
            codeTalon.Text = "Код заказа: " + code.ToString();
            if (Helper.zakazItem.IdPunkt != null && Helper.zakazItem.ZakazProducts.Count() > 0)
            {
                Helper.user724Context.Zakazs.Where(x => x.IdZakaz == Helper.zakazItem.IdZakaz).FirstOrDefault().Code = code;
                Helper.user724Context.Zakazs.Where(x => x.IdZakaz == Helper.zakazItem.IdZakaz).FirstOrDefault().IdPunkt = Helper.zakazItem.IdPunkt;
                Helper.user724Context.Zakazs.Where(x => x.IdZakaz == Helper.zakazItem.IdZakaz).FirstOrDefault().IdSrok = Helper.zakazItem.IdSrok;
                Helper.user724Context.Zakazs.Where(x => x.IdZakaz == Helper.zakazItem.IdZakaz).FirstOrDefault().Date = Helper.zakazItem.Date;
                Helper.user724Context.Zakazs.Where(x => x.IdZakaz == Helper.zakazItem.IdZakaz).FirstOrDefault().IdStatus = 2;
                var box = MessageBoxManager.GetMessageBoxStandard("Уведомление", "Заказ успешно сформирован.");
                box.ShowAsync();
                Helper.productsKorz.Clear();
                Helper.Zakaz.Clear();
                Helper.zakazItem = null;
                Helper.zakazProductItem = null;
                listboxKorz.ItemsSource = null;

            }
            else
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Невозможно создать заказ. Проверьте, что выбран пункт выдачи  и наличие выбранных товаров.");
                box.ShowAsync();
            }

        



    }

        private void Minus_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int amount;
        int ind = (int)((sender as Button)!).Tag!;
        
        Zakaz zakaz = new Zakaz();
        zakaz = Helper.user724Context.Zakazs.Where(x => x.IdZakaz == Helper.zakazItem.IdZakaz).FirstOrDefault();
        amount = Helper.user724Context.ZakazProducts.Where(x => x.IdZakaz == Helper.zakazItem.IdZakaz && x.IdProduct == ind).FirstOrDefault().Amount.Value-1;

        Helper.user724Context.ZakazProducts.Where(x => x.IdZakaz == Helper.zakazItem.IdZakaz && x.IdProduct == ind).FirstOrDefault().Amount = amount;
        if (amount == 0)
        {
            ZakazProduct zakazProduct = new ZakazProduct();
            zakazProduct = zakaz.ZakazProducts.Where(x => x.IdProduct == ind ).FirstOrDefault();
            Product pr = Helper.productsKorz.Where(x => x.IdProduct ==ind).FirstOrDefault();
            

            Helper.user724Context.ZakazProducts.Remove(zakazProduct);
            Helper.user724Context.SaveChanges();
            if (zakaz.ZakazProducts.Count()==0)
            {
                Helper.user724Context.Zakazs.Remove(zakaz);
                Helper.zakazItem = null;
            }
            Helper.user724Context.SaveChanges();
            Helper.productsKorz.Remove(pr);
        }
        loadKorzProds();
    }

    private void Plus_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int ind = (int)((sender as Button)!).Tag!;
        int id = Helper.user724Context.ZakazProducts.Where(x => x.IdProduct == ind).FirstOrDefault().IdZakaz;
        int amount = Helper.user724Context.ZakazProducts.Where(x => x.IdProduct == ind && x.IdZakaz == Helper.zakazItem.IdZakaz).FirstOrDefault().Amount.Value;

        if (amount < Helper.user724Context.Products.Where(x => x.IdProduct == ind).FirstOrDefault().AmountManufacturer)
        {
            amount = amount + 1;
            Helper.user724Context.ZakazProducts.Where(x => x.IdProduct == ind && x.IdZakaz == Helper.zakazItem.IdZakaz).FirstOrDefault().Amount = amount;

            Helper.user724Context.SaveChanges();

        }
        else
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Вы выбрали максимальное количество товара имеющееся на складе.");
            box.ShowAsync();
        }
       
        loadKorzProds();
    }

    private void ComboBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        Helper.zakazItem.IdPunkt = punktCmb.SelectedIndex+1;
        Helper.zakazItem.IdPunktNavigation = Helper.user724Context.PunktVydahis.Where(x => x.IdPunkt == Helper.zakazItem.IdPunkt).FirstOrDefault();
    }

    private void Vyhod_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainWindow main = new MainWindow();
        main.Show();
        this.Close();

    }

}