namespace api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManyToManyChoicesUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Choices", "User_Id", c => c.Int());
            AlterColumn("dbo.Users", "FbId", c => c.String());
            AddForeignKey("dbo.Choices", "User_Id", "dbo.Users", "Id");
            CreateIndex("dbo.Choices", "User_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Choices", new[] { "User_Id" });
            DropForeignKey("dbo.Choices", "User_Id", "dbo.Users");
            AlterColumn("dbo.Users", "FBId", c => c.String());
            DropColumn("dbo.Choices", "User_Id");
        }
    }
}
