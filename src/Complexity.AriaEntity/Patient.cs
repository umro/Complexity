namespace Complexity.AriaEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Patient")]
    public partial class Patient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Patient()
        {
            Courses = new HashSet<Course>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long PatientSer { get; set; }

        [Required]
        [StringLength(25)]
        public string PatientId { get; set; }

        [StringLength(25)]
        public string PatientId2 { get; set; }

        [Required]
        [StringLength(64)]
        public string PatientUID { get; set; }

        [Required]
        [StringLength(30)]
        public string PatientType { get; set; }

        public DateTime CreationDate { get; set; }

        [StringLength(32)]
        public string CreationUserName { get; set; }

        [StringLength(64)]
        public string SSN { get; set; }

        [StringLength(64)]
        public string FirstName { get; set; }

        [StringLength(64)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(64)]
        public string LastName { get; set; }

        [StringLength(16)]
        public string NameSuffix { get; set; }

        [StringLength(16)]
        public string Honorific { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(16)]
        public string Sex { get; set; }

        [StringLength(64)]
        public string WorkPhone { get; set; }

        [StringLength(64)]
        public string HomePhone { get; set; }

        [StringLength(64)]
        public string Citizenship { get; set; }

        [StringLength(64)]
        public string Race { get; set; }

        [StringLength(64)]
        public string BirthCountry { get; set; }

        [StringLength(64)]
        public string BirthState { get; set; }

        [StringLength(64)]
        public string BirthCounty { get; set; }

        [StringLength(64)]
        public string BirthCity { get; set; }

        [StringLength(32)]
        public string PatientIdIssuer { get; set; }

        [StringLength(64)]
        public string MaidenName { get; set; }

        [StringLength(64)]
        public string MothersMaidenName { get; set; }

        [StringLength(64)]
        public string MedRecordLocator { get; set; }

        [StringLength(16)]
        public string Language { get; set; }

        [StringLength(64)]
        public string Occupation { get; set; }

        [StringLength(64)]
        public string SpecialNeeds { get; set; }

        [StringLength(64)]
        public string ReligiousPreference { get; set; }

        public DateTime? ArchiveDate { get; set; }

        [StringLength(32)]
        public string ArchiveVolume { get; set; }

        [StringLength(254)]
        public string Comment { get; set; }

        public int ClinicalTrialFlag { get; set; }

        [StringLength(254)]
        public string FileDataSiteID { get; set; }

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

        [StringLength(64)]
        public string MobilePhone { get; set; }

        [StringLength(64)]
        public string Ethnicity { get; set; }

        public long? MobilePhoneProviderSer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course> Courses { get; set; }
    }
}
