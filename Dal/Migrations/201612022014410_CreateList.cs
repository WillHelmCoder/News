namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ItemList",
                c => new
                    {
                        ItemListId = c.Int(nullable: false, identity: true),
                        ListId = c.Int(nullable: false),
                        Name = c.String(),
                        Url = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        ListOfItems_ListOfItemsId = c.Int(),
                    })
                .PrimaryKey(t => t.ItemListId)
                .ForeignKey("dbo.ListOfItems", t => t.ListOfItems_ListOfItemsId)
                .Index(t => t.ListOfItems_ListOfItemsId);
            
            CreateTable(
                "dbo.ListOfItems",
                c => new
                    {
                        ListOfItemsId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        MagazineId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ListOfItemsId)
                .ForeignKey("dbo.Magazine", t => t.MagazineId)
                .Index(t => t.MagazineId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ItemList", "ListOfItems_ListOfItemsId", "dbo.ListOfItems");
            DropForeignKey("dbo.ListOfItems", "MagazineId", "dbo.Magazine");
            DropIndex("dbo.ListOfItems", new[] { "MagazineId" });
            DropIndex("dbo.ItemList", new[] { "ListOfItems_ListOfItemsId" });
            DropTable("dbo.ListOfItems");
            DropTable("dbo.ItemList");
        }
    }
}
