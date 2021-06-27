namespace MediationApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        CategoryDescription = c.String(),
                        CategoryColour = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Mantras",
                c => new
                    {
                        MantraID = c.Int(nullable: false, identity: true),
                        MantraName = c.String(),
                    })
                .PrimaryKey(t => t.MantraID);
            
            CreateTable(
                "dbo.JournalEntries",
                c => new
                    {
                        JournalEntryID = c.Int(nullable: false, identity: true),
                        Location = c.String(),
                        MoodBefore = c.String(),
                        MoodAfter = c.String(),
                        Thoughts = c.String(),
                        SessionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JournalEntryID)
                .ForeignKey("dbo.MeditationSessions", t => t.SessionID, cascadeDelete: true)
                .Index(t => t.SessionID);
            
            CreateTable(
                "dbo.MeditationSessions",
                c => new
                    {
                        SessionID = c.Int(nullable: false, identity: true),
                        SessionDate = c.DateTime(nullable: false),
                        SessionStartTime = c.DateTime(nullable: false),
                        SessionEndTime = c.DateTime(nullable: false),
                        SessionDuration = c.Int(nullable: false),
                        MantraID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SessionID)
                .ForeignKey("dbo.Mantras", t => t.MantraID, cascadeDelete: true)
                .Index(t => t.MantraID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
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
                "dbo.MantraCategories",
                c => new
                    {
                        Mantra_MantraID = c.Int(nullable: false),
                        Category_CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Mantra_MantraID, t.Category_CategoryID })
                .ForeignKey("dbo.Mantras", t => t.Mantra_MantraID, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_CategoryID, cascadeDelete: true)
                .Index(t => t.Mantra_MantraID)
                .Index(t => t.Category_CategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.JournalEntries", "SessionID", "dbo.MeditationSessions");
            DropForeignKey("dbo.MeditationSessions", "MantraID", "dbo.Mantras");
            DropForeignKey("dbo.MantraCategories", "Category_CategoryID", "dbo.Categories");
            DropForeignKey("dbo.MantraCategories", "Mantra_MantraID", "dbo.Mantras");
            DropIndex("dbo.MantraCategories", new[] { "Category_CategoryID" });
            DropIndex("dbo.MantraCategories", new[] { "Mantra_MantraID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.MeditationSessions", new[] { "MantraID" });
            DropIndex("dbo.JournalEntries", new[] { "SessionID" });
            DropTable("dbo.MantraCategories");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.MeditationSessions");
            DropTable("dbo.JournalEntries");
            DropTable("dbo.Mantras");
            DropTable("dbo.Categories");
        }
    }
}
