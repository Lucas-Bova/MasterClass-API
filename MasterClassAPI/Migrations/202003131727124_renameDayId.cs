namespace MasterClassAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renameDayId : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Days", name: "Day_Id", newName: "Att_Id");
            RenameIndex(table: "dbo.Days", name: "IX_Day_Id", newName: "IX_Att_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Days", name: "IX_Att_Id", newName: "IX_Day_Id");
            RenameColumn(table: "dbo.Days", name: "Att_Id", newName: "Day_Id");
        }
    }
}
