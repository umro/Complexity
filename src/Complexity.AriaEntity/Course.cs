namespace Complexity.AriaEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Course")]
    public partial class Course
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Course()
        {
            PlanSetups = new HashSet<PlanSetup>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long CourseSer { get; set; }

        public long PatientSer { get; set; }

        [Required]
        [StringLength(16)]
        public string CourseId { get; set; }

        public DateTime? StartDateTime { get; set; }

        [Required]
        [StringLength(16)]
        public string ClinicalStatus { get; set; }

        [StringLength(32)]
        public string CompletedByUserName { get; set; }

        public DateTime? CompletedDateTime { get; set; }

        [StringLength(254)]
        public string Comment { get; set; }

        [StringLength(254)]
        public string ClinicalProtocolDir { get; set; }

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

        [StringLength(255)]
        public string TransactionId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanSetup> PlanSetups { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
