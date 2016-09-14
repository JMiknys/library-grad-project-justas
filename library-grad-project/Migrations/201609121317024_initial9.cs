namespace LibraryGradProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservation", "User_Id", c => c.Int());
            CreateIndex("dbo.Reservation", "User_Id");
            AddForeignKey("dbo.Reservation", "User_Id", "dbo.User", "Id");
            DropColumn("dbo.Reservation", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservation", "UserId", c => c.String());
            DropForeignKey("dbo.Reservation", "User_Id", "dbo.User");
            DropIndex("dbo.Reservation", new[] { "User_Id" });
            DropColumn("dbo.Reservation", "User_Id");
        }
    }
}
