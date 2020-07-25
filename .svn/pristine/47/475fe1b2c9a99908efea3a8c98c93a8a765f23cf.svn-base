namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemList_Newsletter : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ListOfItems", "MagazineId", "dbo.Magazine");
            DropIndex("dbo.ListOfItems", new[] { "MagazineId" });
            CreateTable(
                "dbo.ItemList",
                c => new
                    {
                        ItemListId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Url = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        MagazineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ItemListId)
                .ForeignKey("dbo.Magazine", t => t.MagazineId)
                .Index(t => t.MagazineId);
            
            AddColumn("dbo.NewsLetter", "Name", c => c.String());
            AddColumn("dbo.NewsLetter", "CreationDate", c => c.DateTime(nullable: false));
            DropTable("dbo.ListOfItems");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ListOfItems",
                c => new
                    {
                        ListOfItemsId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Url = c.String(),
                        MagazineId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ListOfItemsId);
            
            DropForeignKey("dbo.ItemList", "MagazineId", "dbo.Magazine");
            DropIndex("dbo.ItemList", new[] { "MagazineId" });
            DropColumn("dbo.NewsLetter", "CreationDate");
            DropColumn("dbo.NewsLetter", "Name");
            DropTable("dbo.ItemList");
            CreateIndex("dbo.ListOfItems", "MagazineId");
            AddForeignKey("dbo.ListOfItems", "MagazineId", "dbo.Magazine", "MagazineId");
        }
    }
}
