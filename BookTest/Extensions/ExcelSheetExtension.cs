using ClosedXML.Excel;

namespace BookTest.Extensions
{
	public static class ExcelSheetExtension
	{
		public static void AddHeader(this IXLWorksheet sheet, string[] cellHeader)
		{
			for (int i = 0; i < cellHeader.Length; i++)
			{
				sheet.Cell(1, i + 1).SetValue(cellHeader[i]);
			}
			var header=sheet.Range(1,1,1,cellHeader.Length);
			header.Style.Fill.BackgroundColor = XLColor.Black;
			header.Style.Font.FontColor = XLColor.White;
			header.Style.Font.Bold = true;
		}
		public static void Formate(this IXLWorksheet sheet)
		{
			sheet.ColumnsUsed().AdjustToContents();
			sheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
			sheet.CellsUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
			sheet.CellsUsed().Style.Border.OutsideBorderColor = XLColor.Red;
		}
		public static void AddLocalImage(this IXLWorksheet sheet, string imagePath)
		{
			sheet.AddPicture(imagePath)
				.MoveTo(sheet.Cell("A1"))
				.Scale(.02);
		}
	}
}
