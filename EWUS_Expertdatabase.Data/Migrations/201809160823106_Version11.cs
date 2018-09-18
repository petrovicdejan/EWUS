namespace EWUS_Expertdatabase.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentItems", "ProjectMeasure_Id", c => c.Long());
            CreateIndex("dbo.DocumentItems", "ProjectMeasure_Id");
            AddForeignKey("dbo.DocumentItems", "ProjectMeasure_Id", "dbo.ProjectMeasures", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocumentItems", "ProjectMeasure_Id", "dbo.ProjectMeasures");
            DropIndex("dbo.DocumentItems", new[] { "ProjectMeasure_Id" });
            DropColumn("dbo.DocumentItems", "ProjectMeasure_Id");
        }
    }
}
