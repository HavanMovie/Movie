namespace Grp8MovieShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JunctionTableConceptupd2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "Department_Id", "dbo.Departments");
            DropIndex("dbo.Employees", new[] { "Department_Id" });
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
            
            DropColumn("dbo.Employees", "Department_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "Department_Id", c => c.Int());
            DropForeignKey("dbo.EmployeeDepartments", "Department_Id", "dbo.Departments");
            DropForeignKey("dbo.EmployeeDepartments", "Employee_Id", "dbo.Employees");
            DropIndex("dbo.EmployeeDepartments", new[] { "Department_Id" });
            DropIndex("dbo.EmployeeDepartments", new[] { "Employee_Id" });
            DropTable("dbo.EmployeeDepartments");
            CreateIndex("dbo.Employees", "Department_Id");
            AddForeignKey("dbo.Employees", "Department_Id", "dbo.Departments", "Id");
        }
    }
}
