namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSlider : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Slider", "NewsId", "dbo.News");
            DropIndex("dbo.Slider", new[] { "NewsId" });
            CreateTable(
                "dbo.Slide",
                c => new
                    {
                        SlideId = c.Int(nullable: false, identity: true),
                        NewsId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        SliderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SlideId)
                .ForeignKey("dbo.News", t => t.NewsId)
                .ForeignKey("dbo.Slider", t => t.SliderId)
                .Index(t => t.NewsId)
                .Index(t => t.SliderId);
            
            AddColumn("dbo.Slider", "sGuid", c => c.String());
            AddColumn("dbo.Slider", "Name", c => c.String());
            DropColumn("dbo.Slider", "NewsId");
            DropColumn("dbo.Slider", "Order");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Slider", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.Slider", "NewsId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Slide", "SliderId", "dbo.Slider");
            DropForeignKey("dbo.Slide", "NewsId", "dbo.News");
            DropIndex("dbo.Slide", new[] { "SliderId" });
            DropIndex("dbo.Slide", new[] { "NewsId" });
            DropColumn("dbo.Slider", "Name");
            DropColumn("dbo.Slider", "sGuid");
            DropTable("dbo.Slide");
            CreateIndex("dbo.Slider", "NewsId");
            AddForeignKey("dbo.Slider", "NewsId", "dbo.News", "NewsId");
        }
    }
}
