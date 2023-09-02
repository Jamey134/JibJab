#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JibJab.Models;
public class Blog
{
    [Key]
    // Represents a globally unique identifier (GUID)
    // GUID is a 128-bit text string that represents an identification (ID) but y
    public int PostId { get; set; }

    // [Required]
    // [MinLength(3, ErrorMessage = "Heading must be at least 3 characters long.")]
    // public string Heading { get; set; }
    [Required]
    [MinLength(3, ErrorMessage = "Title must be at least 3 characters long.")]
    public string BlogTitle {get; set;}
    [Required]
    [MaxLength(20, ErrorMessage = "Content must more than 20 characters")]
    public string BlogContent{ get; set; }
    [Required]
    [MinLength(10, ErrorMessage = "Description must be at least 10 characters long.")]
    public string Description{ get; set; }
    [Required]
    public string ImageURL{ get; set; }
    [Required]
    public string UrlHandle {get; set;}
    [Required]
    [MinLength(3, ErrorMessage = "Your name must be at least 3 characters long.")]
    public string Author {get; set;}
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    
    //-----foreign key/one-to-many connection------
    public int UserId {get; set;}

    public User? Creator {get; set;}




//-------many to many connection----------
// ICollection is another type of collection, which derives from IEnumerable and extends it's functionality to modify (Add or Update or Remove) data.
    //public ICollection<Tag> Tags {get; set;}

}