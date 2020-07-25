namespace News.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdManager : DbMigration
    {
        /// <summary>
        /// En tabla AdCategory
        /// Se agregaron los campos StartDate y EndDate para controlar la vigencia de los anuncios
        /// Se agrego el campo Tags para controlar con que articulos se mostraran los anuncios pertenecientes a un grupo
        /// En tabla Advertise
        /// Se quitaron los campos Keywords, MetaDesc, Alt y Permalink
        /// Ahora seran generados al momento de la consulta para ahorrar campos en la base de datos
        /// </summary>

        public override void Up()
        {
            AddColumn("dbo.AdCategory", "Tags", c => c.String());
            AddColumn("dbo.AdCategory", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AdCategory", "EndDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Advertise", "Alt");
            DropColumn("dbo.Advertise", "MetaDesc");
            DropColumn("dbo.Advertise", "Keywords");
            DropColumn("dbo.Advertise", "Permalink");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Advertise", "Permalink", c => c.String());
            AddColumn("dbo.Advertise", "Keywords", c => c.String());
            AddColumn("dbo.Advertise", "MetaDesc", c => c.String());
            AddColumn("dbo.Advertise", "Alt", c => c.String());
            DropColumn("dbo.AdCategory", "EndDate");
            DropColumn("dbo.AdCategory", "StartDate");
            DropColumn("dbo.AdCategory", "Tags");
        }
    }
}
