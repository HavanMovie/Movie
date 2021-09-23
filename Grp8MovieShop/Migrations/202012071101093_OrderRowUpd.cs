namespace Grp8MovieShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderRowUpd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderRows", "NoofCopies", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderRows", "NoofCopies");
        }
    }
}
