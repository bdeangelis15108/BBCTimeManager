using Microsoft.AspNetCore.Mvc;
using Nucleus.Web.Controllers;

namespace Nucleus.Web.Public.Controllers
{
    public class HomeController : NucleusControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}