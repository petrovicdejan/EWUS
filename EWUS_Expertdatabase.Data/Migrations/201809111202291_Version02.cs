namespace EWUS_Expertdatabase.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version02 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentItems", "InvolvedParty_Id", c => c.Long());
            CreateIndex("dbo.DocumentItems", "InvolvedParty_Id");
            AddForeignKey("dbo.DocumentItems", "InvolvedParty_Id", "dbo.InvolvedParties", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocumentItems", "InvolvedParty_Id", "dbo.InvolvedParties");
            DropIndex("dbo.DocumentItems", new[] { "InvolvedParty_Id" });
            DropColumn("dbo.DocumentItems", "InvolvedParty_Id");
        }
    }
}
