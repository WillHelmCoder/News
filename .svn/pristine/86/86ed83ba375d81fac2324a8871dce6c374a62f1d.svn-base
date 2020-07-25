namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class advertiseUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Advertise", "Url", c => c.String());
            AddColumn("dbo.Advertise", "IFrame", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Advertise", "IFrame");
            DropColumn("dbo.Advertise", "Url");
        }
    }
}
