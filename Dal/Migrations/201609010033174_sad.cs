namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sad : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Category", "Height", c => c.Int(nullable: false));
            AddColumn("dbo.Category", "Width", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Category", "Width");
            DropColumn("dbo.Category", "Height");
        }
    }
}
