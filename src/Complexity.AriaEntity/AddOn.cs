namespace Complexity.AriaEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AddOn")]
    public partial class AddOn
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AddOn()
        {
            FieldAddOns = new HashSet<FieldAddOn>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long AddOnSer { get; set; }

        public long ResourceSer { get; set; }

        public long? AddOnMaterialSer { get; set; }

        [Required]
        [StringLength(16)]
        public string AddOnId { get; set; }

        [StringLength(64)]
        public string AddOnName { get; set; }

        [StringLength(30)]
        public string AddOnType { get; set; }

        public DateTime CreationDate { get; set; }

        [StringLength(32)]
        public string CreationUserName { get; set; }

        [Required]
        [StringLength(16)]
        public string ObjectStatus { get; set; }

        [StringLength(32)]
        public string DisplayCode { get; set; }

        public int? InternalCode { get; set; }

        public int ExtDeviceVerification { get; set; }

        public int OverridePossible { get; set; }

        public int ValidationDone { get; set; }

        public int PFVerifyDone { get; set; }

        [StringLength(254)]
        public string Comment { get; set; }

        [Required]
        [StringLength(32)]
        public string HstryUserName { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] HstryTimeStamp { get; set; }

        public DateTime HstryDateTime { get; set; }

        [StringLength(32)]
        public string HstryTaskName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FieldAddOn> FieldAddOns { get; set; }

        public virtual MLC MLC { get; set; }
    }
}
