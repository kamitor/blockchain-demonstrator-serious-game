using System;

namespace Blockchain_Demonstrator_Web_App.Models
{
    /// <summary>
    /// This view model is used by the HomeController Error action.
    /// </summary>
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}