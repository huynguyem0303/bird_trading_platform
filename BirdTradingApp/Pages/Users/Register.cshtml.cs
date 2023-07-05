using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using BirdTrading.Utils.Others;
using BirdTrading.ViewModel;
using BirdTradingApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol.Plugins;

namespace BirdTradingApp.Pages.Users
{
    public class RegisterModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegisterModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public RegisterViewModel ResModel { get; set; }

        public async Task<IActionResult> OnPostSignIn()
        {
           return RedirectToPage("../Login/Index");
        }

        public async Task<IActionResult> OnPostCreate()
        {
            if (ModelState.IsValid)
            {
                if (ResModel.Password.Equals(ResModel.ConfirmPassword))
                {
                    if (ResModel.UserFullName.Length > 30 || ResModel.UserFullName.Length < 12)
                    {
                        ModelState.AddModelError(string.Empty, "Full name must be between 12 and 30 characters.");
                        return Page();
                    }
                    if(ResModel.UserFullName.Contains("  "))
                    {
                        ModelState.AddModelError(string.Empty, "Please input valid name.");
                        return Page();
                    }
                    if (ResModel.Password.Length > 30 || ResModel.Password.Length < 6)
                    {
                        ModelState.AddModelError(string.Empty, "Password mus be betwwen 12-30 characters");
                        return Page();
                    }
                    var user = await _unitOfWork.UserRepository.CreateUserAsync(ResModel.UserName, ResModel.Password, ResModel.UserFullName);
                    if (user != null)
                    {
                        HttpContext.Session.SetString("Role", user.Role.ToString());
                        HttpContext.Session.SetInt32("Id", user.Id);
                        HttpContext.Session.SetString("Name", user.Name);
                        TempData["success"] = "Register Succeed";
                        return RedirectToPage("/Index");
                    }
                    ModelState.AddModelError(string.Empty, "Sorry this Email/Phone has been used if you already have an account please Sign In");
                    return Page();
                }              
                ModelState.AddModelError(string.Empty, "Incorrect password confirm!");
            }
            return Page();
        }
    }
}
