namespace LibraryGradProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Book", "CoverUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Book", "CoverUrl");
        }
    }
}
