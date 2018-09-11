namespace EWUS_Expertdatabase.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version03 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Classifications", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Classifications", "ClassificationType", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Classifications", "Value", c => c.String(nullable: false));
            AlterColumn("dbo.InvolvedParties", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.InvolvedParties", "Email", c => c.String(maxLength: 50));
            AlterColumn("dbo.InvolvedParties", "Logo", c => c.String(maxLength: 255));
            AlterColumn("dbo.DocumentItems", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.DocumentInstances", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.MeasureLinks", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.Measures", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.MeasurePictures", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.Projects", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.Projects", "ServicedObject", c => c.String(maxLength: 255));
            AlterColumn("dbo.Projects", "Location", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Projects", "ZipCode", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Projects", "City", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Projects", "Telephone", c => c.String(maxLength: 50));
            AlterColumn("dbo.Projects", "ContactPerson", c => c.String(maxLength: 50));
            AlterColumn("dbo.Projects", "PropertyNumber", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "PropertyNumber", c => c.String());
            AlterColumn("dbo.Projects", "ContactPerson", c => c.String());
            AlterColumn("dbo.Projects", "Telephone", c => c.String());
            AlterColumn("dbo.Projects", "City", c => c.String());
            AlterColumn("dbo.Projects", "ZipCode", c => c.String());
            AlterColumn("dbo.Projects", "Location", c => c.String());
            AlterColumn("dbo.Projects", "ServicedObject", c => c.String());
            AlterColumn("dbo.Projects", "Name", c => c.String());
            AlterColumn("dbo.MeasurePictures", "Name", c => c.String());
            AlterColumn("dbo.Measures", "Name", c => c.String());
            AlterColumn("dbo.MeasureLinks", "Name", c => c.String());
            AlterColumn("dbo.DocumentInstances", "Name", c => c.String());
            AlterColumn("dbo.DocumentItems", "Name", c => c.String());
            AlterColumn("dbo.InvolvedParties", "Logo", c => c.String());
            AlterColumn("dbo.InvolvedParties", "Email", c => c.String());
            AlterColumn("dbo.InvolvedParties", "Name", c => c.String());
            AlterColumn("dbo.Classifications", "Value", c => c.String());
            AlterColumn("dbo.Classifications", "ClassificationType", c => c.String());
            AlterColumn("dbo.Classifications", "Name", c => c.String());
        }
    }
}
