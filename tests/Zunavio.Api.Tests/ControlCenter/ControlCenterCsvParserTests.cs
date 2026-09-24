using Zunavio.Api.ControlCenter;

namespace Zunavio.Api.Tests.ControlCenter;

public sealed class ControlCenterCsvParserTests
{
    [Fact]
    public void ParseProjects_keeps_quoted_commas_inside_marketplace_cell()
    {
        const string csv = "Project_ID,Working_Title,Marketplace,Language,Current_Gate,Status,Market_Score,Next_Action,Project_Folder_URL\n" +
            "ZNV-010,My Digital Life Emergency Planner,\"United States primary; United Kingdom, Canada, Australia secondary\",English,REJECTED,ARCHIVED,67,NONE,https://drive.google.com/drive/folders/abc";

        var projects = ControlCenterCsvParser.ParseProjects(csv);

        Assert.Single(projects);
        Assert.Equal("ZNV-010", projects[0].ProjectId);
        Assert.Equal("United States primary; United Kingdom, Canada, Australia secondary", projects[0].Marketplace);
        Assert.Equal("English", projects[0].Language);
        Assert.Equal("REJECTED", projects[0].CurrentGate);
        Assert.Equal("ARCHIVED", projects[0].Status);
        Assert.Equal("67", projects[0].MarketScore);
        Assert.Equal("NONE", projects[0].NextAction);
    }

    [Fact]
    public void ParseProjects_supports_google_sheet_export_index_column()
    {
        const string csv = ",Project_ID,Working_Title,Marketplace,Language,Current_Gate,Status,Next_Action\n" +
            "7,ZNV-015,Aging Parents Home Safety Audit & Action Planner,\"United States primary; Canada, United Kingdom, Australia secondary\",English,MARKET_RESEARCH,ACTIVE,REVIEW_SCOUT_DECISION";

        var projects = ControlCenterCsvParser.ParseProjects(csv);

        Assert.Single(projects);
        Assert.Equal("ZNV-015", projects[0].ProjectId);
        Assert.Equal("United States primary; Canada, United Kingdom, Australia secondary", projects[0].Marketplace);
        Assert.Equal("REVIEW_SCOUT_DECISION", projects[0].NextAction);
    }
}
