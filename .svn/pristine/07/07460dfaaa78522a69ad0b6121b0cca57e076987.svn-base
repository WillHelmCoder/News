namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class keyPointContainer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.KeyPointsContainer", "KeyPointId", "dbo.KeyPoint");
            DropIndex("dbo.KeyPointsContainer", new[] { "KeyPointId" });
            AddColumn("dbo.KeyPoint", "KeyPointsContainerId", c => c.Int(nullable: false));
            AddColumn("dbo.KeyPoint", "SecondaryImage", c => c.String());
            AddColumn("dbo.KeyPoint", "MainImage", c => c.String());
            CreateIndex("dbo.KeyPoint", "KeyPointsContainerId");
            AddForeignKey("dbo.KeyPoint", "KeyPointsContainerId", "dbo.KeyPointsContainer", "KeyPointsContainerId");
            DropColumn("dbo.KeyPoint", "Image");
            DropColumn("dbo.KeyPoint", "imageSign");
            DropColumn("dbo.KeyPointsContainer", "KeyPointId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.KeyPointsContainer", "KeyPointId", c => c.Int(nullable: false));
            AddColumn("dbo.KeyPoint", "imageSign", c => c.String());
            AddColumn("dbo.KeyPoint", "Image", c => c.String());
            DropForeignKey("dbo.KeyPoint", "KeyPointsContainerId", "dbo.KeyPointsContainer");
            DropIndex("dbo.KeyPoint", new[] { "KeyPointsContainerId" });
            DropColumn("dbo.KeyPoint", "MainImage");
            DropColumn("dbo.KeyPoint", "SecondaryImage");
            DropColumn("dbo.KeyPoint", "KeyPointsContainerId");
            CreateIndex("dbo.KeyPointsContainer", "KeyPointId");
            AddForeignKey("dbo.KeyPointsContainer", "KeyPointId", "dbo.KeyPoint", "KeyPointId");
        }
    }
}
