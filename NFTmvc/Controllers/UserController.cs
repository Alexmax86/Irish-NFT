// import the necessary namespaces
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NFTmvc.Models;
using NFTmvc.Areas.Identity.Data;
using Newtonsoft.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using NFTmvc.Data;


namespace NFTmvc.Controllers;

public class UserController : Controller
{
    // inject the UserManager and HttpContext classes in your controller or service
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserController(UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IActionResult> userid()
    {
        // retrieve the ID of the current user
        string userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
        return Content(userId);
    }
    
}