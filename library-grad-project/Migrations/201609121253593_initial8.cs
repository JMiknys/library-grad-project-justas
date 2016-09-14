namespace LibraryGradProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservation", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservation", "UserId");
        }
    }
}
