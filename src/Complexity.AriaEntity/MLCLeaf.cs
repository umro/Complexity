namespace Complexity.AriaEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MLCLeaf")]
    public partial class MLCLeaf
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long MLCLeafSer { get; set; }

        public long MLCBankSer { get; set; }

        [Required]
        [StringLength(16)]
        public string LeafId { get; set; }

        public int LeafNumber { get; set; }

        public double? OffsetY { get; set; }

        public double? MaxRetractPosition { get; set; }

        public double? MaxExtendPosition { get; set; }

        public double Width { get; set; }

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

        public virtual MLCBank MLCBank { get; set; }
    }
}
