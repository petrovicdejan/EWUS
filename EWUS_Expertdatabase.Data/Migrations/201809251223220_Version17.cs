namespace EWUS_Expertdatabase.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version17 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DocumentItems", "DocumentInstance_Id", "dbo.DocumentInstances");
            DropForeignKey("dbo.MeasurePictures", "MeasureId", "dbo.Measures");
            DropIndex("dbo.DocumentItems", new[] { "DocumentInstance_Id" });
            DropIndex("dbo.MeasurePictures", new[] { "MeasureId" });
            DropColumn("dbo.DocumentItems", "BinaryValue");
            DropColumn("dbo.DocumentItems", "DocumentInstance_Id");
            DropTable("dbo.DocumentInstances");
            DropTable("dbo.MeasurePictures");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MeasurePictures",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        Filename = c.String(),
                        MeasureId = c.Long(nullable: false),
                        Description = c.String(),
                        Guid = c.Guid(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DocumentInstances",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        Description = c.String(),
                        Guid = c.Guid(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.DocumentItems", "DocumentInstance_Id", c => c.Long());
            AddColumn("dbo.DocumentItems", "BinaryValue", c => c.Binary());
            CreateIndex("dbo.MeasurePictures", "MeasureId");
            CreateIndex("dbo.DocumentItems", "DocumentInstance_Id");
            AddForeignKey("dbo.MeasurePictures", "MeasureId", "dbo.Measures", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DocumentItems", "DocumentInstance_Id", "dbo.DocumentInstances", "Id");
        }
    }
}
