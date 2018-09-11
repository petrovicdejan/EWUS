namespace EWUS_Expertdatabase.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version05 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Classifications", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.Classifications", "ClassificationType", c => c.String(maxLength: 255));
            AlterColumn("dbo.Classifications", "Value", c => c.String());
            AlterColumn("dbo.InvolvedParties", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.Measures", "Name", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Measures", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.InvolvedParties", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Classifications", "Value", c => c.String(nullable: false));
            AlterColumn("dbo.Classifications", "ClassificationType", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Classifications", "Name", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
