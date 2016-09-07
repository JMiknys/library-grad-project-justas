namespace LibraryGradProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reservation", "BookId", "dbo.Book");
            DropIndex("dbo.Reservation", new[] { "BookId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Reservation", "BookId");
            AddForeignKey("dbo.Reservation", "BookId", "dbo.Book", "Id", cascadeDelete: true);
        }
    }
}
