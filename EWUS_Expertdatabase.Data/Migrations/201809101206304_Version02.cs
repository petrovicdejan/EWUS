namespace EWUS_Expertdatabase.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version02 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DocumentItems", "DocumentInstanceId", "dbo.DocumentInstances");
            DropIndex("dbo.DocumentItems", new[] { "DocumentInstanceId" });
            RenameColumn(table: "dbo.DocumentItems", name: "DocumentInstanceId", newName: "DocumentInstance_Id");
            AlterColumn("dbo.DocumentItems", "DocumentInstance_Id", c => c.Long());
            CreateIndex("dbo.DocumentItems", "DocumentInstance_Id");
            AddForeignKey("dbo.DocumentItems", "DocumentInstance_Id", "dbo.DocumentInstances", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocumentItems", "DocumentInstance_Id", "dbo.DocumentInstances");
            DropIndex("dbo.DocumentItems", new[] { "DocumentInstance_Id" });
            AlterColumn("dbo.DocumentItems", "DocumentInstance_Id", c => c.Long(nullable: false));
            RenameColumn(table: "dbo.DocumentItems", name: "DocumentInstance_Id", newName: "DocumentInstanceId");
            CreateIndex("dbo.DocumentItems", "DocumentInstanceId");
            AddForeignKey("dbo.DocumentItems", "DocumentInstanceId", "dbo.DocumentInstances", "Id", cascadeDelete: true);
        }
    }
}
