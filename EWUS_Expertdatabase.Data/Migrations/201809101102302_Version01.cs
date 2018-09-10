namespace EWUS_Expertdatabase.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version01 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentItems", "Measure_Id", c => c.Long());
            CreateIndex("dbo.DocumentItems", "Measure_Id");
            AddForeignKey("dbo.DocumentItems", "Measure_Id", "dbo.Measures", "Id");
            DropColumn("dbo.DocumentInstances", "RefersTo_Id");
            DropColumn("dbo.DocumentInstances", "RefersTo_TypeName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DocumentInstances", "RefersTo_TypeName", c => c.String());
            AddColumn("dbo.DocumentInstances", "RefersTo_Id", c => c.Long(nullable: false));
            DropForeignKey("dbo.DocumentItems", "Measure_Id", "dbo.Measures");
            DropIndex("dbo.DocumentItems", new[] { "Measure_Id" });
            DropColumn("dbo.DocumentItems", "Measure_Id");
        }
    }
}
