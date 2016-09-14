namespace LibraryGradProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial5 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.User", "Salt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "Salt", c => c.String());
        }
    }
}
