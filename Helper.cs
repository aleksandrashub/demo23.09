using Order.Context;
using Order.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order
{
    public static class Helper
    {
        public static readonly MyprojContext user724Context = new MyprojContext();
        public static List<Product> productsKorz = new List<Product>();
        public static List<ZakazProduct> zakazProduct = new List<ZakazProduct>();
        public static List<Zakaz> Zakaz = new List<Zakaz>();
        public static Zakaz zakazItem = new Zakaz();
        public static ZakazProduct zakazProductItem = new ZakazProduct();
    }
}
