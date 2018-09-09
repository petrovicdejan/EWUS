namespace EWUS_Expertdatabase.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classifications",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        ClassificationType = c.String(),
                        Value = c.String(),
                        Description = c.String(),
                        Guid = c.Guid(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InvolvedParties",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        RootType = c.String(),
                        Email = c.String(),
                        Logo = c.String(),
                        Description = c.String(),
                        Guid = c.Guid(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DocumentInstances",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        RefersTo_Id = c.Long(nullable: false),
                        RefersTo_TypeName = c.String(),
                        Description = c.String(),
                        Guid = c.Guid(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DocumentItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        DocumentInstanceId = c.Long(nullable: false),
                        BinaryValue = c.Binary(),
                        DocumentName = c.String(),
                        DocumentMimeType = c.String(),
                        DocumentSize = c.String(),
                        ObjectId = c.String(),
                        Description = c.String(),
                        Guid = c.Guid(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocumentInstances", t => t.DocumentInstanceId, cascadeDelete: true)
                .Index(t => t.DocumentInstanceId);
            
            CreateTable(
                "dbo.MeasureLinks",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Link = c.String(),
                        MeasureId = c.Long(nullable: false),
                        Description = c.String(),
                        Guid = c.Guid(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Measures", t => t.MeasureId, cascadeDelete: true)
                .Index(t => t.MeasureId);
            
            CreateTable(
                "dbo.Measures",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        SerialNumber = c.Int(nullable: false),
                        OperationTypeId = c.Int(),
                        Saving = c.String(),
                        InvestmentCost = c.Int(nullable: false),
                        Description = c.String(),
                        Guid = c.Guid(nullable: false, identity: true),
                        OperationType_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Classifications", t => t.OperationType_Id)
                .Index(t => t.OperationType_Id);
            
            CreateTable(
                "dbo.MeasurePictures",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Filename = c.String(),
                        MeasureId = c.Long(nullable: false),
                        Description = c.String(),
                        Guid = c.Guid(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Measures", t => t.MeasureId, cascadeDelete: true)
                .Index(t => t.MeasureId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        PerformanceOfEquipment = c.Int(nullable: false),
                        ServicedObject = c.String(),
                        InvestmentTotal = c.Int(nullable: false),
                        SavingTotal = c.Int(nullable: false),
                        Remark = c.String(),
                        ChangesOfUsage = c.String(),
                        Location = c.String(),
                        ZipCode = c.String(),
                        City = c.String(),
                        Telephone = c.String(),
                        ContactPerson = c.String(),
                        Description = c.String(),
                        Guid = c.Guid(nullable: false, identity: true),
                        Customer_Id = c.Long(),
                        Property_Id = c.Long(),
                        Region_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.Classifications", t => t.Property_Id)
                .ForeignKey("dbo.Classifications", t => t.Region_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.Property_Id)
                .Index(t => t.Region_Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InvolvedParties", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Individuals",
                c => new
                    {
                        Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InvolvedParties", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.MaintenanceCompanies",
                c => new
                    {
                        Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InvolvedParties", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MaintenanceCompanies", "Id", "dbo.InvolvedParties");
            DropForeignKey("dbo.Individuals", "Id", "dbo.InvolvedParties");
            DropForeignKey("dbo.Customers", "Id", "dbo.InvolvedParties");
            DropForeignKey("dbo.Projects", "Region_Id", "dbo.Classifications");
            DropForeignKey("dbo.Projects", "Property_Id", "dbo.Classifications");
            DropForeignKey("dbo.Projects", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Measures", "OperationType_Id", "dbo.Classifications");
            DropForeignKey("dbo.MeasurePictures", "MeasureId", "dbo.Measures");
            DropForeignKey("dbo.MeasureLinks", "MeasureId", "dbo.Measures");
            DropForeignKey("dbo.DocumentItems", "DocumentInstanceId", "dbo.DocumentInstances");
            DropIndex("dbo.MaintenanceCompanies", new[] { "Id" });
            DropIndex("dbo.Individuals", new[] { "Id" });
            DropIndex("dbo.Customers", new[] { "Id" });
            DropIndex("dbo.Projects", new[] { "Region_Id" });
            DropIndex("dbo.Projects", new[] { "Property_Id" });
            DropIndex("dbo.Projects", new[] { "Customer_Id" });
            DropIndex("dbo.MeasurePictures", new[] { "MeasureId" });
            DropIndex("dbo.Measures", new[] { "OperationType_Id" });
            DropIndex("dbo.MeasureLinks", new[] { "MeasureId" });
            DropIndex("dbo.DocumentItems", new[] { "DocumentInstanceId" });
            DropTable("dbo.MaintenanceCompanies");
            DropTable("dbo.Individuals");
            DropTable("dbo.Customers");
            DropTable("dbo.Projects");
            DropTable("dbo.MeasurePictures");
            DropTable("dbo.Measures");
            DropTable("dbo.MeasureLinks");
            DropTable("dbo.DocumentItems");
            DropTable("dbo.DocumentInstances");
            DropTable("dbo.InvolvedParties");
            DropTable("dbo.Classifications");
        }
    }
}
