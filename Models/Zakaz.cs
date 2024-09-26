using System;
using System.Collections.Generic;

namespace Order.Models;

public partial class Zakaz
{
    public int IdZakaz { get; set; }

    public DateOnly? Date { get; set; }

    public int? Code { get; set; }

    public int? IdPunkt { get; set; }

    public int? IdStatus { get; set; }

    public int? IdSrok { get; set; }

    public virtual PunktVydahi? IdPunktNavigation { get; set; }

    public virtual SrokDostavki? IdSrokNavigation { get; set; }

    public virtual StatusZakaz? IdStatusNavigation { get; set; }

    public virtual ICollection<ZakazProduct> ZakazProducts { get; set; } = new List<ZakazProduct>();
}
