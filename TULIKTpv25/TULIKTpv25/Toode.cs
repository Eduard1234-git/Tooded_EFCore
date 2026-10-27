using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace Tooded_EFCore;

public class Toode
{
    public int Id { get; set; }
    public string Toodenimetus { get; set; } = "";
    public int Kogus { get; set; }
    [Precision(10, 2)]
    public decimal Hind { get; set; }
    [Precision(10, 2)]
    public decimal? VanaHind { get; set; }   // soodustuse korral: hind enne allahindlust
    public string? Pilt { get; set; }
    public int KategooriaId { get; set; }
    public Kategooria Kategooria { get; set; } = null!;

    [NotMapped]
    public int? SoodustusProtsent =>
        VanaHind is decimal v && v > Hind ? (int)Math.Round((1 - Hind / v) * 100) : null;
}
