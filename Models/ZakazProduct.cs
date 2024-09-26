using System;
using System.Collections.Generic;

namespace Order.Models;

public partial class ZakazProduct
{
    public int IdZakaz { get; set; }

    public int IdProduct { get; set; }

    public int? Amount { get; set; }

    public virtual Product IdProductNavigation { get; set; } = null!;

    public virtual Zakaz IdZakazNavigation { get; set; } = null!;
}
