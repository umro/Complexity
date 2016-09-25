namespace Complexity.AriaEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MLC")]
    public partial class MLC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MLC()
        {
            MLCBanks = new HashSet<MLCBank>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long AddOnSer { get; set; }

        [StringLength(254)]
        public string ManufacturerName { get; set; }

        [StringLength(64)]
        public string MLCSerialNumber { get; set; }

        [StringLength(64)]
        public string MLCModel { get; set; }

        public double? Rotation { get; set; }

        [StringLength(64)]
        public string SupportedFiles { get; set; }

        public int ArcEnableFlag { get; set; }

        public int DoseEnableFlag { get; set; }

        public double MinDoseDynamicLeafGap { get; set; }

        public double MinArcDynamicLeafGap { get; set; }

        public double MaxLeafSpeed { get; set; }

        public double DoseDynamicLeafTolerance { get; set; }

        public double ArcDynamicLeafTolerance { get; set; }

        public double? MinStaticLeafGap { get; set; }

        public double MinSegmentThreshold { get; set; }

        public int MaxControlPoints { get; set; }

        public double? SourceDistance { get; set; }

        public double? ParallelJawSetBack { get; set; }

        public double? PerpendicularJawSetBack { get; set; }

        public double? MaxPerpendicularJawOpening { get; set; }

        public double NominalLeafLength { get; set; }

        public double? FitMLCToBlockMarginDefault { get; set; }

        public short? FitMLCToBlockMarginMethodDefault { get; set; }

        public virtual AddOn AddOn { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MLCBank> MLCBanks { get; set; }
    }
}
