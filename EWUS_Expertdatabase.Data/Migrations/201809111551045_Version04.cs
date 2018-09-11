namespace EWUS_Expertdatabase.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version04 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "Location", c => c.String(maxLength: 255));
            AlterColumn("dbo.Projects", "ZipCode", c => c.String(maxLength: 10));
            AlterColumn("dbo.Projects", "City", c => c.String(maxLength: 255));
            AlterColumn("dbo.Projects", "PropertyNumber", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "PropertyNumber", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Projects", "City", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Projects", "ZipCode", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Projects", "Location", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
