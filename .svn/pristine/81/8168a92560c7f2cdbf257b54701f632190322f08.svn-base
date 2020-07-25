namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Data",
                c => new
                    {
                        DataId = c.Int(nullable: false, identity: true),
                        DataParentId = c.Int(),
                        Title = c.String(),
                        Description = c.String(),
                        Order = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DataId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Data");
        }
    }
}
