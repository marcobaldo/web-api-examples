namespace api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttemptToFixMany2Many : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Choices", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Choice_Id", "dbo.Choices");
            DropIndex("dbo.Choices", new[] { "User_Id" });
            DropIndex("dbo.Users", new[] { "Choice_Id" });
            CreateTable(
                "dbo.Choices_Users",
                c => new
                    {
                        Choice_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Choice_Id, t.User_Id })
                .ForeignKey("dbo.Choices", t => t.Choice_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Choice_Id)
                .Index(t => t.User_Id);
            
            DropColumn("dbo.Choices", "User_Id");
            DropColumn("dbo.Users", "Choice_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Choice_Id", c => c.Int());
            AddColumn("dbo.Choices", "User_Id", c => c.Int());
            DropIndex("dbo.Choices_Users", new[] { "User_Id" });
            DropIndex("dbo.Choices_Users", new[] { "Choice_Id" });
            DropForeignKey("dbo.Choices_Users", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Choices_Users", "Choice_Id", "dbo.Choices");
            DropTable("dbo.Choices_Users");
            CreateIndex("dbo.Users", "Choice_Id");
            CreateIndex("dbo.Choices", "User_Id");
            AddForeignKey("dbo.Users", "Choice_Id", "dbo.Choices", "Id");
            AddForeignKey("dbo.Choices", "User_Id", "dbo.Users", "Id");
        }
    }
}
