namespace MediationApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mantra_category_validation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categories", "CategoryName", c => c.String(nullable: false));
            AlterColumn("dbo.Categories", "CategoryDescription", c => c.String(nullable: false));
            AlterColumn("dbo.Mantras", "MantraName", c => c.String(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Mantras", "MantraName", c => c.String());
            AlterColumn("dbo.Categories", "CategoryDescription", c => c.String());
            AlterColumn("dbo.Categories", "CategoryName", c => c.String());
        }
    }
}
