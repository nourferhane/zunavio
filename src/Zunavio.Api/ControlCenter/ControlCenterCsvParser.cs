using System.Text;

namespace Zunavio.Api.ControlCenter;

public static class ControlCenterCsvParser
{
    public static IReadOnlyList<ControlCenterProjectRow> ParseProjects(string csvContent)
    {
        if (string.IsNullOrWhiteSpace(csvContent))
        {
            return Array.Empty<ControlCenterProjectRow>();
        }

        var rows = ParseRows(csvContent)
            .Where(row => row.Any(cell => !string.IsNullOrWhiteSpace(cell)))
            .ToList();

        if (rows.Count < 2)
        {
            return Array.Empty<ControlCenterProjectRow>();
        }

        var header = rows[0];
        var indexes = header
            .Select((name, index) => new { Name = NormalizeHeader(name), Index = index })
            .Where(item => !string.IsNullOrWhiteSpace(item.Name))
            .GroupBy(item => item.Name)
            .ToDictionary(group => group.Key, group => group.First().Index);

        return rows
            .Skip(1)
            .Where(row => !string.IsNullOrWhiteSpace(Get(row, indexes, "Project_ID")))
            .Select(row => new ControlCenterProjectRow
            {
                ProjectId = Get(row, indexes, "Project_ID"),
                WorkingTitle = Get(row, indexes, "Working_Title"),
                FinalTitle = Get(row, indexes, "Final_Title"),
                Marketplace = Get(row, indexes, "Marketplace"),
                Language = Get(row, indexes, "Language"),
                TargetAge = Get(row, indexes, "Target_Age"),
                BookType = Get(row, indexes, "Book_Type"),
                Season = Get(row, indexes, "Season"),
                CurrentGate = Get(row, indexes, "Current_Gate"),
                Status = Get(row, indexes, "Status"),
                MarketScore = Get(row, indexes, "Market_Score"),
                ManuscriptVersion = Get(row, indexes, "Manuscript_Version"),
                VisualBibleVersion = Get(row, indexes, "Visual_Bible_Version"),
                ProductionVersion = Get(row, indexes, "Production_Version"),
                QaResult = Get(row, indexes, "QA_Result"),
                NextAction = Get(row, indexes, "Next_Action"),
                CreatedAt = Get(row, indexes, "Created_At"),
                UpdatedAt = Get(row, indexes, "Updated_At"),
                ProjectFolderUrl = Get(row, indexes, "Project_Folder_URL")
            })
            .ToList();
    }

    private static IReadOnlyList<IReadOnlyList<string>> ParseRows(string csvContent)
    {
        var rows = new List<IReadOnlyList<string>>();
        var currentRow = new List<string>();
        var currentCell = new StringBuilder();
        var insideQuotes = false;

        for (var i = 0; i < csvContent.Length; i++)
        {
            var current = csvContent[i];

            if (current == '"')
            {
                if (insideQuotes && i + 1 < csvContent.Length && csvContent[i + 1] == '"')
                {
                    currentCell.Append('"');
                    i++;
                    continue;
                }

                insideQuotes = !insideQuotes;
                continue;
            }

            if (current == ',' && !insideQuotes)
            {
                currentRow.Add(currentCell.ToString().Trim());
                currentCell.Clear();
                continue;
            }

            if ((current == '\n' || current == '\r') && !insideQuotes)
            {
                if (current == '\r' && i + 1 < csvContent.Length && csvContent[i + 1] == '\n')
                {
                    i++;
                }

                currentRow.Add(currentCell.ToString().Trim());
                currentCell.Clear();
                rows.Add(currentRow);
                currentRow = new List<string>();
                continue;
            }

            currentCell.Append(current);
        }

        currentRow.Add(currentCell.ToString().Trim());
        rows.Add(currentRow);

        return rows;
    }

    private static string Get(
        IReadOnlyList<string> row,
        IReadOnlyDictionary<string, int> indexes,
        string columnName)
    {
        return indexes.TryGetValue(NormalizeHeader(columnName), out var index) && index < row.Count
            ? row[index]
            : string.Empty;
    }

    private static string NormalizeHeader(string value)
    {
        return value.Trim().TrimStart('\ufeff').Replace(" ", "_").ToUpperInvariant();
    }
}
