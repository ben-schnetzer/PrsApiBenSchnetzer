using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PrsApi.Models;

[Table("Users")]
public partial class User1
{
    [Key]
    public int Id { get; set; }
}
