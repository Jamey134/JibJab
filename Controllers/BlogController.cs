using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JibJab.Models;

//Added for Include:
using Microsoft.EntityFrameworkCore;

//ADDED for session check
using Microsoft.AspNetCore.Mvc.Filters;

namespace JibJab.Controllers;

[SessionCheck]
public class BlogController : Controller
{
    private readonly ILogger<BlogController> _logger;

    // Add field - adding context into our class // "db" can eb any name
    private readonly MyContext db;

    public BlogController(ILogger<BlogController> logger, MyContext context)
    {
        _logger = logger;
        db = context;
    }

    //Use RESTful routing 
    // -------Dashboard-------
    [HttpGet("blogs")]
    public IActionResult Index()
    {
        {
            //Many to many, add guests list and invited guests, to get 3 tables of information
            //List<Blog> blogs = db.Weddings.Include(g => g.Guests).ThenInclude(u => u.User).Include(c => c.Creator).ToList();
            return View("All");
        }
    }

    //-------Display Page For Wedding Form------
    [HttpGet("blogs/new")]
    public IActionResult New()
    {
        return View("New"); //<--- HTML page to see our new, displayed wedding
    }

    //-------Add a Blog into db--------
    [HttpPost("blogs/create")]
    public IActionResult Create(Blog newBlog) //<----- Method to create a wedding and add in db
    {
        if (ModelState.IsValid) //<--- validation 
        {
            newBlog.UserId = (int)HttpContext.Session.GetInt32("UUID");
            db.Blogs.Add(newBlog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        else
        {
            // call the method to render the new page
            return View("New");
        }
    }
}