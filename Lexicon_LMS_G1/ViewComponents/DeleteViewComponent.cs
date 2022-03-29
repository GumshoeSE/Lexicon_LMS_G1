using Lexicon_LMS_G1.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lexicon_LMS_G1.ViewComponents
{
    public class DeleteViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke(string deleteController, string returnController = "", int returnId = -1)
        {
            var model = new DeleteViewModel
            {
                DeleteController = deleteController,
                ReturnController = returnController,
                ReturnId = returnId
            };

            return View(model);
        }

    }
        
}
