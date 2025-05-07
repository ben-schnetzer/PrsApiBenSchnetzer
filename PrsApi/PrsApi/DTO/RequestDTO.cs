using Microsoft.EntityFrameworkCore;
using PrsApi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PrsApi.DTO
{
    public class RequestDTO
    {
        //[Key]
        //[Column("ID")]
        public int Id { get; set; }

        //[Column("UserID")]
        public int UserId { get; set; }

        //[StringLength(100)]
        //[Unicode(false)]
        public string Description { get; set; } = null!;

        //[StringLength(255)]
        //[Unicode(false)]
        public string Justification { get; set; } = null!;

        public DateOnly DateNeeded { get; set; }

        //[StringLength(25)]
        //[Unicode(false)]
        public string DeliveryMode { get; set; } = null!;
    }
}
