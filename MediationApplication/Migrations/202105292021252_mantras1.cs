namespace MediationApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mantras1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Mantras",
                c => new
                    {
                        MantraID = c.Int(nullable: false, identity: true),
                        MantraName = c.String(),
                    })
                .PrimaryKey(t => t.MantraID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Mantras");
        }
    }
}
