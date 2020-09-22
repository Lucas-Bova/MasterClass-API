namespace MasterClassAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeUserIdRequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Classrooms", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Classrooms", new[] { "User_Id" });
            AlterColumn("dbo.Classrooms", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Classrooms", "User_Id");
            AddForeignKey("dbo.Classrooms", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Classrooms", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Classrooms", new[] { "User_Id" });
            AlterColumn("dbo.Classrooms", "User_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Classrooms", "User_Id");
            AddForeignKey("dbo.Classrooms", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
