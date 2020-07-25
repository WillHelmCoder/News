namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Url_OnDatas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Data", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Data", "Url");
        }
    }
}
