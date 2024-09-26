using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;

namespace Order.Models;

public partial class Product
{
    public int IdProduct { get; set; }

    public string? NameProduct { get; set; }

    public string? Descriprion { get; set; }

    public int? IdManufacturer { get; set; }

    public float? Cost { get; set; }

    public int? IdDiscount { get; set; }

    public string? Image { get; set; }

    public int? AmountManufacturer { get; set; }

    public virtual Discount? IdDiscountNavigation { get; set; }

    public virtual Manufacturer? IdManufacturerNavigation { get; set; }

    public virtual ICollection<ZakazProduct> ZakazProducts { get; set; } = new List<ZakazProduct>();

    public Bitmap? bitmap => Image != null ? new Bitmap($@"Assets\\{Image}") : null;

}
