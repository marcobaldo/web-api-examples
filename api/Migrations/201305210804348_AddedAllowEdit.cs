namespace api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAllowEdit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Polls", "MaxVotes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Polls", "MaxVotes");
        }
    }
}
