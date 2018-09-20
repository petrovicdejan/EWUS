namespace EWUS_Expertdatabase.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version14 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectMeasurePerformances", "DocumentItem_Id", "dbo.DocumentItems");
            AddForeignKey("dbo.ProjectMeasurePerformances", "DocumentItem_Id", "dbo.DocumentItems", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectMeasurePerformances", "DocumentItem_Id", "dbo.DocumentItems");
            AddForeignKey("dbo.ProjectMeasurePerformances", "DocumentItem_Id", "dbo.DocumentItems", "Id", cascadeDelete: true);
        }
    }
}
