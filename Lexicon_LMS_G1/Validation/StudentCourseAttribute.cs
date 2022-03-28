using Lexicon_LMS_G1.Areas.Identity.Pages.Account;
using Lexicon_LMS_G1.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static Lexicon_LMS_G1.Areas.Identity.Pages.Account.LoginModel;

namespace Lexicon_LMS_G1.Validation
{
    public class StudentCourseAttribute : ValidationAttribute
    {
        const string errorMessage = "You need to select the course the student should be enrolled in";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var model = validationContext.ObjectInstance as RegisterModel.InputModel;
            if(model.Role == "Student")
            {
                if(value == null)
                    return new ValidationResult(errorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
