namespace MediationApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mantrascategories : DbMigration
    {
        public override void Up()
        {
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
            DropForeignKey("dbo.MantraCategories", "Category_CategoryID", "dbo.Categories");
            DropForeignKey("dbo.MantraCategories", "Mantra_MantraID", "dbo.Mantras");
            DropIndex("dbo.MantraCategories", new[] { "Category_CategoryID" });
            DropIndex("dbo.MantraCategories", new[] { "Mantra_MantraID" });
            DropTable("dbo.MantraCategories");
        }
    }
}
