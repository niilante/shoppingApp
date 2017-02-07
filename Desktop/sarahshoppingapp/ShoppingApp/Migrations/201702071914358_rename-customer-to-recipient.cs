namespace ShoppingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renamecustomertorecipient : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Customers", newName: "Recipients");
            AddColumn("dbo.Orders", "Recipient_Id", c => c.Int());
            CreateIndex("dbo.Orders", "Recipient_Id");
            AddForeignKey("dbo.Orders", "Recipient_Id", "dbo.Recipients", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "Recipient_Id", "dbo.Recipients");
            DropIndex("dbo.Orders", new[] { "Recipient_Id" });
            DropColumn("dbo.Orders", "Recipient_Id");
            RenameTable(name: "dbo.Recipients", newName: "Customers");
        }
    }
}
