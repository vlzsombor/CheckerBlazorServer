using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CheckerBlazorServer.Areas.Identity.Pages.Account
{


    public class LoginModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }

        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ReturnUrl = Url.Content("~/");
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, true, lockoutOnFailure: false);
                if (result.Succeeded)
                    return LocalRedirect(ReturnUrl);
            }
            return Page();
        }

        public void OnGet()
        {
            ReturnUrl = Url.Content("~/");
        }


        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

    }
}
