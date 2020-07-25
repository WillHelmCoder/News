namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class algo : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.KeyPoint", name: "MagazineId", newName: "Magazine_MagazineId");
            RenameIndex(table: "dbo.KeyPoint", name: "IX_MagazineId", newName: "IX_Magazine_MagazineId");
            DropColumn("dbo.KeyPoint", "DataParentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.KeyPoint", "DataParentId", c => c.Int());
            RenameIndex(table: "dbo.KeyPoint", name: "IX_Magazine_MagazineId", newName: "IX_MagazineId");
            RenameColumn(table: "dbo.KeyPoint", name: "Magazine_MagazineId", newName: "MagazineId");
        }
    }
}
