namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Image_OnDatas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Data", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Data", "Image");
        }
    }
}
