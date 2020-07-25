namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GalerySystem_06092016 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Galery",
                c => new
                    {
                        GaleryId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Alt = c.String(),
                        MetaDesc = c.String(),
                        Keywords = c.String(),
                        Permalink = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        MagazineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GaleryId)
                .ForeignKey("dbo.Magazine", t => t.MagazineId)
                .Index(t => t.MagazineId);
            
            CreateTable(
                "dbo.GaleryImage",
                c => new
                    {
                        GaleryImageId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        GaleryId = c.Int(nullable: false),
                        ImageId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GaleryImageId)
                .ForeignKey("dbo.Image", t => t.ImageId)
                .ForeignKey("dbo.Galery", t => t.GaleryId)
                .Index(t => t.GaleryId)
                .Index(t => t.ImageId);
            
            CreateTable(
                "dbo.Image",
                c => new
                    {
                        ImageId = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Title = c.String(),
                        Alt = c.String(),
                        UploadIP = c.String(),
                        UploadDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ImageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Galery", "MagazineId", "dbo.Magazine");
            DropForeignKey("dbo.GaleryImage", "GaleryId", "dbo.Galery");
            DropForeignKey("dbo.GaleryImage", "ImageId", "dbo.Image");
            DropIndex("dbo.GaleryImage", new[] { "ImageId" });
            DropIndex("dbo.GaleryImage", new[] { "GaleryId" });
            DropIndex("dbo.Galery", new[] { "MagazineId" });
            DropTable("dbo.Image");
            DropTable("dbo.GaleryImage");
            DropTable("dbo.Galery");
        }
    }
}
