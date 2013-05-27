namespace api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemvoedPollCircularReference : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Choices", "Poll_Id", "dbo.Polls");
            DropIndex("dbo.Choices", new[] { "Poll_Id" });
            RenameColumn(table: "dbo.Choices", name: "Poll_Id", newName: "PollId");
            AddForeignKey("dbo.Choices", "PollId", "dbo.Polls", "Id", cascadeDelete: true);
            CreateIndex("dbo.Choices", "PollId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Choices", new[] { "PollId" });
            DropForeignKey("dbo.Choices", "PollId", "dbo.Polls");
            RenameColumn(table: "dbo.Choices", name: "PollId", newName: "Poll_Id");
            CreateIndex("dbo.Choices", "Poll_Id");
            AddForeignKey("dbo.Choices", "Poll_Id", "dbo.Polls", "Id");
        }
    }
}
