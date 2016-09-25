namespace Complexity.AriaEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FieldAddOn")]
    public partial class FieldAddOn
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long AddOnSer { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long RadiationSer { get; set; }

        public long? CacheKeySer { get; set; }

        public long? SlotSer { get; set; }

        [StringLength(64)]
        public string CustomCode { get; set; }

        public int? DicomSeqNumber { get; set; }

        public short UserPreselection { get; set; }

        [Required]
        [StringLength(32)]
        public string HstryUserName { get; set; }

        public DateTime HstryDateTime { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] HstryTimeStamp { get; set; }

        [StringLength(32)]
        public string HstryTaskName { get; set; }

        public virtual AddOn AddOn { get; set; }
    }
}
