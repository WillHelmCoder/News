namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsDeletedToSlides : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Slide", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Slide", "IsDeleted");
        }
    }
}
