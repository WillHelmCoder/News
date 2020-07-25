namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Encuestas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answer",
                c => new
                    {
                        AnswerId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        QuestionId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AnswerId)
                .ForeignKey("dbo.Question", t => t.QuestionId)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Question",
                c => new
                    {
                        QuestionId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        QuizId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QuestionId)
                .ForeignKey("dbo.Quiz", t => t.QuizId)
                .Index(t => t.QuizId);
            
            CreateTable(
                "dbo.Quiz",
                c => new
                    {
                        QuizId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.QuizId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Question", "QuizId", "dbo.Quiz");
            DropForeignKey("dbo.Answer", "QuestionId", "dbo.Question");
            DropIndex("dbo.Question", new[] { "QuizId" });
            DropIndex("dbo.Answer", new[] { "QuestionId" });
            DropTable("dbo.Quiz");
            DropTable("dbo.Question");
            DropTable("dbo.Answer");
        }
    }
}
