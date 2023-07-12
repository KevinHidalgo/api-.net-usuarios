﻿using System;
using System.Collections.Generic;

namespace NetCoreApi.Models;

public partial class Rol
{
    public int IdRol { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
