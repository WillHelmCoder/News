namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ad_Orientation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Advertise", "Horizontal", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Advertise", "Horizontal");
        }
    }
}
