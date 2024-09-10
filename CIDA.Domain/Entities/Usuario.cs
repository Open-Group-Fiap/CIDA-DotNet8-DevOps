using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CIDA.Domain.Entities;

[Table("T_OP_USUARIO")]
// Unique Key NumDocumento and IdAutenticacao
[Index(nameof(NumDocumento), nameof(IdAutenticacao), IsUnique = true)]
public class Usuario
{
    [Key]
    [Column("ID_USUARIO")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdUsuario { get; set; }

    [Required]
    [ForeignKey("Autenticacao")]
    [Column("ID_AUTENTICACAO")]
    public int IdAutenticacao { get; set; }

    public Autenticacao Autenticacao { get; set; }

    [Required]
    [Column("NOME")]
    [StringLength(255)]
    public string Nome { get; set; }

    [Required]
    [Column("TIPO_DOCUMENTO")]
    [StringLength(4)]
    public TipoDocumento TipoDocumento { get; set; }

    [Required]
    [Column("NUM_DOCUMENTO")]
    [StringLength(255)]
    public string NumDocumento { get; set; }

    [Required]
    [Column("TELEFONE")]
    [StringLength(255)]
    public string Telefone { get; set; }

    [Required]
    [Column("DATA_CRIACAO", TypeName = "DATE")]
    public DateTime DataCriacao { get; set; }

    [Required]
    [Column("STATUS")]
    [StringLength(255)]
    public Status Status { get; set; }

    // Relacionamentos
    public ICollection<Arquivo> Arquivos { get; set; }
    public ICollection<Resumo> Resumos { get; set; }
    public ICollection<Insight> Insights { get; set; }
}