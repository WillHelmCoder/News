namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataList_MagId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Data", "MagazineId", c => c.Int());
            CreateIndex("dbo.Data", "MagazineId");
            AddForeignKey("dbo.Data", "MagazineId", "dbo.Magazine", "MagazineId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Data", "MagazineId", "dbo.Magazine");
            DropIndex("dbo.Data", new[] { "MagazineId" });
            DropColumn("dbo.Data", "MagazineId");
        }
    }
}
