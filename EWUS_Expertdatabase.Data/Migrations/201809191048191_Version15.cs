namespace EWUS_Expertdatabase.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Measures", "SavingPercent", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Measures", "SavingPercent");
        }
    }
}
