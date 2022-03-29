using Lexicon_LMS_G1.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lexicon_LMS_G1.ViewComponents
{
    public class DeleteViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke(string deleteController, string returnController = "", string returnAction = "",
            int returnId = -1, string deleteModalId = "delete1")
        {
            var model = new DeleteViewModel
            {
                DeleteController = deleteController,
                ReturnController = returnController,
                ReturnId = returnId,
                ReturnAction = returnAction,
                DeleteModalId = deleteModalId
            };

            return View(model);
        }

    }
        
}
