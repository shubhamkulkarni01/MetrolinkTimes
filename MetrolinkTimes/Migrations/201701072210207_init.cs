namespace MetrolinkTimes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        latitude = c.Double(nullable: false),
                        longitude = c.Double(nullable: false),
                        Line = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StationTrains",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        time = c.DateTime(nullable: false),
                        Day = c.Int(nullable: false),
                        station_Id = c.Int(nullable: false),
                        train_Id = c.Int(),
                        user_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stations", t => t.station_Id, cascadeDelete: true)
                .ForeignKey("dbo.Trains", t => t.train_Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.station_Id)
                .Index(t => t.train_Id)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.Trains",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        train_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        fcm_id = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StationTrains", "user_Id", "dbo.Users");
            DropForeignKey("dbo.StationTrains", "train_Id", "dbo.Trains");
            DropForeignKey("dbo.StationTrains", "station_Id", "dbo.Stations");
            DropIndex("dbo.StationTrains", new[] { "user_Id" });
            DropIndex("dbo.StationTrains", new[] { "train_Id" });
            DropIndex("dbo.StationTrains", new[] { "station_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.Trains");
            DropTable("dbo.StationTrains");
            DropTable("dbo.Stations");
        }
    }
}
