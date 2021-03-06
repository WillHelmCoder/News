namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuizValidations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quiz", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Quiz", "IsDeleted");
        }
    }
}
