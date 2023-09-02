#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JibJab.Models;
public class Tag
{
    [Key]
    // Represents a globally unique identifier (GUID)
    // GUID is a 128-bit text string that represents an identification (ID) but can store over 300,000+ ID numbers in db
    public Guid TagId { get; set; }

    [Required]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
    public string Name { get; set; }
    [Required]
    [MinLength(3, ErrorMessage = "Display name must be at least 3 characters long.")]
    public string DisplayName {get; set;}

//-------many to many connection----------
// ICollection is another type of collection, which derives from IEnumerable and extends it's functionality to modify (Add or Update or Remove) data.
    public ICollection<Post> Posts {get; set;}
}


