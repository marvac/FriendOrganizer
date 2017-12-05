namespace FriendOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLanguages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Language",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Friend", "LanguageId", c => c.Int());
            CreateIndex("dbo.Friend", "LanguageId");
            AddForeignKey("dbo.Friend", "LanguageId", "dbo.Language", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Friend", "LanguageId", "dbo.Language");
            DropIndex("dbo.Friend", new[] { "LanguageId" });
            DropColumn("dbo.Friend", "LanguageId");
            DropTable("dbo.Language");
        }
    }
}
