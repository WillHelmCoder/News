namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Optionchanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Option",
                c => new
                    {
                        OptionId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Value = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OptionId)
                .ForeignKey("dbo.Question", t => t.QuestionId)
                .Index(t => t.QuestionId);
            
            AddColumn("dbo.Answer", "Option_OptionId", c => c.Int());
            CreateIndex("dbo.Answer", "Option_OptionId");
            AddForeignKey("dbo.Answer", "Option_OptionId", "dbo.Option", "OptionId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Option", "QuestionId", "dbo.Question");
            DropForeignKey("dbo.Answer", "Option_OptionId", "dbo.Option");
            DropIndex("dbo.Option", new[] { "QuestionId" });
            DropIndex("dbo.Answer", new[] { "Option_OptionId" });
            DropColumn("dbo.Answer", "Option_OptionId");
            DropTable("dbo.Option");
        }
    }
}
