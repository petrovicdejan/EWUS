namespace EWUS_Expertdatabase.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version14 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectMeasurePerformances", "DocumentItem_Id", "dbo.DocumentItems");
            AddColumn("dbo.Measures", "SavingPercent", c => c.Int(nullable: false));
            AddColumn("dbo.Projects", "DocumentItemId", c => c.Int());
            AddColumn("dbo.Projects", "DocumentItem_Id", c => c.Long());
            CreateIndex("dbo.Projects", "DocumentItem_Id");
            AddForeignKey("dbo.Projects", "DocumentItem_Id", "dbo.DocumentItems", "Id");
            AddForeignKey("dbo.ProjectMeasurePerformances", "DocumentItem_Id", "dbo.DocumentItems", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectMeasurePerformances", "DocumentItem_Id", "dbo.DocumentItems");
            DropForeignKey("dbo.Projects", "DocumentItem_Id", "dbo.DocumentItems");
            DropIndex("dbo.Projects", new[] { "DocumentItem_Id" });
            DropColumn("dbo.Projects", "DocumentItem_Id");
            DropColumn("dbo.Projects", "DocumentItemId");
            DropColumn("dbo.Measures", "SavingPercent");
            AddForeignKey("dbo.ProjectMeasurePerformances", "DocumentItem_Id", "dbo.DocumentItems", "Id", cascadeDelete: true);
        }
    }
}
