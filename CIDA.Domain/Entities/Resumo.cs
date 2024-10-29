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

    [Required]
    [Column("DATA_GERACAO", TypeName = "DATE")]
    public DateTime DataGeracao { get; set; }

    [Required]
    [Column("DESCRICAO")]
    [StringLength(1000000)]
    public string Descricao { get; set; }
}