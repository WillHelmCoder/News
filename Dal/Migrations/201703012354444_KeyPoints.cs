namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class KeyPoints : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Data", "MagazineId", "dbo.Magazine");
            DropIndex("dbo.Data", new[] { "MagazineId" });
            CreateTable(
                "dbo.KeyPoint",
                c => new
                    {
                        KeyPointId = c.Int(nullable: false, identity: true),
                        DataParentId = c.Int(),
                        Title = c.String(),
                        Description = c.String(),
                        Image = c.String(),
                        imageSign = c.String(),
                        Url = c.String(),
                        Order = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        MagazineId = c.Int(),
                    })
                .PrimaryKey(t => t.KeyPointId)
                .ForeignKey("dbo.Magazine", t => t.MagazineId)
                .Index(t => t.MagazineId);
            
            CreateTable(
                "dbo.KeyPointsContainer",
                c => new
                    {
                        KeyPointsContainerId = c.Int(nullable: false, identity: true),
                        KeyPointId = c.Int(nullable: false),
                        MagazineId = c.Int(nullable: false),
                        Name = c.String(),
                        Kguid = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.KeyPointsContainerId)
                .ForeignKey("dbo.KeyPoint", t => t.KeyPointId)
                .ForeignKey("dbo.Magazine", t => t.MagazineId)
                .Index(t => t.KeyPointId)
                .Index(t => t.MagazineId);
            
            DropTable("dbo.Data");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Data",
                c => new
                    {
                        DataId = c.Int(nullable: false, identity: true),
                        DataParentId = c.Int(),
                        Title = c.String(),
                        Description = c.String(),
                        Image = c.String(),
                        imageSign = c.String(),
                        Url = c.String(),
                        Order = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        MagazineId = c.Int(),
                    })
                .PrimaryKey(t => t.DataId);
            
            DropForeignKey("dbo.KeyPointsContainer", "MagazineId", "dbo.Magazine");
            DropForeignKey("dbo.KeyPointsContainer", "KeyPointId", "dbo.KeyPoint");
            DropForeignKey("dbo.KeyPoint", "MagazineId", "dbo.Magazine");
            DropIndex("dbo.KeyPointsContainer", new[] { "MagazineId" });
            DropIndex("dbo.KeyPointsContainer", new[] { "KeyPointId" });
            DropIndex("dbo.KeyPoint", new[] { "MagazineId" });
            DropTable("dbo.KeyPointsContainer");
            DropTable("dbo.KeyPoint");
            CreateIndex("dbo.Data", "MagazineId");
            AddForeignKey("dbo.Data", "MagazineId", "dbo.Magazine", "MagazineId");
        }
    }
}
