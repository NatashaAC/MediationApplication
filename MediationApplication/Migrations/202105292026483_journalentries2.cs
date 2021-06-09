namespace MediationApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class journalentries2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JournalEntries",
                c => new
                    {
                        JournalEntryID = c.Int(nullable: false, identity: true),
                        Location = c.String(),
                        MoodBefore = c.String(),
                        MoodAfter = c.String(),
                        Thoughts = c.String(),
                    })
                .PrimaryKey(t => t.JournalEntryID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.JournalEntries");
        }
    }
}
