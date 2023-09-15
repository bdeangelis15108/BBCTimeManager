using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace Nucleus.Web.Controllers
{
    public class HomeController : NucleusControllerBase
    {
        [DisableAuditing]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Ui");
        }
    }
}
