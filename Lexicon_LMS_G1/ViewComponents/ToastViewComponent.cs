using Lexicon_LMS_G1.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lexicon_LMS_G1.ViewComponents
{
    public class ToastViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke(string message)
        {
            var model = new ToastViewModel
            {
                Message = message
            };

            return View(model);
        }
    }
}
