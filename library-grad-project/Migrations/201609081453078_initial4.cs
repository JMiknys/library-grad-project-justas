namespace LibraryGradProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "Name", c => c.String(maxLength: 100));
            CreateIndex("dbo.User", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.User", new[] { "Name" });
            AlterColumn("dbo.User", "Name", c => c.String());
        }
    }
}
