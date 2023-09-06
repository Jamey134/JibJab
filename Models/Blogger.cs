#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace JibJab.Models;

public class Blogger
{
    [Key]
    public int BloggerId { get; set; }

    //------User Foreign Key-------
    public int UserId { get; set; }
    public User? User { get; set; }


    //------Blog Foreign Key-------
    public int PostId { get; set; }
    public Blog? Blog { get; set; }

    public DateTime CreatedAt {get;set;} = DateTime.Now;        
    public DateTime UpdatedAt {get;set;} = DateTime.Now;

}