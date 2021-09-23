namespace Grp8MovieShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustUpd : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "EmailAddress", c => c.String(nullable: false, maxLength: 256));
            CreateIndex("dbo.Customers", "EmailAddress", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Customers", new[] { "EmailAddress" });
            AlterColumn("dbo.Customers", "EmailAddress", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
