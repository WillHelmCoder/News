namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class openQuestion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Question", "Open", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Question", "Open");
        }
    }
}
