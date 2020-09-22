namespace MasterClassAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stu_id : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Attends", "Stu_Id", "dbo.Students");
            DropPrimaryKey("dbo.Students");
            DropColumn("dbo.Students", "Student_Id");
            AddColumn("dbo.Students", "Stu_Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Students", "Stu_Id");
            AddForeignKey("dbo.Attends", "Stu_Id", "dbo.Students", "Stu_Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "Student_Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Attends", "Stu_Id", "dbo.Students");
            DropPrimaryKey("dbo.Students");
            DropColumn("dbo.Students", "Stu_Id");
            AddPrimaryKey("dbo.Students", "Student_Id");
            AddForeignKey("dbo.Attends", "Stu_Id", "dbo.Students", "Student_Id", cascadeDelete: true);
        }
    }
}
