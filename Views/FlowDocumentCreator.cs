using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Model;

namespace Views
{
    public class FlowDocumentCreator
    {
        private const int ColumnGap = 0;

        private const int InitialRowNumber = 1;

        private const double CellBorderThickness = 1;

        private const double CellPadding = 3;

        private const double FontSize = 14;

        private const double SmallFontSize = 10;

        private const string FontFamily = "Arial";
        
        [SecurityCritical]
        public FlowDocument Create(string fileFullName, IEnumerable<Account> accounts, PageSize pageSize)
        {
            var flowDocument = new FlowDocument
            {
                PageWidth = pageSize.Width, 
                PageHeight = pageSize.Height, 
                ColumnGap = ColumnGap, // одна колонка на страницу
                ColumnWidth = pageSize.Width // одна колонка на страницу
            };

            if (!string.IsNullOrWhiteSpace(fileFullName))
            {
                if (File.Exists(fileFullName))
                {
                    var fileInfo = new FileInfo(fileFullName);

                    var paragraph = new Paragraph();
                    paragraph.Inlines.Add(new Run($"File path: {fileInfo.FullName} - Last modified date: {fileInfo.LastWriteTime}"));
                    paragraph.FontFamily = new FontFamily(FontFamily);
                    paragraph.FontSize = SmallFontSize;
                    flowDocument.Blocks.Add(paragraph);
                }
            }

            var groups = accounts.GroupBy(x => x.AccountType);
            var rowCounter = InitialRowNumber;

            foreach (var group in groups)
            {
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Italic(new Bold(new Run(group.Key))));
                paragraph.FontFamily = new FontFamily(FontFamily);
                paragraph.FontSize = FontSize;

                var table = new Table();
                
                table.Columns.Add(new TableColumn {Width = new GridLength(5, GridUnitType.Star)});
                table.Columns.Add(new TableColumn {Width = new GridLength(20, GridUnitType.Star)});
                table.Columns.Add(new TableColumn {Width = new GridLength(20, GridUnitType.Star)});
                table.Columns.Add(new TableColumn {Width = new GridLength(20, GridUnitType.Star)});
                table.Columns.Add(new TableColumn {Width = new GridLength(35, GridUnitType.Star)});
                
                var rowGroup = new TableRowGroup();
                var headerRow = new TableRow { Background = GetRowColor(rowGroup) };
                rowGroup.Rows.Add(headerRow);
                table.RowGroups.Add(rowGroup);
                
                headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("#")))));
                headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Resource name")))));
                headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Login")))));
                headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Password")))));
                headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Comment")))));
                
                foreach(var account in group)
                {
                    var row = new TableRow { Background = GetRowColor(rowGroup)};
                    rowGroup.Rows.Add(row);
                    row.Cells.Add(new TableCell(new Paragraph(new Run($"{rowCounter++}"))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run(account.ResourceName))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run($"{account.Login}"))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run($"{account.Password.ToUnsecure()}"))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run(account.Comment))));
                }

                foreach (var cell in table.RowGroups.SelectMany(x => x.Rows).SelectMany(x => x.Cells))
                {
                    cell.BorderBrush = Brushes.Black;
                    cell.BorderThickness = new Thickness(CellBorderThickness);
                    cell.Padding = new Thickness(CellPadding);
                    cell.FontFamily = new FontFamily(FontFamily);
                    cell.FontSize = FontSize;
                }

                flowDocument.Blocks.Add(paragraph);
                flowDocument.Blocks.Add(table);
            }
            
            return flowDocument;
        }

        private SolidColorBrush GetRowColor(TableRowGroup rowGroup)
        {
            return rowGroup.Rows.Count % 2 == 0 ? Brushes.LightGray : Brushes.White;
        }
    }

    public class PageSize
    {
        public PageSize(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public double Width { get; }

        public double Height { get; }
    }
}