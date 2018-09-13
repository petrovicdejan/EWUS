namespace EWUS_Expertdatabase.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version08 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Classifications", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.Classifications", "ClassificationType", c => c.String(maxLength: 255));
            AlterColumn("dbo.Classifications", "Value", c => c.String());
            AlterColumn("dbo.InvolvedParties", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.InvolvedParties", "Email", c => c.String());
            AlterColumn("dbo.Measures", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.Projects", "Location", c => c.String(maxLength: 255));
            AlterColumn("dbo.Projects", "ZipCode", c => c.String());
            AlterColumn("dbo.Projects", "City", c => c.String(maxLength: 255));
            AlterColumn("dbo.Projects", "Telephone", c => c.String());
            AlterColumn("dbo.Projects", "ContactPerson", c => c.String());
            AlterColumn("dbo.Projects", "PropertyNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "PropertyNumber", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Projects", "ContactPerson", c => c.String(maxLength: 50));
            AlterColumn("dbo.Projects", "Telephone", c => c.String(maxLength: 50));
            AlterColumn("dbo.Projects", "City", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Projects", "ZipCode", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Projects", "Location", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Measures", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.InvolvedParties", "Email", c => c.String(maxLength: 50));
            AlterColumn("dbo.InvolvedParties", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Classifications", "Value", c => c.String(nullable: false));
            AlterColumn("dbo.Classifications", "ClassificationType", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Classifications", "Name", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
