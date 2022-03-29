namespace PassionSem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class document : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        DocumentID = c.Int(nullable: false, identity: true),
                        DocumentName = c.String(),
                    })
                .PrimaryKey(t => t.DocumentID);
            
            CreateTable(
                "dbo.DocumentCountries",
                c => new
                    {
                        Document_DocumentID = c.Int(nullable: false),
                        Country_CountryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Document_DocumentID, t.Country_CountryID })
                .ForeignKey("dbo.Documents", t => t.Document_DocumentID, cascadeDelete: true)
                .ForeignKey("dbo.Countries", t => t.Country_CountryID, cascadeDelete: true)
                .Index(t => t.Document_DocumentID)
                .Index(t => t.Country_CountryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocumentCountries", "Country_CountryID", "dbo.Countries");
            DropForeignKey("dbo.DocumentCountries", "Document_DocumentID", "dbo.Documents");
            DropIndex("dbo.DocumentCountries", new[] { "Country_CountryID" });
            DropIndex("dbo.DocumentCountries", new[] { "Document_DocumentID" });
            DropTable("dbo.DocumentCountries");
            DropTable("dbo.Documents");
        }
    }
}
