namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuizChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quiz", "MagazineId", c => c.Int(nullable: false));
            CreateIndex("dbo.Quiz", "MagazineId");
            AddForeignKey("dbo.Quiz", "MagazineId", "dbo.Magazine", "MagazineId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Quiz", "MagazineId", "dbo.Magazine");
            DropIndex("dbo.Quiz", new[] { "MagazineId" });
            DropColumn("dbo.Quiz", "MagazineId");
        }
    }
}
