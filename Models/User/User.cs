using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_2.Models;

public class User
{
    // [Key]
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }

    // [Required]
    public string account { get; set; }

    // [Required]
    public string password { get; set; }
    
    public int role { get; set; }
 
    public virtual InformationUser InformationUser { get; set; }
}