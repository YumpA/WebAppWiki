using Microsoft.AspNetCore.Mvc;
using WebAppWiki.BusinessLogic;
using WebAppWiki.BusinessLogic.Filters;
using WebAppWiki.Helpers;
using WebAppWiki.Models.Filters;

namespace WebAppWiki.Controllers
{
    public class FiltersController : Controller
    {
        private readonly ServiceFilter _serviceFilter;

        public FiltersController(ServiceFilter serviceFilter)
        {
            _serviceFilter = serviceFilter;
        }

        public IActionResult FiltersView()
        {
            var filter = HttpContext.Session.LoadObject<FilterMain>();

            var model = _serviceFilter.ProjectToModel(filter);

            return View(filter);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ApplyFilterGameName(FilterMainModel model)
        {
            return UpdateFilter(model);
        }

        public IActionResult UpdateFilter(FilterMainModel model)
        {
            var filter = HttpContext.Session.LoadObject<FilterMain>();
            
            _serviceFilter.FiltersUpdate(filter, model);

            HttpContext.Session.SaveObject(filter);

			string? returnUrl = Url.Action("List", "Games");
			return Redirect(returnUrl);
		}
    }
}
