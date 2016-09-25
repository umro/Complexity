namespace Complexity.AriaEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Radiation")]
    public partial class Radiation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long RadiationSer { get; set; }

        public long? ResourceSer { get; set; }

        public long PlanSetupSer { get; set; }

        [Required]
        [StringLength(16)]
        public string RadiationId { get; set; }

        [StringLength(64)]
        public string RadiationName { get; set; }

        [Required]
        [StringLength(32)]
        public string RadiationType { get; set; }

        [StringLength(254)]
        public string Comment { get; set; }

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

        public int RadiationNumber { get; set; }

        [StringLength(64)]
        public string TechniqueLabel { get; set; }

        public int? RadiationOrder { get; set; }

        public long? RefImageSer { get; set; }

        [StringLength(254)]
        public string SetupNote { get; set; }

        [StringLength(64)]
        public string RefImageUID { get; set; }

        public long? RefImageSOPClassSer { get; set; }

        public DateTime CreationDate { get; set; }

        [StringLength(32)]
        public string CreationUserName { get; set; }

        public virtual PlanSetup PlanSetup { get; set; }
    }
}
