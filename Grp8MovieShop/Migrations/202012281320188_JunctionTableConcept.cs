namespace Grp8MovieShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JunctionTableConcept : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmployeeDepartments",
                c => new
                    {
                        Employee_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Employee_Id, t.Department_Id })
                .ForeignKey("dbo.Employees", t => t.Employee_Id, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.Employee_Id)
                .Index(t => t.Department_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeeDepartments", "Department_Id", "dbo.Departments");
            DropForeignKey("dbo.EmployeeDepartments", "Employee_Id", "dbo.Employees");
            DropIndex("dbo.EmployeeDepartments", new[] { "Department_Id" });
            DropIndex("dbo.EmployeeDepartments", new[] { "Employee_Id" });
            DropTable("dbo.EmployeeDepartments");
            DropTable("dbo.Employees");
            DropTable("dbo.Departments");
        }
    }
}
