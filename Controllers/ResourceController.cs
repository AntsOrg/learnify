using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using learnify.Models;

namespace learnify.Controllers;

public class ResourceController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    [ActivatorUtilitiesConstructor] //solve for multiple constructors
    public ResourceController(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }
    public IActionResult ViewResource()
    {
        var resources = _dbContext.Resources.ToList();
        return View(resources);
    }

    public IActionResult AddResource()
    {
        #pragma warning disable
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext.Session.GetString("User") != null)
        #pragma warning restore
        {
            return View();
        }
        TempData["NotLoggedIn"] = "Login to add your resources.";
        return RedirectToAction("LoginForm","User");
    }


    [HttpPost]
    public IActionResult SubmitResource([FromForm] Resource resource)
    {
        if(ModelState.IsValid){
        try
        {
            #pragma warning disable
            var userId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            resource.UserId = Guid.Parse(userId);
            #pragma warning restore
            _dbContext.Resources.Add(resource);
            _dbContext.SaveChanges();
            TempData["Success"] ="Whoa! captain thanks for your contribution.";
            return RedirectToAction(nameof(ViewResource));
        }
        catch
        {
            return RedirectToAction("AddResource");
        }
 
        }
        TempData["Error"] = "Error inserting data, try again";
        return View();
   }


//Delete
    public IActionResult Delete(Guid Id){
    
    var resId = Id.ToString();
    if(resId == null){
        return NotFound();
    }
    var resource = _dbContext.Resources.FirstOrDefault(r => r.Id == Id);
    if(resource == null){
        return NotFound();
    }
    Console.WriteLine("Deleting resource");
    _dbContext.Resources.Remove(resource);
    _dbContext.SaveChanges();
    Console.WriteLine("Resource deleted");
    return RedirectToAction("Session", "ActivityManager");

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
