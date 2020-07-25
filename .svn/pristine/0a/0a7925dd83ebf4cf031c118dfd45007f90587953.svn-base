namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isDeleted2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Slider", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Slider", "IsDeleted");
        }
    }
}
