namespace MediationApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class validation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Mantras", "MantraName", c => c.String(nullable: true));
            AlterColumn("dbo.JournalEntries", "Location", c => c.String(nullable: false));
            AlterColumn("dbo.JournalEntries", "MoodBefore", c => c.String(nullable: false));
            AlterColumn("dbo.JournalEntries", "MoodAfter", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JournalEntries", "MoodAfter", c => c.String());
            AlterColumn("dbo.JournalEntries", "MoodBefore", c => c.String());
            AlterColumn("dbo.JournalEntries", "Location", c => c.String());
            AlterColumn("dbo.Mantras", "MantraName", c => c.String());
        }
    }
}
