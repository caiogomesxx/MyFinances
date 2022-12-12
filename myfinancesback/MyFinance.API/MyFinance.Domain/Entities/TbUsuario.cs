using System;
using System.Collections.Generic;

namespace MyFinance.Infra.Context;

public partial class TbUsuario
{
    public int IdUsuario { get; set; }

    public string DsNome { get; set; } = null!;

    public string DsTelefone { get; set; } = null!;

    public string DsEmail { get; set; } = null!;

    public string DsSenha { get; set; } = null!;
}
