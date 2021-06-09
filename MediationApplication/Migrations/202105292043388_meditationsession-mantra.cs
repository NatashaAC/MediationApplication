namespace MediationApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class meditationsessionmantra : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MeditationSessions", "MantraID", c => c.Int(nullable: false));
            CreateIndex("dbo.MeditationSessions", "MantraID");
            AddForeignKey("dbo.MeditationSessions", "MantraID", "dbo.Mantras", "MantraID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MeditationSessions", "MantraID", "dbo.Mantras");
            DropIndex("dbo.MeditationSessions", new[] { "MantraID" });
            DropColumn("dbo.MeditationSessions", "MantraID");
        }
    }
}
