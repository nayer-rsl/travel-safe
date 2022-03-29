namespace PassionSem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CountryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        CountryID = c.Int(nullable: false, identity: true),
                        CountryName = c.String(),
                        CountryContinent = c.String(),
                    })
                .PrimaryKey(t => t.CountryID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Countries");
        }
    }
}
