namespace MasterClassAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrequiredtostudents : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Students", "Stu_LName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "Stu_LName", c => c.String());
        }
    }
}
