namespace Complexity.AriaEntity
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AriaContext : DbContext
    {
        private const string ConnectionString = "data source=<location>;initial catalog=variansystem;persist security info=True;user id=<username>;password=<password>;MultipleActiveResultSets=True;App=EntityFramework";

        public AriaContext() : base(ConnectionString)
        {
            Database.SetInitializer<AriaContext>(null);
        }

        public virtual DbSet<AddOn> AddOns { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<FieldAddOn> FieldAddOns { get; set; }
        public virtual DbSet<MLC> MLCs { get; set; }
        public virtual DbSet<MLCBank> MLCBanks { get; set; }
        public virtual DbSet<MLCLeaf> MLCLeaves { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<PlanSetup> PlanSetups { get; set; }
        public virtual DbSet<Radiation> Radiations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddOn>()
                .Property(e => e.HstryTimeStamp)
                .IsFixedLength();

            modelBuilder.Entity<AddOn>()
                .HasMany(e => e.FieldAddOns)
                .WithRequired(e => e.AddOn)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AddOn>()
                .HasOptional(e => e.MLC)
                .WithRequired(e => e.AddOn)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Course>()
                .Property(e => e.HstryTimeStamp)
                .IsFixedLength();

            modelBuilder.Entity<Course>()
                .Property(e => e.TransactionId)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.PlanSetups)
                .WithRequired(e => e.Course)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FieldAddOn>()
                .Property(e => e.HstryTimeStamp)
                .IsFixedLength();

            modelBuilder.Entity<MLCBank>()
                .Property(e => e.HstryTimeStamp)
                .IsFixedLength();

            modelBuilder.Entity<MLCLeaf>()
                .Property(e => e.HstryTimeStamp)
                .IsFixedLength();

            modelBuilder.Entity<Patient>()
                .Property(e => e.Sex)
                .IsFixedLength();

            modelBuilder.Entity<Patient>()
                .Property(e => e.HstryTimeStamp)
                .IsFixedLength();

            modelBuilder.Entity<Patient>()
                .HasMany(e => e.Courses)
                .WithRequired(e => e.Patient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlanSetup>()
                .Property(e => e.HstryTimeStamp)
                .IsFixedLength();

            modelBuilder.Entity<PlanSetup>()
                .Property(e => e.ViewingPlane)
                .IsFixedLength();

            modelBuilder.Entity<PlanSetup>()
                .Property(e => e.ViewingPlaneULCorner)
                .IsFixedLength();

            modelBuilder.Entity<PlanSetup>()
                .Property(e => e.ViewingPlaneLRCorner)
                .IsFixedLength();

            modelBuilder.Entity<PlanSetup>()
                .Property(e => e.TransactionId)
                .IsUnicode(false);

            modelBuilder.Entity<PlanSetup>()
                .HasMany(e => e.Radiations)
                .WithRequired(e => e.PlanSetup)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Radiation>()
                .Property(e => e.HstryTimeStamp)
                .IsFixedLength();
        }
    }
}
