using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using VSM.DTO;

namespace VSM.Misc
{
    public static class BillPdfHelper
    {
        public static byte[] GenerateBillPdf(
            Guid billId,
            string customerName,
            string serviceDescription,
            IEnumerable<CategoryAmountDto> categoryAmounts,
            string billDescription,
            float totalAmount)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.Content()
                        .Column(col =>
                        {
                            col.Item().PaddingBottom(10).Element(x => x.Text("VSM").FontSize(28).Bold().FontColor(Colors.Blue.Medium).AlignCenter());
                            col.Item().PaddingBottom(10).Element(x => x.Text("Vehicle Service Bill").FontSize(20).Bold().AlignCenter());
                            col.Item().Element(x => x.Text($"Bill ID: {billId}").FontSize(12));
                            col.Item().Element(x => x.Text($"Customer Name: {customerName}").FontSize(12));
                            col.Item().Element(x => x.Text($"Service Description: {serviceDescription}").FontSize(12));
                            col.Item().PaddingBottom(10).Element(x => x.Text($"Bill Description: {billDescription}").FontSize(12));

                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Category").FontSize(12).Bold();
                                    header.Cell().Element(CellStyle).Text("Amount").FontSize(12).Bold();
                                });

                                foreach (var cat in categoryAmounts)
                                {
                                    table.Cell().Element(CellStyle).Text(cat.CategoryName).FontSize(11);
                                    table.Cell().Element(CellStyle).Text(cat.Amount.ToString("0.00")).FontSize(11);
                                }

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.PaddingVertical(4).PaddingHorizontal(2);
                                }
                            });

                            col.Item().PaddingTop(10).AlignRight().Element(x =>
                                x.Text($"Total Amount: {totalAmount:0.00}")
                                    .FontSize(14)
                                    .Bold()
                                    .FontColor(Colors.Green.Darken2)
                            );
                        });
                });
            });

            return document.GeneratePdf();
        }
    }
}