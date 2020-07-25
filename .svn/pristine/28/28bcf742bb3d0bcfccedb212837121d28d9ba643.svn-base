namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Prueba : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.News", "Rank", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.News", "Rank", c => c.Long());
        }
    }
}
