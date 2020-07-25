namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnswerChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answer", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Answer", "IsDeleted");
        }
    }
}
