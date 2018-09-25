namespace EWUS_Expertdatabase.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectMeasurePerformances", "PageBreak", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectMeasurePerformances", "PageBreak");
        }
    }
}
