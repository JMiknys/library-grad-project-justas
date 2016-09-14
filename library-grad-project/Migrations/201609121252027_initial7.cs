namespace LibraryGradProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reservation", "User_Id", "dbo.User");
            DropIndex("dbo.Reservation", new[] { "User_Id" });
            DropColumn("dbo.Reservation", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservation", "User_Id", c => c.Int());
            CreateIndex("dbo.Reservation", "User_Id");
            AddForeignKey("dbo.Reservation", "User_Id", "dbo.User", "Id");
        }
    }
}
