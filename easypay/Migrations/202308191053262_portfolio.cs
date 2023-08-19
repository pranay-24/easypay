namespace easypay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class portfolio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        PaymentId = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentDate = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        TransactionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentId)
                .ForeignKey("dbo.Transactions", t => t.TransactionId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.TransactionId);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionID = c.Int(nullable: false, identity: true),
                        TransactionType = c.String(),
                        Description = c.String(),
                        TransactionRemarks = c.String(),
                    })
                .PrimaryKey(t => t.TransactionID);
            
            CreateTable(
                "dbo.Properties",
                c => new
                    {
                        PropertyID = c.Int(nullable: false, identity: true),
                        PropertyName = c.String(),
                        PropertyRent = c.Int(nullable: false),
                        PropertyPrice = c.Int(nullable: false),
                        PlayerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PropertyID)
                .ForeignKey("dbo.Players", t => t.PlayerID, cascadeDelete: true)
                .Index(t => t.PlayerID);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        PlayerID = c.Int(nullable: false, identity: true),
                        PlayerName = c.String(),
                        PlayerBalance = c.Int(nullable: false),
                        PlayerPosition = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlayerID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Portfolios",
                c => new
                    {
                        PortfolioID = c.Int(nullable: false, identity: true),
                        PortfolioName = c.String(),
                        Risk = c.String(),
                    })
                .PrimaryKey(t => t.PortfolioID);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        StocksID = c.Int(nullable: false, identity: true),
                        StockName = c.String(),
                        StockBuyPrice = c.Int(nullable: false),
                        StockSellPrice = c.Int(nullable: false),
                        StockQty = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StocksID);
            
            CreateTable(
                "dbo.PropertyTransactions",
                c => new
                    {
                        Property_PropertyID = c.Int(nullable: false),
                        Transaction_TransactionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Property_PropertyID, t.Transaction_TransactionID })
                .ForeignKey("dbo.Properties", t => t.Property_PropertyID, cascadeDelete: true)
                .ForeignKey("dbo.Transactions", t => t.Transaction_TransactionID, cascadeDelete: true)
                .Index(t => t.Property_PropertyID)
                .Index(t => t.Transaction_TransactionID);
            
            CreateTable(
                "dbo.StockPortfolios",
                c => new
                    {
                        Stock_StocksID = c.Int(nullable: false),
                        Portfolio_PortfolioID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Stock_StocksID, t.Portfolio_PortfolioID })
                .ForeignKey("dbo.Stocks", t => t.Stock_StocksID, cascadeDelete: true)
                .ForeignKey("dbo.Portfolios", t => t.Portfolio_PortfolioID, cascadeDelete: true)
                .Index(t => t.Stock_StocksID)
                .Index(t => t.Portfolio_PortfolioID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockPortfolios", "Portfolio_PortfolioID", "dbo.Portfolios");
            DropForeignKey("dbo.StockPortfolios", "Stock_StocksID", "dbo.Stocks");
            DropForeignKey("dbo.Payments", "UserId", "dbo.Users");
            DropForeignKey("dbo.Payments", "TransactionId", "dbo.Transactions");
            DropForeignKey("dbo.PropertyTransactions", "Transaction_TransactionID", "dbo.Transactions");
            DropForeignKey("dbo.PropertyTransactions", "Property_PropertyID", "dbo.Properties");
            DropForeignKey("dbo.Properties", "PlayerID", "dbo.Players");
            DropIndex("dbo.StockPortfolios", new[] { "Portfolio_PortfolioID" });
            DropIndex("dbo.StockPortfolios", new[] { "Stock_StocksID" });
            DropIndex("dbo.PropertyTransactions", new[] { "Transaction_TransactionID" });
            DropIndex("dbo.PropertyTransactions", new[] { "Property_PropertyID" });
            DropIndex("dbo.Properties", new[] { "PlayerID" });
            DropIndex("dbo.Payments", new[] { "TransactionId" });
            DropIndex("dbo.Payments", new[] { "UserId" });
            DropTable("dbo.StockPortfolios");
            DropTable("dbo.PropertyTransactions");
            DropTable("dbo.Stocks");
            DropTable("dbo.Portfolios");
            DropTable("dbo.Users");
            DropTable("dbo.Players");
            DropTable("dbo.Properties");
            DropTable("dbo.Transactions");
            DropTable("dbo.Payments");
        }
    }
}
