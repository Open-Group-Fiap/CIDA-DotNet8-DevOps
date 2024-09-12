using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CIDA.Domain.Entities;

[Table("T_OP_ARQUIVO")]
//Unique key URL
[Index(nameof(Url), IsUnique = true)]
public class Arquivo
{
    [Key]
    [Column("ID_ARQUIVO")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdArquivo { get; set; }

    [Required]
    [ForeignKey("Usuario")]
    [Column("ID_USUARIO")]
    public int IdUsuario { get; set; }

    [ForeignKey("Resumo")]
    [Column("ID_RESUMO")]
    public int? IdResumo { get; set; }

    [Required]
    [Column("NOME")]
    [StringLength(255)]
    public string Nome { get; set; }

    [Required]
    [Column("EXTENSAO")]
    [StringLength(255)]
    public string Extensao { get; set; }

    [Required] [Column("TAMANHO")] public int Tamanho { get; set; }

    [Required] [Column("DATA_UPLOAD")] public DateTime DataUpload { get; set; }

    [Required]
    [Column("URL")]
    [StringLength(255)]
    public string Url { get; set; }
}