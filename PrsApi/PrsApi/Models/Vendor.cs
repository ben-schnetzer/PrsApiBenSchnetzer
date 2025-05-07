using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace PrsApi.Models;

[Table("Vendor")]
[Index("VendorCode", Name = "vcode", IsUnique = true)]
public partial class Vendor
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string VendorCode { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string VendorName { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string VendorAddress { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string VendorCity { get; set; } = null!;

    [StringLength(2)]
    [Unicode(false)]
    public string VendorState { get; set; } = null!;

    [StringLength(5)]
    [Unicode(false)]
    public string VendorZip { get; set; } = null!;

    [StringLength(12)]
    [Unicode(false)]
    public string VendorPhoneNumber { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string VendorEmail { get; set; } = null!;

    //[InverseProperty("Vendor")]
    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
