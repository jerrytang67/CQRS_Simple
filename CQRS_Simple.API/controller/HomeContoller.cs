using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_Simple.controller
{
    [Route("[controller]")]

    public class HomeContoller : ControllerBase
    {
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}