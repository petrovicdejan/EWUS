namespace EWUS_Expertdatabase.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "DocumentItemId", c => c.Int());
            AddColumn("dbo.Projects", "DocumentItem_Id", c => c.Long());
            CreateIndex("dbo.Projects", "DocumentItem_Id");
            AddForeignKey("dbo.Projects", "DocumentItem_Id", "dbo.DocumentItems", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "DocumentItem_Id", "dbo.DocumentItems");
            DropIndex("dbo.Projects", new[] { "DocumentItem_Id" });
            DropColumn("dbo.Projects", "DocumentItem_Id");
            DropColumn("dbo.Projects", "DocumentItemId");
        }
    }
}
