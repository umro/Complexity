namespace Complexity.AriaEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MLCBank")]
    public partial class MLCBank
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MLCBank()
        {
            MLCLeaves = new HashSet<MLCLeaf>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long MLCBankSer { get; set; }

        [Required]
        [StringLength(16)]
        public string MLCBankId { get; set; }

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

        public long AddOnSer { get; set; }

        public double? MaxLeafExposure { get; set; }

        public double? MaxLeafSpan { get; set; }

        public virtual MLC MLC { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MLCLeaf> MLCLeaves { get; set; }
    }
}
