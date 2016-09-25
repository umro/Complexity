namespace Complexity.AriaEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PlanSetup")]
    public partial class PlanSetup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PlanSetup()
        {
            Radiations = new HashSet<Radiation>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long PlanSetupSer { get; set; }

        public long? PatientSupportDeviceSer { get; set; }

        public long? PrescriptionSer { get; set; }

        public long CourseSer { get; set; }

        [Required]
        [StringLength(16)]
        public string PlanSetupId { get; set; }

        [StringLength(64)]
        public string PlanSetupName { get; set; }

        public DateTime CreationDate { get; set; }

        [StringLength(32)]
        public string CreationUserName { get; set; }

        [Required]
        [StringLength(64)]
        public string Status { get; set; }

        [Required]
        [StringLength(32)]
        public string StatusUserName { get; set; }

        public DateTime StatusDate { get; set; }

        public double? PlanNormFactor { get; set; }

        [StringLength(64)]
        public string PlanNormMethod { get; set; }

        [StringLength(254)]
        public string Comment { get; set; }

        [StringLength(16)]
        public string TreatmentTechnique { get; set; }

        [StringLength(16)]
        public string ApplicationSetupType { get; set; }

        public int? ApplicationSetupNumber { get; set; }

        [Required]
        [StringLength(16)]
        public string TreatmentType { get; set; }

        public string CalcModelOptions { get; set; }

        public int CalcModelOptionsLen { get; set; }

        [StringLength(16)]
        public string TreatmentOrientation { get; set; }

        public double? PrescribedPercentage { get; set; }

        public long? PrimaryPTVSer { get; set; }

        public int IrregFlag { get; set; }

        public string FieldRules { get; set; }

        public int FieldRulesLen { get; set; }

        public double? SkinFlashMargin { get; set; }

        [StringLength(64)]
        public string ProtocolPhaseId { get; set; }

        public int MultiFieldOptFlag { get; set; }

        public long? CopyOfSer { get; set; }

        public long? StructureSetSer { get; set; }

        public long? EquipmentSer { get; set; }

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

        [Column(TypeName = "image")]
        public byte[] RaySearchPrivateData { get; set; }

        public int RaySearchPrivateDataLen { get; set; }

        [StringLength(16)]
        public string Intent { get; set; }

        [MaxLength(96)]
        public byte[] ViewingPlane { get; set; }

        [MaxLength(24)]
        public byte[] ViewingPlaneULCorner { get; set; }

        [MaxLength(24)]
        public byte[] ViewingPlaneLRCorner { get; set; }

        public double? BrachyPdrPulseInterval { get; set; }

        public int? BrachyPdrNoOfPulses { get; set; }

        [StringLength(255)]
        public string TransactionId { get; set; }

        public long? ImageSer { get; set; }

        public virtual Course Course { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Radiation> Radiations { get; set; }
    }
}
