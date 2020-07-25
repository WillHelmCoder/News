namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Question", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Question", "IsDeleted");
        }
    }
}
