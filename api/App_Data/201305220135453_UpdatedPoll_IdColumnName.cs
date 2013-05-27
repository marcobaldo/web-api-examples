namespace api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedPoll_IdColumnName : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Choices", name: "PollId", newName: "Poll_Id");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Choices", name: "Poll_Id", newName: "PollId");
        }
    }
}
