using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JibJab.Models;

//Added for Include:
using Microsoft.EntityFrameworkCore;

//ADDED for session check
using Microsoft.AspNetCore.Mvc.Filters;

namespace JibJab.Controllers;

[SessionCheck]
public class AdminTagController : Controller
{
    private readonly ILogger<AdminTagController> _logger;

    // Add field - adding context into our class // "db" can eb any name
    private readonly MyContext db;

    public AdminTagController(ILogger<AdminTagController> logger, MyContext context)
    {
        _logger = logger;
        db = context;
    }

    //Use RESTful routing 
    // -------Dashboard-------
    [HttpGet("weddings")]
    public IActionResult Index()
    {
        {
            //Many to many, add guests list and invited guests, to get 3 tables of information
            List<Wedding> weddings = db.Weddings.Include(g => g.Guests).ThenInclude(u => u.User).Include(c => c.Creator).ToList();
            return View("All", weddings);
        }
    }

    //-------Display Page For Wedding Form------
    [HttpGet("weddings/new")]
    public IActionResult New()
    {
        return View("New"); //<--- HTML page to see our new, displayed wedding
    }

    //-------Add a wedding into db--------
    [HttpPost("weddings/create")]
    public IActionResult Create(Wedding newWedding) //<----- Method to create a wedding and add in db
    {
        if (ModelState.IsValid) //<--- validation 
        {
            newWedding.UserId = (int)HttpContext.Session.GetInt32("UUID");
            db.Weddings.Add(newWedding);
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