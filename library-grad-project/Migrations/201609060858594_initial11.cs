namespace LibraryGradProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial11 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Reservation", "BookId");
            AddForeignKey("dbo.Reservation", "BookId", "dbo.Book", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservation", "BookId", "dbo.Book");
            DropIndex("dbo.Reservation", new[] { "BookId" });
        }
    }
}
