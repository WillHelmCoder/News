namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryParent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Category", "showImage", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Category", "showImage");
        }
    }
}
