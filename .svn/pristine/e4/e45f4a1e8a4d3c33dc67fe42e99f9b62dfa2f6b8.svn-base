namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptionAddValues : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Answer", "Option_OptionId", "dbo.Option");
            DropIndex("dbo.Answer", new[] { "Option_OptionId" });
            AddColumn("dbo.Answer", "Value", c => c.Int());
            AddColumn("dbo.Option", "IsDeleted", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Option", "Value", c => c.Int());
            DropColumn("dbo.Answer", "Option_OptionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Answer", "Option_OptionId", c => c.Int());
            AlterColumn("dbo.Option", "Value", c => c.String());
            DropColumn("dbo.Option", "IsDeleted");
            DropColumn("dbo.Answer", "Value");
            CreateIndex("dbo.Answer", "Option_OptionId");
            AddForeignKey("dbo.Answer", "Option_OptionId", "dbo.Option", "OptionId");
        }
    }
}
