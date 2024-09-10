using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CIDA.Domain.Entities;

// Unique Key Email
[Index(nameof(Email), IsUnique = true)]
[Table("T_OP_AUTENTICACAO")]
public class Autenticacao
{
    [Key]
    [Column("ID_AUTENTICACAO")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdAutenticacao { get; set; }

    [Required]
    [Column("EMAIL")]
    [StringLength(255)]
    public string Email { get; set; }

    [Required]
    [Column("HASH_SENHA")]
    [StringLength(255)]
    public string HashSenha { get; set; }

    public Usuario Usuario { get; set; }
}