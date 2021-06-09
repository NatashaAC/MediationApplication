namespace MediationApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class meditationsessions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MeditationSessions",
                c => new
                    {
                        SessionID = c.Int(nullable: false, identity: true),
                        SessionDate = c.DateTime(nullable: false),
                        SessionStartTime = c.DateTime(nullable: false),
                        SessionEndTime = c.DateTime(nullable: false),
                        SessionDuration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SessionID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MeditationSessions");
        }
    }
}
