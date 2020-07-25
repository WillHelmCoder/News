namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addKeyPoints : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KeyPointsContainer", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.KeyPointsContainer", "Description");
        }
    }
}
