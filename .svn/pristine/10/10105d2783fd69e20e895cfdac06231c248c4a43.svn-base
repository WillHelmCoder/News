namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class listOfItems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ListOfItems", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ListOfItems", "Url");
        }
    }
}
