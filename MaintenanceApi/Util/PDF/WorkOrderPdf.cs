using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MaintenanceApi.Util.PDF
{
    public class WorkOrderPdf : IDocument
    {
        public string Title { get; set; } = "Work Order";
        public string Description { get; set; } = "";
        public string Requestor { get; set; } = "";
        public string Status { get; set; } = "";
        public int Mechanic { get; set; } = 0;
        public string Asset { get; set; } = "";

        public DateTime? CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }

        public string ClosedDescription { get; set; } = "";
        public string? ClosedHours { get; set; }
        public string? ClosedMinutes { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string ClosedBy { get; set; } = "";

        public List<byte[]> WorkOrderImages { get; set; } = new();
        public List<byte[]> ClosedWorkOrderImages { get; set; } = new();

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Column(header =>
                {
                    header.Item().Text(Title)
                        .FontSize(22)
                        .Bold()
                        .FontColor(Colors.Blue.Medium);

                    header.Item().Text($"Generated: {DateTime.Now:MM/dd/yyyy hh:mm tt}")
                        .FontSize(9)
                        .FontColor(Colors.Grey.Darken1);
                });

                page.Content().Column(col =>
                {
                    col.Spacing(18);

                    // Main details section
                    col.Item().Element(SectionContainer).Column(section =>
                    {
                        section.Spacing(10);

                        section.Item().Text("Work Order Details")
                            .FontSize(14)
                            .Bold();

                        section.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(130);
                                columns.RelativeColumn();
                            });

                            void AddRow(string label, string value)
                            {
                                table.Cell().PaddingVertical(4).Text(label).Bold();
                                table.Cell().PaddingVertical(4).Text(value ?? "");
                            }

                            AddRow("Status", Status);
                            AddRow("Requestor", Requestor);
                            AddRow("Mechanic", "U Perezq");
                            AddRow("Asset", Asset);
                            AddRow("Created Date", CreatedDate?.ToString("MM/dd/yyyy") ?? "");
                            AddRow("Due Date", DueDate?.ToString("MM/dd/yyyy") ?? "");
                            AddRow("Description", Description);
                        });
                    });

                    // Work order images
                    if (WorkOrderImages != null && WorkOrderImages.Any())
                    {
                        col.Item().Element(SectionContainer).Column(section =>
                        {
                            section.Spacing(10);

                            section.Item().Text("Work Order Photos")
                                .FontSize(14)
                                .Bold();

                            section.Item().Grid(grid =>
                            {
                                grid.Columns(2);
                                grid.Spacing(10);

                                foreach (var image in WorkOrderImages)
                                {
                                    grid.Item().Border(1)
                                        .BorderColor(Colors.Grey.Lighten2)
                                        .Padding(5)
                                        .Height(180)
                                        .AlignMiddle()
                                        .AlignCenter()
                                        .Image(image, ImageScaling.FitArea);
                                }
                            });
                        });
                    }

                    // Completed section
                    if (Status?.Equals("Completed", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        col.Item().Element(SectionContainer).Column(section =>
                        {
                            section.Spacing(10);

                            section.Item().Text("Completion Details")
                                .FontSize(14)
                                .Bold()
                                .FontColor(Colors.Green.Darken2);

                            section.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(130);
                                    columns.RelativeColumn();
                                });

                                void AddRow(string label, string value)
                                {
                                    table.Cell().PaddingVertical(4).Text(label).Bold();
                                    table.Cell().PaddingVertical(4).Text(value ?? "");
                                }

                                AddRow("Closed By", ClosedBy);
                                AddRow("Closed Date", ClosedDate?.ToString("MM/dd/yyyy hh:mm tt") ?? "");
                                AddRow("Hours", ClosedHours?.ToString() ?? "0");
                                AddRow("Minutes", ClosedMinutes?.ToString() ?? "0");
                                AddRow("Closed Description", ClosedDescription);
                            });
                        });

                        if (ClosedWorkOrderImages != null && ClosedWorkOrderImages.Any())
                        {
                            col.Item().Element(SectionContainer).Column(section =>
                            {
                                section.Spacing(10);

                                section.Item().Text("Completion Photos")
                                    .FontSize(14)
                                    .Bold();

                                section.Item().Grid(grid =>
                                {
                                    grid.Columns(2);
                                    grid.Spacing(10);

                                    foreach (var image in ClosedWorkOrderImages)
                                    {
                                        grid.Item().Border(1)
                                            .BorderColor(Colors.Grey.Lighten2)
                                            .Padding(5)
                                            .Height(180)
                                            .AlignMiddle()
                                            .AlignCenter()
                                            .Image(image, ImageScaling.FitArea);
                                    }
                                });
                            });
                        }
                    }
                });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
            });
        }

        static IContainer SectionContainer(IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Background(Colors.White)
                .Padding(15);
        }
    }
}