namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ItemList", "ListOfItems_ListOfItemsId", "dbo.ListOfItems");
            DropIndex("dbo.ItemList", new[] { "ListOfItems_ListOfItemsId" });
            DropTable("dbo.ItemList");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.ItemListId);
            
            CreateIndex("dbo.ItemList", "ListOfItems_ListOfItemsId");
            AddForeignKey("dbo.ItemList", "ListOfItems_ListOfItemsId", "dbo.ListOfItems", "ListOfItemsId");
        }
    }
}
