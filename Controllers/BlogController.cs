using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JibJab.Models;

//Added for Include:
using Microsoft.EntityFrameworkCore;

//ADDED for session check
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
            List<Blog> blogs = db.Blogs.Include(c => c.Creator).ToList();
            
            //Many to many, add guests list and invited guests, to get 3 tables of information
            //List<Blog> blogs = db.Weddings.Include(g => g.Guests).ThenInclude(u => u.User).Include(c => c.Creator).ToList();
            return View("All", blogs);
        }
    }

    //-------Display Page For Blog Form------
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

//---------View One (READ) Wedding----------
    // [HttpGet("blogs/{postId}")] //<--- Double check the id nomenclature
    // public IActionResult Details(int postId)
    // {
    //     //Include many to many guests list
    //     Blog? wedding = db.Blogs.Include(g => g.Guests).ThenInclude(u => u.User).FirstOrDefault(w => w.WeddingId == weddingId);

    //     if (wedding == null)
    //     {
    //         return RedirectToAction("Index");
    //     }
    //     return View("Details", wedding);
    // }


    [HttpGet("blog/{id}")]
    public IActionResult Details(int id){
        
        Blog? blogs = db.Blogs.Include(a => a.Creator).FirstOrDefault(f => f.PostId == id);

        if(blogs == null){
        return RedirectToAction("Index");
        }

        else{
            return View("Details", blogs);
        }
    }


    // ----------Update Blog-----------
    [HttpGet("blog/{id}/edit")]

    //add id in parameter*
    public IActionResult Edit(int id)
    {
        // confirm it matches the id we're passing in above*
    Blog? blogs = db.Blogs.Include(v => v.Creator).FirstOrDefault(p => p.PostId == id);

    //confirming the creator of the wedding is editing 
    if (blogs == null || blogs.UserId != HttpContext.Session.GetInt32("UUID")) //<--- (Session check)
    {
        return RedirectToAction("Index");
    }
        //passing weddings data down to view
        return View("Edit", blogs);
    }

    //---------Delete a wedding---------
    [HttpPost("blog/{id}/delete")]
    public IActionResult Delete(int id)

    
    {
        Blog? blogs = db.Blogs.FirstOrDefault(d => d.PostId == id);

        //To stop from deleting other users' data
        if(blogs == null || blogs.UserId != HttpContext.Session.GetInt32("UUID")) 
        {
            return RedirectToAction("Index");
        }

        db.Blogs.Remove(blogs);
        db.SaveChanges();
        return RedirectToAction("Index");
    }
}