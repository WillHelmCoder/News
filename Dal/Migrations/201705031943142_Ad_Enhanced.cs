namespace News.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Ad_Enhanced : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Advertise", "Source", c => c.String());
            AddColumn("dbo.Advertise", "Medium", c => c.String());
            AddColumn("dbo.Advertise", "Campaign", c => c.String());
            AddColumn("dbo.Advertise", "Term", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Advertise", "Term");
            DropColumn("dbo.Advertise", "Campaign");
            DropColumn("dbo.Advertise", "Medium");
            DropColumn("dbo.Advertise", "Source");
        }
    }
}
