namespace MasterClassAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classrooms",
                c => new
                    {
                        Cls_Id = c.Int(nullable: false, identity: true),
                        Cls_Name = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Cls_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Student_Id = c.Int(nullable: false, identity: true),
                        Stu_FName = c.String(),
                        Stu_LName = c.String(),
                        Stu_Pic = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Student_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Attends",
                c => new
                    {
                        Stu_Id = c.Int(nullable: false),
                        Cls_Id = c.Int(nullable: false),
                        Att_Id = c.Int(nullable: false, identity: true),
                        Att_Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Att_Id)
                .ForeignKey("dbo.Classrooms", t => t.Cls_Id, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Stu_Id, cascadeDelete: true)
                .Index(t => t.Stu_Id)
                .Index(t => t.Cls_Id);
            
            CreateTable(
                "dbo.Days",
                c => new
                    {
                        Day_Id = c.Int(nullable: false),
                        Day_Interactions = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Day_Id)
                .ForeignKey("dbo.Attends", t => t.Day_Id)
                .Index(t => t.Day_Id);
            
            CreateTable(
                "dbo.Histories",
                c => new
                    {
                        Hist_Id = c.Int(nullable: false, identity: true),
                        Att_Id = c.Int(nullable: false),
                        Hist_Date = c.DateTime(nullable: false),
                        Hist_Interactions = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Hist_Id)
                .ForeignKey("dbo.Attends", t => t.Att_Id, cascadeDelete: true)
                .Index(t => t.Att_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Classrooms", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Attends", "Stu_Id", "dbo.Students");
            DropForeignKey("dbo.Histories", "Att_Id", "dbo.Attends");
            DropForeignKey("dbo.Days", "Day_Id", "dbo.Attends");
            DropForeignKey("dbo.Attends", "Cls_Id", "dbo.Classrooms");
            DropForeignKey("dbo.Students", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Histories", new[] { "Att_Id" });
            DropIndex("dbo.Days", new[] { "Day_Id" });
            DropIndex("dbo.Attends", new[] { "Cls_Id" });
            DropIndex("dbo.Attends", new[] { "Stu_Id" });
            DropIndex("dbo.Students", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Classrooms", new[] { "User_Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Histories");
            DropTable("dbo.Days");
            DropTable("dbo.Attends");
            DropTable("dbo.Students");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Classrooms");
        }
    }
}
