using WebAPI.Entities;

namespace WebAPI
{
    public class ErrorMessages
    {
        public const string NotFoundMessage = "{0} with ID {1} not found!";

        public const string ErrorMessageTemplate = "An error occurred while {0} the {1}!";

        public const string InvalidErrorMessage = "Invalid {0}: {1}";
    }
}
