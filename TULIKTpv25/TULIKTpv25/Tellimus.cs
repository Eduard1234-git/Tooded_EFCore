using Microsoft.EntityFrameworkCore;
namespace Tooded_EFCore;

public class Tellimus
{
    public int Id { get; set; }
    public DateTime Kuupaev { get; set; }
    public string Klient { get; set; } = "";
    [Precision(10, 2)]
    public decimal Kokku { get; set; }
    public List<TellimuseRida> Read { get; set; } = new();
}

public class TellimuseRida
{
    public int Id { get; set; }
    public int TellimusId { get; set; }
    public Tellimus Tellimus { get; set; } = null!;
    public string ToodeNimi { get; set; } = "";   // koopia nimest: tellimus ei muutu, kui toode kustutatakse
    public int Kogus { get; set; }
    [Precision(10, 2)]
    public decimal Hind { get; set; }
}
