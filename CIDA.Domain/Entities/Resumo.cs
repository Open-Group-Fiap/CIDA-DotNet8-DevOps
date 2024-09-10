using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIDA.Domain.Entities;

[Table("T_OP_RESUMO")]
public class Resumo
{
    [Key]
    [Column("ID_RESUMO")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdResumo { get; set; }

    [Required]
    [ForeignKey("Usuario")]
    [Column("ID_USUARIO")]
    public int IdUsuario { get; set; }

    public Usuario Usuario { get; set; }

    [Required]
    [Column("DATA_GERACAO", TypeName = "DATE")]
    public DateTime DataGeracao { get; set; }

    [Required]
    [Column("DESCRICAO")]
    [StringLength(8000)]
    public string Descricao { get; set; }

    // Relacionamento com Insights
    public ICollection<Insight> Insights { get; set; }
    public ICollection<Arquivo> Arquivos { get; set; }
}