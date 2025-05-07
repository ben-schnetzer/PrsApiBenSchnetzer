using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PrsApi.Models
{
    [Keyless]
    public class LogIn
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
