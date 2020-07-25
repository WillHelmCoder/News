namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdCategory_Active : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdCategory", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdCategory", "Active");
        }
    }
}
