namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class thumbnail : DbMigration
    {
        public override void Up()
        {
                       AddColumn("dbo.News", "Thumbnail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "Thumbnail");
        }
    }
}
