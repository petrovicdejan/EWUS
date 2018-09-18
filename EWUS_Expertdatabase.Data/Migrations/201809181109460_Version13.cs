namespace EWUS_Expertdatabase.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version13 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ProjectMeasurePerformances", "ProjectMeasureId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectMeasurePerformances", "ProjectMeasureId", c => c.Int(nullable: false));
        }
    }
}
