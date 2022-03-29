namespace PassionSem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Foreignkey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Countries", "LanguageID", c => c.Int(nullable: false));
            CreateIndex("dbo.Countries", "LanguageID");
            AddForeignKey("dbo.Countries", "LanguageID", "dbo.Languages", "LanguageID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Countries", "LanguageID", "dbo.Languages");
            DropIndex("dbo.Countries", new[] { "LanguageID" });
            DropColumn("dbo.Countries", "LanguageID");
        }
    }
}
