namespace EWUS_Expertdatabase.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version09 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectMeasures",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        ProjectId = c.Long(nullable: false),
                        MeasureId = c.Long(nullable: false),
                        MaintenanceCompanyId = c.Int(),
                        PerformanseSheetStatusId = c.Int(),
                        PerformanseSheetNumber = c.Int(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        Specification = c.String(),
                        InvestmenCost = c.Int(nullable: false),
                        SubmittedOnDate = c.DateTime(nullable: false),
                        SubmittedBy = c.String(),
                        Release = c.Boolean(nullable: false),
                        Remark = c.String(),
                        Description = c.String(),
                        Guid = c.Guid(nullable: false, identity: true),
                        MaintenanceCompany_Id = c.Long(),
                        PerformanseSheetStatus_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MaintenanceCompanies", t => t.MaintenanceCompany_Id)
                .ForeignKey("dbo.Measures", t => t.MeasureId)
                .ForeignKey("dbo.Classifications", t => t.PerformanseSheetStatus_Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .Index(t => t.ProjectId)
                .Index(t => t.MeasureId)
                .Index(t => t.MaintenanceCompany_Id)
                .Index(t => t.PerformanseSheetStatus_Id);
            
            CreateTable(
                "dbo.ProjectMeasurePerformances",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        ProjectMeasureId = c.Int(nullable: false),
                        DocumentItemId = c.Int(),
                        Description = c.String(),
                        Guid = c.Guid(nullable: false, identity: true),
                        DocumentItem_Id = c.Long(),
                        ProjectMeasure_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocumentItems", t => t.DocumentItem_Id)
                .ForeignKey("dbo.ProjectMeasures", t => t.ProjectMeasure_Id)
                .Index(t => t.DocumentItem_Id)
                .Index(t => t.ProjectMeasure_Id);
            
            AddColumn("dbo.DocumentItems", "Position", c => c.Int(nullable: false));
            AddColumn("dbo.DocumentItems", "Hide", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Classifications", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Classifications", "ClassificationType", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Classifications", "Value", c => c.String(nullable: false));
            AlterColumn("dbo.InvolvedParties", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.InvolvedParties", "Email", c => c.String(maxLength: 50));
            AlterColumn("dbo.Measures", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Projects", "Location", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Projects", "ZipCode", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Projects", "City", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Projects", "Telephone", c => c.String(maxLength: 50));
            AlterColumn("dbo.Projects", "ContactPerson", c => c.String(maxLength: 50));
            AlterColumn("dbo.Projects", "PropertyNumber", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectMeasurePerformances", "ProjectMeasure_Id", "dbo.ProjectMeasures");
            DropForeignKey("dbo.ProjectMeasurePerformances", "DocumentItem_Id", "dbo.DocumentItems");
            DropForeignKey("dbo.ProjectMeasures", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectMeasures", "PerformanseSheetStatus_Id", "dbo.Classifications");
            DropForeignKey("dbo.ProjectMeasures", "MeasureId", "dbo.Measures");
            DropForeignKey("dbo.ProjectMeasures", "MaintenanceCompany_Id", "dbo.MaintenanceCompanies");
            DropIndex("dbo.ProjectMeasurePerformances", new[] { "ProjectMeasure_Id" });
            DropIndex("dbo.ProjectMeasurePerformances", new[] { "DocumentItem_Id" });
            DropIndex("dbo.ProjectMeasures", new[] { "PerformanseSheetStatus_Id" });
            DropIndex("dbo.ProjectMeasures", new[] { "MaintenanceCompany_Id" });
            DropIndex("dbo.ProjectMeasures", new[] { "MeasureId" });
            DropIndex("dbo.ProjectMeasures", new[] { "ProjectId" });
            AlterColumn("dbo.Projects", "PropertyNumber", c => c.String());
            AlterColumn("dbo.Projects", "ContactPerson", c => c.String());
            AlterColumn("dbo.Projects", "Telephone", c => c.String());
            AlterColumn("dbo.Projects", "City", c => c.String(maxLength: 255));
            AlterColumn("dbo.Projects", "ZipCode", c => c.String());
            AlterColumn("dbo.Projects", "Location", c => c.String(maxLength: 255));
            AlterColumn("dbo.Measures", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.InvolvedParties", "Email", c => c.String());
            AlterColumn("dbo.InvolvedParties", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.Classifications", "Value", c => c.String());
            AlterColumn("dbo.Classifications", "ClassificationType", c => c.String(maxLength: 255));
            AlterColumn("dbo.Classifications", "Name", c => c.String(maxLength: 255));
            DropColumn("dbo.DocumentItems", "Hide");
            DropColumn("dbo.DocumentItems", "Position");
            DropTable("dbo.ProjectMeasurePerformances");
            DropTable("dbo.ProjectMeasures");
        }
    }
}
