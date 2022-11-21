using Common;
using Makta.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Email;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Makta.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailSender _emailSender;

        public HomeController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }
      
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult  Blog()
            {
            return View();
        }  public IActionResult  BlogDetails()
            {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Index2()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Subscribe(string email)
        {
            try
            {
                if(!Validators.IsValidEmail(email))
                    return new JsonResult("Please enter a valid email address.");

                _emailSender.SendNewSubscribe(email);
                return new JsonResult("You are in! Thanks for being a part of Makta community.");
            }
            catch(Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}
