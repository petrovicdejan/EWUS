namespace EWUS_Expertdatabase.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version12 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DocumentItems", "ProjectMeasure_Id", "dbo.ProjectMeasures");
            DropForeignKey("dbo.ProjectMeasurePerformances", "DocumentItem_Id", "dbo.DocumentItems");
            DropIndex("dbo.DocumentItems", new[] { "ProjectMeasure_Id" });
            AddColumn("dbo.ProjectMeasurePerformances", "Position", c => c.Int(nullable: false));
            AddColumn("dbo.ProjectMeasurePerformances", "Hide", c => c.Boolean(nullable: false));
            AddForeignKey("dbo.ProjectMeasurePerformances", "DocumentItem_Id", "dbo.DocumentItems", "Id", cascadeDelete: true);
            DropColumn("dbo.DocumentItems", "ProjectMeasure_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DocumentItems", "ProjectMeasure_Id", c => c.Long());
            DropForeignKey("dbo.ProjectMeasurePerformances", "DocumentItem_Id", "dbo.DocumentItems");
            DropColumn("dbo.ProjectMeasurePerformances", "Hide");
            DropColumn("dbo.ProjectMeasurePerformances", "Position");
            CreateIndex("dbo.DocumentItems", "ProjectMeasure_Id");
            AddForeignKey("dbo.ProjectMeasurePerformances", "DocumentItem_Id", "dbo.DocumentItems", "Id");
            AddForeignKey("dbo.DocumentItems", "ProjectMeasure_Id", "dbo.ProjectMeasures", "Id");
        }
    }
}
