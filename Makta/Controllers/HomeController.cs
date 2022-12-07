using Common;
using Makta.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult BlogDetails()
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
                if (!Validators.IsValidEmail(email))
                    return new JsonResult("Please enter a valid email address.");

                _emailSender.SendNewSubscribe(email);

                _emailSender.SaveData($"subscribe - {email}");

                return new JsonResult("You are in! Thanks for being a part of Makta community.");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        public IActionResult joinus()
        {
            return View();
        }

        [HttpPost]
        public IActionResult submitjoinus(string data)
        {
            try
            {
                if (string.IsNullOrEmpty(data))
                    return new JsonResult(new ReturnClass { Description = "Please fill the form and try again.", IsSucceed = false, ButtonText = "Got it!" });

                var model = JsonConvert.DeserializeObject<JoinUsDto>(data);
                if (model == null)
                    return new JsonResult(new ReturnClass { Description = "something went wrong, please try again later.", IsSucceed = false, ButtonText = "Sure!" });

                if (!Validators.IsValidEmail(model.Email))
                    return new JsonResult(new ReturnClass { Description = "Please enter a valid email address.", IsSucceed = false, ButtonText = "ok let me check!" });

                _emailSender.SendEmailtoAdmin(data);

                _emailSender.SaveData($"joinus - {data}");
                return new JsonResult(new ReturnClass { Description = "Thanks for being a part of Makta community. A community member will process your request and will contact you no more than 2 business days.", IsSucceed = true, ButtonText = "Cool, thanks!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}
