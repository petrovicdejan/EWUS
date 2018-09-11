namespace EWUS_Expertdatabase.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version06 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "ZipCode", c => c.String());
            AlterColumn("dbo.Projects", "Telephone", c => c.String());
            AlterColumn("dbo.Projects", "ContactPerson", c => c.String());
            AlterColumn("dbo.Projects", "PropertyNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "PropertyNumber", c => c.String(maxLength: 10));
            AlterColumn("dbo.Projects", "ContactPerson", c => c.String(maxLength: 50));
            AlterColumn("dbo.Projects", "Telephone", c => c.String(maxLength: 50));
            AlterColumn("dbo.Projects", "ZipCode", c => c.String(maxLength: 10));
        }
    }
}
