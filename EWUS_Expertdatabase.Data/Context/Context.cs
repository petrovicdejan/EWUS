using EWUS_Expertdatabase.Model;
using System.Data.Entity;

namespace EWUS_Expertdatabase.Data
{
    public class EWUSDbContext : DbContext
    {
        public EWUSDbContext() : base("EWUS_Expertdatabase")
        {
            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<EWUSDbContext, Migrations.Configuration>());
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.ComplexType<Reference>();

            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<MaintenanceCompany>().ToTable("MaintenanceCompanies");
            modelBuilder.Entity<Individual>().ToTable("Individuals");
        }
    }
}
