#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace JibJab.Models;

public class Bloggers
{
    [Key]
    public int BloggersId { get; set; }

    //------User Foreign Key-------
    public int UserId { get; set; }
    public User? User { get; set; }


    //------Wedding Foreign Key-------
    public int PostId { get; set; }
    public Blog? Blog { get; set; }

    public DateTime CreatedAt {get;set;} = DateTime.Now;        
    public DateTime UpdatedAt {get;set;} = DateTime.Now;

}