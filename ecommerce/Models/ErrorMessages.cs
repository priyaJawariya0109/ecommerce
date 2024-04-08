using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.Models
{
    public static class ErrorMessages
    {
        public static string TenantNotRegisted = "Tenant not registed, Please contact to admin.";

        public static string AlreadyExists = " already exists.";
        public static string SavedMessage = " saved successfully.";
        public static string UpdatedMessage = " updated successfully.";
        public static string DeletedMessage = " deleted successfully.";
        public static string SomethingWrong = "Something went wrong.";
        public static string RecordNotFound = "Record not found.";
        public static string InvalidUserNamePassowrd = "Invalid username/password.";
        public static string ShortCode = "Please enter short code.";
        public static string UserChecked = "Please select ";
        public static string Code = "Please enter code.";
        public static string Enter = "Please enter ";
        public static string Select = "Please select ";
        public static string SelectAction = "Please select action ";
        public static string FileUploadError = "File not uploaded";
        public static string Invalid = "Invalid";

        //signup masssages
        public static string NAME = "Please enter name.";
        public static string USERNAME = "Please enter username.";
        public static string PASSWORD = "Please enter password.";
        public static string USER_TYPE = "Please enter user type.";
        public static string Contact = "Please enter a valid contact number.";
        public static string ContactBlank = "Please enter contact number.";
        public static string UserCode = "Please enter usercode.";
        public static string UserAddress = "Please enter address.";
        public static string UserAddressLength = "Please enter address below 200 characters.";
        public static string PasswordLength = "Please enter password of maximum 15 characters.";
        public static string UsernameLength = "Please enter username of maximum 25 characters.";

        //Accessibility rights
        public static string NoAccess = "You are not authorized to ";
        public static string SomethingWentWrong = " Something went wrong.";


        //error message for clinic expenses
        public static string Date = "Please select date.";
        public static string DateSevenDays = "Please select date between last seven days from today.";
        public static string ExpenseType = "Please select expense type.";
        public static string Vendor = "Please enter vendor.";
        public static string PaymentModeType = "Please select payment mode type.";
        public static string Amount = "Please enter amount.";
        public static string AmountValid = "Please enter valid amount.";
        public static string Remark = "Please enter remark.";
        public static string Clinic = "Please select clinic.";
        public static string SDate = "Please select start date.";
        public static string EDate = "Please select end date.";
        public static string DateSixtyDays = "Two months searching is only applicable i.e 60 days from the start date.";
        public static string Duplicate = "Entry already exist.";
        public static string EndDateGrater = "End Date should be after start date ";
        public static string NoDataFound = "No data found";
    }

    public static class ExceptionHelper
    {
        public static int LineNumber(this Exception e)
        {

            int linenum = 0;
            try
            {
                //linenum = Convert.ToInt32(e.StackTrace.Substring(e.StackTrace.LastIndexOf(":line") + 5));

                //For Localized Visual Studio ... In other languages stack trace  doesn't end with ":Line 12"
                linenum = Convert.ToInt32(e.StackTrace.Substring(e.StackTrace.LastIndexOf(' ')));

            }


            catch
            {
                //Stack trace is not available!
            }
            return linenum;
        }
    }
}
