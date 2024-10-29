using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CIDA.Domain.Entities;

[Table("T_OP_INSIGHT")]
// Unique key IdResumo
[Index(nameof(IdResumo), IsUnique = true)]
public class Insight
{
    [Key]
    [Column("ID_INSIGHT")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdInsight { get; set; }

    [Required]
    [ForeignKey("Usuario")]
    [Column("ID_USUARIO")]
    public int IdUsuario { get; set; }

    public Usuario Usuario { get; set; }

    [Required]
    [ForeignKey("Resumo")]
    [Column("ID_RESUMO")]
    public int IdResumo { get; set; }

    [Required]
    [Column("DATA_GERACAO", TypeName = "DATE")]
    public DateTime DataGeracao { get; set; }

    [Required]
    [Column("DESCRICAO")]
    [StringLength(1000000)]
    public string Descricao { get; set; }
}