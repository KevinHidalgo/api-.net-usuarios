using System;
using System.Collections.Generic;

namespace NetCoreApi.Models;

public partial class Sesione
{
    public int IdSesion { get; set; }

    public int? IdUsuario { get; set; }

    public string? Token { get; set; }

    public DateTime? FechaInicio { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
