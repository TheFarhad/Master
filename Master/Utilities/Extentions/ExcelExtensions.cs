namespace Master.Utilities.Extentions;

using System.Data;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

public static class ExcelExtensions
{
    public static byte[] ToExcelByteArray<T>(this List<T> source, string sheetName = "Result")
    {
        using ExcelPackage excelPackage = new ExcelPackage();
        var ws = excelPackage.Workbook.Worksheets.Add(sheetName);
        var t = typeof(T);
        var headings = t.GetProperties();

        for (int i = 0; i < headings.Count(); i++)
        {
            ws.Cells[1, i + 1].Value = headings[i].Name;
        }

        //populate our Data
        if (source.Count() > 0) ws.Cells["A2"].LoadFromCollection(source);

        using (ExcelRange rng = ws.Cells["A1:BZ1"])
        {
            rng.Style.Font.Bold = true;
            rng.Style.Fill.PatternType = ExcelFillStyle.Solid;  //Set Pattern for the background to Solid
            rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));  //Set color to dark blue
            rng.Style.Font.Color.SetColor(Color.White);
        }

        return excelPackage.GetAsByteArray();
    }

    public static DataTable ExcelToDataTable(this byte[] source)
    {
        var result = new DataTable();
        var excelPackage = new ExcelPackage();
        var hasHeader = true;

        using MemoryStream memoryStream = new MemoryStream(source);
        excelPackage.Load(memoryStream);

        var ws = excelPackage.Workbook.Worksheets.First();

        foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
        {
            result.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
        }

        var startRow = hasHeader ? 2 : 1;

        for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
        {
            var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
            var row = result.NewRow();
            foreach (var cell in wsRow)
            {
                row[cell.Start.Column - 1] = cell.Text;
            }
            result.Rows.Add(row);
        }

        excelPackage.Dispose();

        return result;
    }
}