using System;

namespace DesktopUI.Library.Models
{
    public interface ILoggedInUserModel
    {
        DateTime CreatedDate { get; set; }
        string EmailAddress { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Token { get; set; }
        string UserId { get; set; }

        void LogOffUSer();
    }
}