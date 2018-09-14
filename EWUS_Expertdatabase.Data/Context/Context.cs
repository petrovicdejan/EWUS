using EWUS_Expertdatabase.Model;
using System.Data.Entity;

namespace EWUS_Expertdatabase.Data
{
    public class EWUSDbContext : DbContext
    {
        public EWUSDbContext() : base("EWUS_Expertdatabase")
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Classification> Classifications { get; set; }
        public DbSet<InvolvedParty> InvolvedPartys { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<MaintenanceCompany> MaintenanceCompanies { get; set; }
        public DbSet<Individual> Individuals { get; set; }
        public DbSet<Measure> Measures { get; set; }
        public DbSet<MeasurePicture> MeasurePictures { get; set; }
        public DbSet<MeasureLink> MeasureLinks { get; set; }
        public DbSet<DocumentInstance> DocumentInstances { get; set; }
        public DbSet<DocumentItem> DocumentItems { get; set; }
        //public DbSet<ProjectMeasure> ProjectMeasures { get; set; }
        //public DbSet<ProjectMeasurePerformance> ProjectMeasurePerformances { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.ComplexType<Reference>();

            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<MaintenanceCompany>().ToTable("MaintenanceCompanies");
            modelBuilder.Entity<Individual>().ToTable("Individuals");

            //modelBuilder.Entity<ProjectMeasure>().HasKey(pm => new { pm.ProjectId, pm.MeasureId });

            //modelBuilder.Entity<InvolvedParty>().Property(x => x.Name).IsRequired();
            //modelBuilder.Entity<Measure>().Property(x => x.Name).IsRequired();
            //modelBuilder.Entity<Measure>().Property(x => x.SerialNumber).IsRequired();
            //modelBuilder.Entity<Project>().Property(x => x.PropertyNumber).IsRequired();
            //modelBuilder.Entity<Project>().Property(x => x.Location).IsRequired();
            //modelBuilder.Entity<Project>().Property(x => x.ZipCode).IsRequired();
            //modelBuilder.Entity<Project>().Property(x => x.City).IsRequired();
            //modelBuilder.Entity<Classification>().Property(x => x.Name).IsRequired();
            //modelBuilder.Entity<Classification>().Property(x => x.ClassificationType).IsRequired();
            //modelBuilder.Entity<Classification>().Property(x => x.Value).IsRequired();
        }
    }
}
