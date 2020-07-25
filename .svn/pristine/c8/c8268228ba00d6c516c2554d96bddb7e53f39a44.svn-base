namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imageSign : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Data", "imageSign", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Data", "imageSign");
        }
    }
}
