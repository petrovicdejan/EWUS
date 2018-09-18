namespace EWUS_Expertdatabase.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version10 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProjectMeasures", "ModificationDate", c => c.DateTime());
            AlterColumn("dbo.ProjectMeasures", "InvestmenCost", c => c.Int());
            AlterColumn("dbo.ProjectMeasures", "SubmittedOnDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProjectMeasures", "SubmittedOnDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ProjectMeasures", "InvestmenCost", c => c.Int(nullable: false));
            AlterColumn("dbo.ProjectMeasures", "ModificationDate", c => c.DateTime(nullable: false));
        }
    }
}
