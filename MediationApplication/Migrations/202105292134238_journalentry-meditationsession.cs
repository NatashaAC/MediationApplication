namespace MediationApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class journalentrymeditationsession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JournalEntries", "SessionID", c => c.Int(nullable: false));
            CreateIndex("dbo.JournalEntries", "SessionID");
            AddForeignKey("dbo.JournalEntries", "SessionID", "dbo.MeditationSessions", "SessionID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JournalEntries", "SessionID", "dbo.MeditationSessions");
            DropIndex("dbo.JournalEntries", new[] { "SessionID" });
            DropColumn("dbo.JournalEntries", "SessionID");
        }
    }
}
