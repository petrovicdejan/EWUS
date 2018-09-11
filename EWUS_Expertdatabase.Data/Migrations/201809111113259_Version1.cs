namespace EWUS_Expertdatabase.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "PropertyNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "PropertyNumber");
        }
    }
}
