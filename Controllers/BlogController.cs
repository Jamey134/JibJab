using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JibJab.Models;

//Added for Include:
using Microsoft.EntityFrameworkCore;

//ADDED for session check
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Caching.Memory;

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
            // One to Many 
            //List<Blog> blogs = db.Blogs.Include(c => c.Creator).ToList();

            //Many to many, add guests list and invited guests, to get 3 tables of information
            List<Blog> blogs = db.Blogs.Include(g => g.Bloggers).ThenInclude(u => u.User).Include(c => c.Creator).ToList();
            return View("All", blogs);
        }
    }

    //-------Display Page For Blog Form------
    [HttpGet("blogs/new")]
    public IActionResult New()
    {
        return View("New"); //<--- HTML page to see our new, displayed post
    }

    //-------Add a Blog into db--------
    [HttpPost("blogs/create")]
    public IActionResult Create(Blog newBlog) //<----- Method to create blog and add in db
    {
        if (ModelState.IsValid) //<--- validation 
        {
            newBlog.UserId = (int)HttpContext.Session.GetInt32("UUID");
            db.Blogs.Add(newBlog);
            Console.WriteLine("----------------->" + newBlog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        else
        {
            // call the method to render the new page
            return View("New");
        }
    }

    


    [HttpGet("blog/{id}")]
    public IActionResult Details(int id)
    {

        Blog? blogs = db.Blogs.Include(a => a.Creator).FirstOrDefault(f => f.PostId == id);

        if (blogs == null)
        {
            return RedirectToAction("Index");
        }

        else
        {
            return View("Details", blogs);
        }
    }


    // Render Edit Page
    [HttpGet("blog/{id}/edit")]

    //add id in parameter
    public IActionResult Edit(int id)
    {
        // confirm it matches the id we're passing in above
        Blog? blogs = db.Blogs.Include(v => v.Creator).FirstOrDefault(p => p.PostId == id);

        //confirming the creator of the blog is editing 
        if (blogs == null || blogs.UserId != HttpContext.Session.GetInt32("UUID")) //<--- (Session check)
        {
            return RedirectToAction("Index");
        }
        //passing blog data down to view
        return View("Edit", blogs);
    }

    // ----------Update Blog-----------
    [HttpPost("blog/{id}/update")]

    //add id in parameter
    public IActionResult Update(Blog editedBlog, int id)
    {
        if (!ModelState.IsValid)
        {
            return Edit(id);
        }
        // confirm it matches the id we're passing in above
        Blog? blogs = db.Blogs.Include(v => v.Creator).FirstOrDefault(p => p.PostId == id);

        //confirming the creator of the blog is editing 
        if (blogs == null || blogs.UserId != HttpContext.Session.GetInt32("UUID")) //<--- (Session check)
        {
            return RedirectToAction("Index");
        }

        blogs.BlogTitle = editedBlog.BlogTitle;
        blogs.BlogContent = editedBlog.BlogContent;
        blogs.Description = editedBlog.Description;
        blogs.ImageURL = editedBlog.ImageURL;
        blogs.UpdatedAt = DateTime.Now;

        db.Blogs.Update(blogs);
        db.SaveChanges();
        //passing blog data down to view
        return RedirectToAction("Index", blogs);
    }


[HttpGet("blog/{id}/delete")]
public IActionResult Delete(int id)
{
    // Find the blog by id
    Blog? blog = db.Blogs.FirstOrDefault(b => b.PostId == id);

    // Check if the blog exists and belongs to the current user
    if (blog == null || blog.UserId != HttpContext.Session.GetInt32("UUID"))
    {
        // If not found or doesn't belong to the user, redirect to the "Index" action
        return RedirectToAction("Index");
    }

    // Remove the blog from the database
    db.Blogs.Remove(blog);
    db.SaveChanges();

    // After successful deletion, redirect to the "Index" action
    return RedirectToAction("Index");
}


    [HttpPost("blogs/{id}/likes")]
    public IActionResult Like(int id)
    {
        //First get the session
        int? userId = HttpContext.Session.GetInt32("UUID");

        // // if session value is null, send back to dashboard.
        // if (userId == null)
        // {
        //     return RedirectToAction("Index");
        // }

        Blogger? existingLike = db.Bloggers.FirstOrDefault(l => l.UserId == userId.Value && l.PostId == id);

        if (existingLike != null)
        {
            db.Bloggers.Remove(existingLike);
        }
        else
        {
            // Create a new Blogger object and set its properties
            Blogger newLike = new Blogger
            {
                PostId = id,       // Set PostId property
                UserId = userId.Value   // Set UserId property
            };

            Console.WriteLine(newLike);
            db.Bloggers.Add(newLike);
        }

        db.SaveChanges();
        return RedirectToAction("Index", "Blog");
    }

}
