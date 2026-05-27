using RegistroReceitas.Enums;

namespace RegistroReceitas.Models;

public class Receita
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataRegistro { get; set; }
    public decimal Custo { get; set; }
    public TipoReceita TipoReceita { get; set; }
}
