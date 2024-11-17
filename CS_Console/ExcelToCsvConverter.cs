using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class ExcelToCsvConverter
{
    public void ConvertXlsxToCsv(string xlsxFilePath, string csvFolderPath)
    {
        using (var fileStream = new FileStream(xlsxFilePath, FileMode.Open, FileAccess.Read))
        {
            IWorkbook workbook = new XSSFWorkbook(fileStream);

            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                ISheet sheet = workbook.GetSheetAt(i);
                string csvFilePath = Path.Combine(csvFolderPath, sheet.SheetName + ".csv");

                using (var writer = new StreamWriter(csvFilePath, false, Encoding.UTF8))
                {
                    for (int row = 0; row <= sheet.LastRowNum; row++)
                    {
                        IRow currentRow = sheet.GetRow(row);
                        if (currentRow == null) continue;

                        var rowValues = new List<string>();

                        for (int col = 0; col < currentRow.LastCellNum; col++)
                        {
                            var cell = currentRow.GetCell(col);
                            var cellValue = cell?.ToString() ?? string.Empty;

                            // Handle cells containing commas by wrapping in quotes
                            if (cellValue.Contains(","))
                            {
                                cellValue = $"\"{cellValue}\"";
                            }
                            rowValues.Add(cellValue);
                        }

                        writer.WriteLine(string.Join(",", rowValues));
                    }
                }

                Console.WriteLine($"Worksheet '{sheet.SheetName}' has been converted to CSV at '{csvFilePath}'.");
            }
        }
    }
}
//Put this in program.cs
//string xlsxFilePath = @"C:\Users\ossingh\Downloads\Data for securities.xlsx - Equities.xlsx";
//string csvFilePath = @"C:\Users\ossingh\Downloads";

//var converter = new ExcelToCsvConverter();
//converter.ConvertXlsxToCsv(xlsxFilePath, csvFilePath);