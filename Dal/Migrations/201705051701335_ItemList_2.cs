namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemList_2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemList", "Content", c => c.String());
            DropColumn("dbo.ItemList", "Url");
            DropColumn("dbo.NewsLetter", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NewsLetter", "Name", c => c.String());
            AddColumn("dbo.ItemList", "Url", c => c.String());
            DropColumn("dbo.ItemList", "Content");
        }
    }
}
