using System.Globalization;
using Business.DTOs.Responses.Reports;
using ClosedXML.Excel;
using Data.Enums;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp;

namespace Business.Services.Reports;

public class ReportExportService : IReportExportService
{
    static ReportExportService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    private static byte[] RenderChartPng(int width, int height, Action<SKCanvas, SKRect> draw)
    {
        using var surface = SKSurface.Create(new SKImageInfo(width, height));
        var canvas = surface.Canvas;
        canvas.Clear(SKColors.White);

        draw(canvas, new SKRect(0, 0, width, height));

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 90);
        return data.ToArray();
    }

    private static byte[] GenerateLineChartImage(IReadOnlyList<double> values, IReadOnlyList<string> labels, string title)
    {
        const int width = 800;
        const int height = 360;
        return RenderChartPng(width, height, (canvas, area) =>
        {
            var backgroundPaint = new SKPaint { Color = SKColors.White, IsAntialias = true };
            canvas.DrawRect(area, backgroundPaint);

            var titlePaint = new SKPaint { Color = SKColors.Black, TextSize = 20, IsAntialias = true, Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold) };
            canvas.DrawText(title, area.Left + 16, area.Top + 28, titlePaint);

            var chartLeft = area.Left + 60;
            var chartTop = area.Top + 48;
            var chartRight = area.Right - 20;
            var chartBottom = area.Bottom - 40;
            var chartHeight = chartBottom - chartTop;
            var chartWidth = chartRight - chartLeft;

            var axisPaint = new SKPaint { Color = SKColors.Black, StrokeWidth = 2, IsAntialias = true };
            canvas.DrawLine(chartLeft, chartBottom, chartRight, chartBottom, axisPaint);
            canvas.DrawLine(chartLeft, chartBottom, chartLeft, chartTop, axisPaint);

            var maxValue = values.Count == 0 ? 1 : Math.Max(1, (int)Math.Ceiling(values.Max()));
            var stepCount = Math.Min(values.Count, 6);
            var yStep = chartHeight / 5f;
            var textPaint = new SKPaint { Color = SKColors.Black, TextSize = 12, IsAntialias = true };
            for (var i = 0; i <= 5; i++)
            {
                var y = chartBottom - i * yStep;
                canvas.DrawLine(chartLeft - 4, y, chartLeft, y, axisPaint);
                var label = Math.Round(maxValue * i / 5f).ToString(CultureInfo.InvariantCulture);
                canvas.DrawText(label, chartLeft - 10 - textPaint.MeasureText(label), y + 4, textPaint);
            }

            if (values.Count > 0)
            {
                var pointGap = chartWidth / Math.Max(1, values.Count - 1);
                var linePaint = new SKPaint { Color = SKColor.Parse("#3B82F6"), StrokeWidth = 4, IsAntialias = true, Style = SKPaintStyle.Stroke };
                var fillPaint = new SKPaint { Color = SKColor.Parse("#93C5FD").WithAlpha(120), IsAntialias = true, Style = SKPaintStyle.Fill };
                using var path = new SKPath();
                for (var index = 0; index < values.Count; index++)
                {
                    var x = chartLeft + index * pointGap;
                    var value = values[index];
                    var y = chartBottom - (float)(value / maxValue) * chartHeight;
                    if (index == 0)
                        path.MoveTo(x, y);
                    else
                        path.LineTo(x, y);
                }

                using var fillPath = new SKPath(path);
                fillPath.LineTo(chartLeft + (values.Count - 1) * pointGap, chartBottom);
                fillPath.LineTo(chartLeft, chartBottom);
                fillPath.Close();
                canvas.DrawPath(fillPath, fillPaint);
                canvas.DrawPath(path, linePaint);

                var dotPaint = new SKPaint { Color = SKColor.Parse("#1D4ED8"), IsAntialias = true };
                for (var index = 0; index < values.Count; index++)
                {
                    var x = chartLeft + index * pointGap;
                    var y = chartBottom - (float)(values[index] / maxValue) * chartHeight;
                    canvas.DrawCircle(x, y, 5, dotPaint);
                    var labelX = x - 18;
                    canvas.DrawText(labels[index], labelX, chartBottom + 18, textPaint);
                }
            }
        });
    }

    private static byte[] GenerateBarChartImage(IReadOnlyList<double> values, IReadOnlyList<string> labels, string title)
    {
        const int width = 800;
        const int height = 360;
        return RenderChartPng(width, height, (canvas, area) =>
        {
            var backgroundPaint = new SKPaint { Color = SKColors.White, IsAntialias = true };
            canvas.DrawRect(area, backgroundPaint);

            var titlePaint = new SKPaint { Color = SKColors.Black, TextSize = 20, IsAntialias = true, Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold) };
            canvas.DrawText(title, area.Left + 16, area.Top + 28, titlePaint);

            var chartLeft = area.Left + 60;
            var chartTop = area.Top + 48;
            var chartRight = area.Right - 20;
            var chartBottom = area.Bottom - 40;
            var chartHeight = chartBottom - chartTop;
            var chartWidth = chartRight - chartLeft;

            var axisPaint = new SKPaint { Color = SKColors.Black, StrokeWidth = 2, IsAntialias = true };
            canvas.DrawLine(chartLeft, chartBottom, chartRight, chartBottom, axisPaint);
            canvas.DrawLine(chartLeft, chartBottom, chartLeft, chartTop, axisPaint);

            var maxValue = values.Count == 0 ? 1 : Math.Max(1, (int)Math.Ceiling(values.Max()));
            var barCount = values.Count;
            var barSpacing = 12f;
            var barWidth = Math.Max(16, (chartWidth - barSpacing * (barCount + 1)) / barCount);
            var textPaint = new SKPaint { Color = SKColors.Black, TextSize = 12, IsAntialias = true };
            var barPaint = new SKPaint { Color = SKColor.Parse("#F97316"), IsAntialias = true, Style = SKPaintStyle.Fill };
            var labelPaint = new SKPaint { Color = SKColors.Black, TextSize = 10, IsAntialias = true };

            for (var index = 0; index < barCount; index++)
            {
                var x = chartLeft + barSpacing + index * (barWidth + barSpacing);
                var barHeight = (float)(values[index] / maxValue) * chartHeight;
                var rect = new SKRect(x, chartBottom - barHeight, x + barWidth, chartBottom);
                canvas.DrawRect(rect, barPaint);
                var label = labels[index];
                var labelWidth = labelPaint.MeasureText(label);
                var labelX = x + (barWidth - labelWidth) / 2f;
                canvas.DrawText(label, labelX, chartBottom + 16, labelPaint);
            }
        });
    }

    private static byte[] GeneratePieChartImage(IReadOnlyList<(string Name, double Value)> slices, string title)
    {
        const int width = 800;
        const int height = 360;
        return RenderChartPng(width, height, (canvas, area) =>
        {
            var backgroundPaint = new SKPaint { Color = SKColors.White, IsAntialias = true };
            canvas.DrawRect(area, backgroundPaint);

            var titlePaint = new SKPaint { Color = SKColors.Black, TextSize = 20, IsAntialias = true, Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold) };
            canvas.DrawText(title, area.Left + 16, area.Top + 28, titlePaint);

            var centerX = area.Left + 260;
            var centerY = area.Top + 190;
            var radius = Math.Min(180, Math.Min(area.Width, area.Height) / 4f);
            var total = Math.Max(1, slices.Sum(s => s.Value));
            var startAngle = -90f;
            var sliceColors = new[] { SKColor.Parse("#22C55E"), SKColor.Parse("#3B82F6"), SKColor.Parse("#F97316"), SKColor.Parse("#A855F7") };
            for (var index = 0; index < slices.Count; index++)
            {
                var slice = slices[index];
                var sweepAngle = (float)(360 * slice.Value / total);
                using var paint = new SKPaint { Color = sliceColors[index % sliceColors.Length], IsAntialias = true, Style = SKPaintStyle.Fill };
                canvas.DrawArc(new SKRect(centerX - radius, centerY - radius, centerX + radius, centerY + radius), startAngle, sweepAngle, true, paint);
                startAngle += sweepAngle;
            }

            var legendPaint = new SKPaint { Color = SKColors.Black, TextSize = 12, IsAntialias = true };
            for (var index = 0; index < slices.Count; index++)
            {
                var slice = slices[index];
                var top = area.Top + 60 + index * 24;
                using var colorPaint = new SKPaint { Color = sliceColors[index % sliceColors.Length], IsAntialias = true, Style = SKPaintStyle.Fill };
                canvas.DrawRect(new SKRect(area.Right - 240, top - 12, area.Right - 220, top + 4), colorPaint);
                canvas.DrawText($"{slice.Name}: {slice.Value:0.##}", area.Right - 210, top, legendPaint);
            }
        });
    }

    public ReportExportResult ExportAdminCourses(AdminCoursesReportDto report, ReportExportFormat format)
    {
        var baseName = $"admin-courses-{report.From:yyyy-MM-dd}_to_{report.To:yyyy-MM-dd}";
        return format switch
        {
            ReportExportFormat.Xlsx => ExportXlsx(baseName + ".xlsx", wb =>
            {
                var ws = wb.Worksheets.Add("Cursos");
                WriteAdminCoursesSheet(ws, report);
            }),
            ReportExportFormat.Pdf => ExportPdf(baseName + ".pdf", doc => BuildAdminCoursesPdf(doc, report)),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }

    public ReportExportResult ExportAdminTeachers(AdminTeachersReportDto report, ReportExportFormat format)
    {
        var baseName = $"admin-teachers-{report.From:yyyy-MM-dd}_to_{report.To:yyyy-MM-dd}";
        return format switch
        {
            ReportExportFormat.Xlsx => ExportXlsx(baseName + ".xlsx", wb =>
            {
                var ws = wb.Worksheets.Add("Docentes");
                WriteAdminTeachersSheet(ws, report);
            }),
            ReportExportFormat.Pdf => ExportPdf(baseName + ".pdf", doc => BuildAdminTeachersPdf(doc, report)),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }

    public ReportExportResult ExportTeacherSummary(TeacherSummaryReportDto report, ReportExportFormat format)
    {
        var baseName = $"teacher-summary-{report.From:yyyy-MM-dd}_to_{report.To:yyyy-MM-dd}";
        return format switch
        {
            ReportExportFormat.Xlsx => ExportXlsx(baseName + ".xlsx", wb =>
            {
                var ws = wb.Worksheets.Add("Resumen");
                WriteTeacherSummarySheet(ws, report);
            }),
            ReportExportFormat.Pdf => ExportPdf(baseName + ".pdf", doc => BuildTeacherSummaryPdf(doc, report)),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }

    public ReportExportResult ExportTeacherCourses(TeacherCoursesReportDto report, ReportExportFormat format)
    {
        var baseName = $"teacher-courses-{report.From:yyyy-MM-dd}_to_{report.To:yyyy-MM-dd}";
        return format switch
        {
            ReportExportFormat.Xlsx => ExportXlsx(baseName + ".xlsx", wb =>
            {
                var ws = wb.Worksheets.Add("Cursos");
                WriteTeacherCoursesSheet(ws, report);
            }),
            ReportExportFormat.Pdf => ExportPdf(baseName + ".pdf", doc => BuildTeacherCoursesPdf(doc, report)),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }

    public ReportExportResult ExportTeacherCourseDetail(TeacherCourseDetailReportDto report, ReportExportFormat format)
    {
        var baseName = $"teacher-course-{report.CourseId}-{report.From:yyyy-MM-dd}_to_{report.To:yyyy-MM-dd}";
        return format switch
        {
            ReportExportFormat.Xlsx => ExportXlsx(baseName + ".xlsx", wb =>
            {
                var ws = wb.Worksheets.Add("Curso");
                WriteTeacherCourseDetailSheet(ws, report);
            }),
            ReportExportFormat.Pdf => ExportPdf(baseName + ".pdf", doc => BuildTeacherCourseDetailPdf(doc, report)),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }

    private static ReportExportResult ExportXlsx(string fileName, Action<XLWorkbook> build)
    {
        using var wb = new XLWorkbook();
        build(wb);

        using var ms = new MemoryStream();
        wb.SaveAs(ms);

        return new ReportExportResult
        {
            Bytes = ms.ToArray(),
            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            FileName = fileName
        };
    }

    private static ReportExportResult ExportPdf(string fileName, Action<IDocumentContainer> build)
    {
        var bytes = Document.Create(build).GeneratePdf();

        return new ReportExportResult
        {
            Bytes = bytes,
            ContentType = "application/pdf",
            FileName = fileName
        };
    }

    private static void WriteAdminCoursesSheet(IXLWorksheet ws, AdminCoursesReportDto report)
    {
        ws.Cell(1, 1).Value = "Universidad Privada Domingo Savio";
        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontSize = 16;
        ws.Cell(1, 1).Style.Font.FontColor = XLColor.Blue;

        ws.Cell(2, 1).Value = "Sistema de Gestión de Aprendizaje (LMS)";
        ws.Cell(2, 1).Style.Font.FontSize = 12;
        ws.Cell(2, 1).Style.Font.FontColor = XLColor.Gray;

        ws.Cell(3, 1).Value = "Reporte: Cursos (Admin)";
        ws.Cell(3, 1).Style.Font.Bold = true;
        ws.Cell(3, 1).Style.Font.FontSize = 14;

        ws.Cell(4, 1).Value = "Desde";
        ws.Cell(4, 2).Value = report.From;
        ws.Cell(5, 1).Value = "Hasta";
        ws.Cell(5, 2).Value = report.To;

        var row = 7;
        ws.Cell(row, 1).Value = "CursoId";
        ws.Cell(row, 2).Value = "Título";
        ws.Cell(row, 3).Value = "DocenteId";
        ws.Cell(row, 4).Value = "Docente";
        ws.Cell(row, 5).Value = "Inscritos";
        ws.Cell(row, 6).Value = "Cancelados";
        ws.Cell(row, 7).Value = "Terminados";
        ws.Cell(row, 8).Value = "Tasa_terminación";

        var headerRange = ws.Range(row, 1, row, 8);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
        headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        foreach (var c in report.Courses)
        {
            row++;
            ws.Cell(row, 1).Value = c.CourseId;
            ws.Cell(row, 2).Value = c.Title;
            ws.Cell(row, 3).Value = c.TeacherId;
            ws.Cell(row, 4).Value = c.TeacherName;
            ws.Cell(row, 5).Value = c.TotalEnrollments;
            ws.Cell(row, 6).Value = c.TotalCancellations;
            ws.Cell(row, 7).Value = c.TotalCompletions;
            ws.Cell(row, 8).Value = c.CompletionRate;

            var dataRange = ws.Range(row, 1, row, 8);
            dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        ws.Columns().AdjustToContents();
    }

    private static void WriteAdminTeachersSheet(IXLWorksheet ws, AdminTeachersReportDto report)
    {
        ws.Cell(1, 1).Value = "Universidad Privada Domingo Savio";
        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontSize = 16;
        ws.Cell(1, 1).Style.Font.FontColor = XLColor.Blue;

        ws.Cell(2, 1).Value = "Sistema de Gestión de Aprendizaje (LMS)";
        ws.Cell(2, 1).Style.Font.FontSize = 12;
        ws.Cell(2, 1).Style.Font.FontColor = XLColor.Gray;

        ws.Cell(3, 1).Value = "Reporte: Docentes (Admin)";
        ws.Cell(3, 1).Style.Font.Bold = true;
        ws.Cell(3, 1).Style.Font.FontSize = 14;

        ws.Cell(4, 1).Value = "Desde";
        ws.Cell(4, 2).Value = report.From;
        ws.Cell(5, 1).Value = "Hasta";
        ws.Cell(5, 2).Value = report.To;

        var row = 7;
        ws.Cell(row, 1).Value = "DocenteId";
        ws.Cell(row, 2).Value = "Docente";
        ws.Cell(row, 3).Value = "Cursos";
        ws.Cell(row, 4).Value = "Inscritos";
        ws.Cell(row, 5).Value = "Cancelados";
        ws.Cell(row, 6).Value = "Terminados";
        ws.Cell(row, 7).Value = "Tasa_terminación";

        var headerRange = ws.Range(row, 1, row, 7);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
        headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        foreach (var t in report.Teachers)
        {
            row++;
            ws.Cell(row, 1).Value = t.TeacherId;
            ws.Cell(row, 2).Value = t.TeacherName;
            ws.Cell(row, 3).Value = t.TotalCourses;
            ws.Cell(row, 4).Value = t.TotalEnrollments;
            ws.Cell(row, 5).Value = t.TotalCancellations;
            ws.Cell(row, 6).Value = t.TotalCompletions;
            ws.Cell(row, 7).Value = t.CompletionRate;

            var dataRange = ws.Range(row, 1, row, 7);
            dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        ws.Columns().AdjustToContents();
    }

    private static void WriteTeacherSummarySheet(IXLWorksheet ws, TeacherSummaryReportDto report)
    {
        ws.Cell(1, 1).Value = "Reporte: Resumen Docente";
        ws.Cell(2, 1).Value = "Docente";
        ws.Cell(2, 2).Value = report.TeacherName;
        ws.Cell(3, 1).Value = "Desde";
        ws.Cell(3, 2).Value = report.From;
        ws.Cell(4, 1).Value = "Hasta";
        ws.Cell(4, 2).Value = report.To;

        ws.Cell(6, 1).Value = "Cursos";
        ws.Cell(6, 2).Value = report.TotalCourses;
        ws.Cell(7, 1).Value = "Inscritos";
        ws.Cell(7, 2).Value = report.TotalEnrollments;
        ws.Cell(8, 1).Value = "Cancelados";
        ws.Cell(8, 2).Value = report.TotalCancellations;
        ws.Cell(9, 1).Value = "Terminados";
        ws.Cell(9, 2).Value = report.TotalCompletions;
        ws.Cell(10, 1).Value = "Tasa_terminación";
        ws.Cell(10, 2).Value = report.CompletionRate;

        ws.Columns().AdjustToContents();
    }

    private static void WriteTeacherCoursesSheet(IXLWorksheet ws, TeacherCoursesReportDto report)
    {
        ws.Cell(1, 1).Value = "Reporte: Cursos del Docente";
        ws.Cell(2, 1).Value = "Docente";
        ws.Cell(2, 2).Value = report.TeacherName;
        ws.Cell(3, 1).Value = "Desde";
        ws.Cell(3, 2).Value = report.From;
        ws.Cell(4, 1).Value = "Hasta";
        ws.Cell(4, 2).Value = report.To;

        var row = 6;
        ws.Cell(row, 1).Value = "CursoId";
        ws.Cell(row, 2).Value = "Título";
        ws.Cell(row, 3).Value = "Publicado";
        ws.Cell(row, 4).Value = "Inscritos";
        ws.Cell(row, 5).Value = "Cancelados";
        ws.Cell(row, 6).Value = "Terminados";
        ws.Cell(row, 7).Value = "Tasa_terminación";
        ws.Range(row, 1, row, 7).Style.Font.Bold = true;

        foreach (var c in report.Courses)
        {
            row++;
            ws.Cell(row, 1).Value = c.CourseId;
            ws.Cell(row, 2).Value = c.Title;
            ws.Cell(row, 3).Value = c.Published;
            ws.Cell(row, 4).Value = c.TotalEnrollments;
            ws.Cell(row, 5).Value = c.TotalCancellations;
            ws.Cell(row, 6).Value = c.TotalCompletions;
            ws.Cell(row, 7).Value = c.CompletionRate;
        }

        ws.Columns().AdjustToContents();
    }

    private static void WriteTeacherCourseDetailSheet(IXLWorksheet ws, TeacherCourseDetailReportDto report)
    {
        ws.Cell(1, 1).Value = "Reporte: Curso (Detalle Docente)";
        ws.Cell(2, 1).Value = "Docente";
        ws.Cell(2, 2).Value = report.TeacherName;
        ws.Cell(3, 1).Value = "Curso";
        ws.Cell(3, 2).Value = report.CourseTitle;
        ws.Cell(4, 1).Value = "Desde";
        ws.Cell(4, 2).Value = report.From;
        ws.Cell(5, 1).Value = "Hasta";
        ws.Cell(5, 2).Value = report.To;

        ws.Cell(7, 1).Value = "Inscritos";
        ws.Cell(7, 2).Value = report.TotalEnrollments;
        ws.Cell(8, 1).Value = "Cancelados";
        ws.Cell(8, 2).Value = report.TotalCancellations;
        ws.Cell(9, 1).Value = "Terminados";
        ws.Cell(9, 2).Value = report.TotalCompletions;
        ws.Cell(10, 1).Value = "Tasa_terminación";
        ws.Cell(10, 2).Value = report.CompletionRate;

        ws.Columns().AdjustToContents();
    }

    private static void BuildAdminCoursesPdf(IDocumentContainer container, AdminCoursesReportDto report)
    {
        var totalEnrollments = report.Courses.Sum(c => c.TotalEnrollments);
        var totalCancellations = report.Courses.Sum(c => c.TotalCancellations);
        var totalCompletions = report.Courses.Sum(c => c.TotalCompletions);
        var active = totalEnrollments - totalCancellations - totalCompletions;

        var pieData = new[]
        {
            new { Name = "Activos", Value = (double)active },
            new { Name = "Cancelados", Value = (double)totalCancellations },
            new { Name = "Completados", Value = (double)totalCompletions }
        }.Where(d => d.Value > 0).ToArray();

        container.Page(page =>
        {
            page.Margin(24);
            page.Size(PageSizes.A4);
            page.DefaultTextStyle(x => x.FontSize(10));
            page.Header().Element(c => AddHeader(c, "Reporte de Cursos (Administrador)", $"Rango: {report.From:yyyy-MM-dd} a {report.To:yyyy-MM-dd}"));
            page.Footer().Element(c => AddFooter(c));
            page.Content().Column(col =>
            {
                col.Item().PaddingTop(20).Text("Detalle de Cursos").Bold().FontSize(14).FontColor(QuestPDF.Helpers.Colors.Blue.Darken4);

                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(40);
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(2);
                        columns.ConstantColumn(60);
                        columns.ConstantColumn(60);
                        columns.ConstantColumn(60);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(QuestPDF.Helpers.Colors.Blue.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Blue.Darken1).Padding(5).Text("ID").SemiBold().FontColor(QuestPDF.Helpers.Colors.Blue.Darken3);
                        header.Cell().Background(QuestPDF.Helpers.Colors.Blue.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Blue.Darken1).Padding(5).Text("Curso").SemiBold().FontColor(QuestPDF.Helpers.Colors.Blue.Darken3);
                        header.Cell().Background(QuestPDF.Helpers.Colors.Blue.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Blue.Darken1).Padding(5).Text("Docente").SemiBold().FontColor(QuestPDF.Helpers.Colors.Blue.Darken3);
                        header.Cell().Background(QuestPDF.Helpers.Colors.Blue.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Blue.Darken1).Padding(5).AlignRight().Text("Inscr.").SemiBold().FontColor(QuestPDF.Helpers.Colors.Blue.Darken3);
                        header.Cell().Background(QuestPDF.Helpers.Colors.Blue.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Blue.Darken1).Padding(5).AlignRight().Text("Canc.").SemiBold().FontColor(QuestPDF.Helpers.Colors.Blue.Darken3);
                        header.Cell().Background(QuestPDF.Helpers.Colors.Blue.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Blue.Darken1).Padding(5).AlignRight().Text("Term.").SemiBold().FontColor(QuestPDF.Helpers.Colors.Blue.Darken3);
                    });

                    foreach (var c in report.Courses.Take(200))
                    {
                        table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).Text(c.CourseId.ToString());
                        table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).Text(c.Title);
                        table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).Text(c.TeacherName ?? "-");
                        table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).AlignRight().Text(c.TotalEnrollments.ToString());
                        table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).AlignRight().Text(c.TotalCancellations.ToString());
                        table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).AlignRight().Text(c.TotalCompletions.ToString());
                    }
                });

                col.Item().PaddingTop(20).Text("Datos de Gráficas").Bold().FontSize(14).FontColor(QuestPDF.Helpers.Colors.Blue.Darken4).Underline();

                if (report.EnrollmentsByMonth.Any())
                {
                    col.Item().PaddingTop(10).Text("Inscripciones por Mes").SemiBold().FontSize(12).FontColor(QuestPDF.Helpers.Colors.Blue.Darken2);
                    var lineImage = GenerateLineChartImage(
                        report.EnrollmentsByMonth.Select(m => (double)m.Count).ToList(),
                        report.EnrollmentsByMonth.Select(m => $"{m.Year}-{m.Month:D2}").ToList(),
                        "Inscripciones por Mes"
                    );
                    col.Item().Image(lineImage).FitWidth();
                }

                if (pieData.Any())
                {
                    col.Item().PaddingTop(10).Text("Estado de Inscripciones").SemiBold().FontSize(12).FontColor(QuestPDF.Helpers.Colors.Blue.Darken2);
                    var pieImage = GeneratePieChartImage(
                        pieData.Select(d => (d.Name, d.Value)).ToList(),
                        "Estado de Inscripciones"
                    );
                    col.Item().Image(pieImage).FitWidth();
                }
            });
        });
    }

    private static void BuildAdminTeachersPdf(IDocumentContainer container, AdminTeachersReportDto report)
    {
        container.Page(page =>
        {
            page.Margin(24);
            page.Size(PageSizes.A4);
            page.DefaultTextStyle(x => x.FontSize(10));
            page.Header().Element(c => AddHeader(c, "Reporte de Docentes (Administrador)", $"Rango: {report.From:yyyy-MM-dd} a {report.To:yyyy-MM-dd}"));
            page.Footer().Element(c => AddFooter(c));
            page.Content().Column(col =>
            {
                col.Item().PaddingTop(20).Text("Detalle de Docentes").Bold().FontSize(14).FontColor(QuestPDF.Helpers.Colors.Blue.Darken4);

                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(50);
                        columns.RelativeColumn(3);
                        columns.ConstantColumn(50);
                        columns.ConstantColumn(60);
                        columns.ConstantColumn(60);
                        columns.ConstantColumn(60);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(QuestPDF.Helpers.Colors.Orange.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Orange.Darken1).Padding(5).Text("ID").SemiBold().FontColor(QuestPDF.Helpers.Colors.Orange.Darken3);
                        header.Cell().Background(QuestPDF.Helpers.Colors.Orange.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Orange.Darken1).Padding(5).Text("Docente").SemiBold().FontColor(QuestPDF.Helpers.Colors.Orange.Darken3);
                        header.Cell().Background(QuestPDF.Helpers.Colors.Orange.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Orange.Darken1).Padding(5).AlignRight().Text("Cursos").SemiBold().FontColor(QuestPDF.Helpers.Colors.Orange.Darken3);
                        header.Cell().Background(QuestPDF.Helpers.Colors.Orange.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Orange.Darken1).Padding(5).AlignRight().Text("Inscr.").SemiBold().FontColor(QuestPDF.Helpers.Colors.Orange.Darken3);
                        header.Cell().Background(QuestPDF.Helpers.Colors.Orange.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Orange.Darken1).Padding(5).AlignRight().Text("Canc.").SemiBold().FontColor(QuestPDF.Helpers.Colors.Orange.Darken3);
                        header.Cell().Background(QuestPDF.Helpers.Colors.Orange.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Orange.Darken1).Padding(5).AlignRight().Text("Term.").SemiBold().FontColor(QuestPDF.Helpers.Colors.Orange.Darken3);
                    });

                    foreach (var t in report.Teachers.Take(200))
                    {
                        table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).Text(t.TeacherId.ToString());
                        table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).Text(t.TeacherName);
                        table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).AlignRight().Text(t.TotalCourses.ToString());
                        table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).AlignRight().Text(t.TotalEnrollments.ToString());
                        table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).AlignRight().Text(t.TotalCancellations.ToString());
                        table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).AlignRight().Text(t.TotalCompletions.ToString());
                    }
                });

                col.Item().PaddingTop(20).Text("Datos de Gráficas").Bold().FontSize(14).FontColor(QuestPDF.Helpers.Colors.Blue.Darken4).Underline();

                if (report.Teachers.Any())
                {
                    col.Item().PaddingTop(10).Text("Inscripciones por Docente").SemiBold().FontSize(12).FontColor(QuestPDF.Helpers.Colors.Blue.Darken2);
                    var barImage = GenerateBarChartImage(
                        report.Teachers.Select(t => (double)t.TotalEnrollments).ToList(),
                        report.Teachers.Select(t => t.TeacherName.Length > 20 ? t.TeacherName.Substring(0, 20) + "..." : t.TeacherName).ToList(),
                        "Inscripciones por Docente"
                    );
                    col.Item().Image(barImage).FitWidth();
                }
            });
        });
    }

    private static void BuildTeacherSummaryPdf(IDocumentContainer container, TeacherSummaryReportDto report)
    {
        var active = report.TotalEnrollments - report.TotalCancellations - report.TotalCompletions;

        container.Page(page =>
        {
            page.Margin(24);
            page.Size(PageSizes.A4);
            page.DefaultTextStyle(x => x.FontSize(10));
            page.Header().Element(c => AddHeader(c, "Reporte de Resumen Docente", $"Docente: {report.TeacherName} | Rango: {report.From:yyyy-MM-dd} a {report.To:yyyy-MM-dd}"));
            page.Footer().Element(c => AddFooter(c));
            page.Content().Column(col =>
            {
                col.Item().PaddingTop(20).Text("Resumen de Actividad").Bold().FontSize(14).FontColor(QuestPDF.Helpers.Colors.Blue.Darken4);

                col.Item().PaddingTop(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(QuestPDF.Helpers.Colors.Green.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Green.Darken1).Padding(5).Text("Métrica").SemiBold().FontColor(QuestPDF.Helpers.Colors.Green.Darken3);
                        header.Cell().Background(QuestPDF.Helpers.Colors.Green.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Green.Darken1).Padding(5).AlignRight().Text("Valor").SemiBold().FontColor(QuestPDF.Helpers.Colors.Green.Darken3);
                    });

                    table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).Text("Cursos");
                    table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).AlignRight().Text(report.TotalCourses.ToString());
                    table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).Text("Inscritos");
                    table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).AlignRight().Text(report.TotalEnrollments.ToString());
                    table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).Text("Cancelados");
                    table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).AlignRight().Text(report.TotalCancellations.ToString());
                    table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).Text("Terminados");
                    table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).AlignRight().Text(report.TotalCompletions.ToString());
                    table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).Text("Tasa Terminación");
                    table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).AlignRight().Text(report.CompletionRate.ToString("0.####"));
                });

                if (report.TotalEnrollments > 0)
                {
                    var pieImage = GeneratePieChartImage(
                        new List<(string Name, double Value)>
                        {
                            ("Activos", report.TotalEnrollments - report.TotalCancellations - report.TotalCompletions),
                            ("Cancelados", report.TotalCancellations),
                            ("Completados", report.TotalCompletions),
                        }.Where(s => s.Value > 0).ToList(),
                        "Estado de Inscripciones"
                    );
                    col.Item().PaddingTop(16).Text("Gráfico de Estado").SemiBold().FontSize(12).FontColor(QuestPDF.Helpers.Colors.Blue.Darken2);
                    col.Item().Image(pieImage).FitWidth();
                }

                col.Item().PaddingTop(20).Text("Gráficas").Bold().FontSize(14);

                var pieData = new[]
                {
                    new { Name = "Activos", Value = (double)active },
                    new { Name = "Cancelados", Value = (double)report.TotalCancellations },
                    new { Name = "Completados", Value = (double)report.TotalCompletions }
                }.Where(d => d.Value > 0).ToArray();

                if (pieData.Any())
                {
                    col.Item().PaddingTop(10).Text("Estado de Inscripciones").SemiBold().FontSize(12).FontColor(QuestPDF.Helpers.Colors.Blue.Darken2);
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(1);
                        });
                        table.Header(header =>
                        {
                            header.Cell().Background(QuestPDF.Helpers.Colors.Green.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Green.Darken1).Padding(5).Text("Estado").SemiBold().FontColor(QuestPDF.Helpers.Colors.Green.Darken3);
                            header.Cell().Background(QuestPDF.Helpers.Colors.Green.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Green.Darken1).Padding(5).AlignRight().Text("Cantidad").SemiBold().FontColor(QuestPDF.Helpers.Colors.Green.Darken3);
                        });
                        foreach (var d in pieData)
                        {
                            table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).Text(d.Name);
                            table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).AlignRight().Text(d.Value.ToString());
                        }
                    });
                }
            });
        });
    }

    private static void BuildTeacherCoursesPdf(IDocumentContainer container, TeacherCoursesReportDto report)
    {
        container.Page(page =>
        {
            page.Margin(24);
            page.Size(PageSizes.A4);
            page.DefaultTextStyle(x => x.FontSize(10));
            page.Content().Column(col =>
            {
                col.Item().Text("Reporte: Cursos del Docente").Bold().FontSize(16).FontColor(QuestPDF.Helpers.Colors.Blue.Darken4);
                col.Item().Text($"Docente: {report.TeacherName}").FontColor(QuestPDF.Helpers.Colors.Grey.Darken2);
                col.Item().Text($"Rango: {report.From:yyyy-MM-dd} a {report.To:yyyy-MM-dd}").FontColor(QuestPDF.Helpers.Colors.Grey.Darken2);

                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(40);
                        columns.RelativeColumn(3);
                        columns.ConstantColumn(60);
                        columns.ConstantColumn(60);
                        columns.ConstantColumn(60);
                        columns.ConstantColumn(60);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(QuestPDF.Helpers.Colors.Purple.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Purple.Darken1).Padding(5).Text("ID").SemiBold().FontColor(QuestPDF.Helpers.Colors.Purple.Darken3);
                        header.Cell().Background(QuestPDF.Helpers.Colors.Purple.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Purple.Darken1).Padding(5).Text("Curso").SemiBold().FontColor(QuestPDF.Helpers.Colors.Purple.Darken3);
                        header.Cell().Background(QuestPDF.Helpers.Colors.Purple.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Purple.Darken1).Padding(5).AlignRight().Text("Inscr.").SemiBold().FontColor(QuestPDF.Helpers.Colors.Purple.Darken3);
                        header.Cell().Background(QuestPDF.Helpers.Colors.Purple.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Purple.Darken1).Padding(5).AlignRight().Text("Canc.").SemiBold().FontColor(QuestPDF.Helpers.Colors.Purple.Darken3);
                        header.Cell().Background(QuestPDF.Helpers.Colors.Purple.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Purple.Darken1).Padding(5).AlignRight().Text("Term.").SemiBold().FontColor(QuestPDF.Helpers.Colors.Purple.Darken3);
                        header.Cell().Background(QuestPDF.Helpers.Colors.Purple.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Purple.Darken1).Padding(5).AlignRight().Text("Tasa").SemiBold().FontColor(QuestPDF.Helpers.Colors.Purple.Darken3);
                    });

                    foreach (var c in report.Courses.Take(200))
                    {
                        table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).Text(c.CourseId.ToString());
                        table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).Text(c.Title);
                        table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).AlignRight().Text(c.TotalEnrollments.ToString());
                        table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).AlignRight().Text(c.TotalCancellations.ToString());
                        table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).AlignRight().Text(c.TotalCompletions.ToString());
                        table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).AlignRight().Text(c.CompletionRate.ToString("0.####"));
                    }
                });

                col.Item().PaddingTop(20).Text("Datos de Gráficas").Bold().FontSize(14).FontColor(QuestPDF.Helpers.Colors.Blue.Darken4).Underline();

                if (report.Courses.Any())
                {
                    col.Item().PaddingTop(10).Text("Inscripciones por Curso").SemiBold().FontSize(12).FontColor(QuestPDF.Helpers.Colors.Blue.Darken2);
                    var barImage = GenerateBarChartImage(
                        report.Courses.Select(c => (double)c.TotalEnrollments).ToList(),
                        report.Courses.Select(c => c.Title.Length > 30 ? c.Title.Substring(0, 30) + "..." : c.Title).ToList(),
                        "Inscripciones por Curso"
                    );
                    col.Item().Image(barImage).FitWidth();
                }
            });
        });
    }

    private static void BuildTeacherCourseDetailPdf(IDocumentContainer container, TeacherCourseDetailReportDto report)
    {
        container.Page(page =>
        {
            page.Margin(24);
            page.Size(PageSizes.A4);
            page.DefaultTextStyle(x => x.FontSize(10));
            page.Content().Column(col =>
            {
                col.Item().Text("Reporte: Curso (Detalle Docente)").Bold().FontSize(16).FontColor(QuestPDF.Helpers.Colors.Blue.Darken4);
                col.Item().Text($"Docente: {report.TeacherName}").FontColor(QuestPDF.Helpers.Colors.Grey.Darken2);
                col.Item().Text($"Curso: {report.CourseTitle}").FontColor(QuestPDF.Helpers.Colors.Grey.Darken2);
                col.Item().Text($"Rango: {report.From:yyyy-MM-dd} a {report.To:yyyy-MM-dd}").FontColor(QuestPDF.Helpers.Colors.Grey.Darken2);

                col.Item().PaddingTop(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(QuestPDF.Helpers.Colors.Red.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Red.Darken1).Padding(5).Text("Métrica").SemiBold().FontColor(QuestPDF.Helpers.Colors.Red.Darken3);
                        header.Cell().Background(QuestPDF.Helpers.Colors.Red.Lighten4).Border(1).BorderColor(QuestPDF.Helpers.Colors.Red.Darken1).Padding(5).AlignRight().Text("Valor").SemiBold().FontColor(QuestPDF.Helpers.Colors.Red.Darken3);
                    });

                    table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).Text("Inscritos");
                    table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).AlignRight().Text(report.TotalEnrollments.ToString());
                    table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).Text("Cancelados");
                    table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).AlignRight().Text(report.TotalCancellations.ToString());
                    table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).Text("Terminados");
                    table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).AlignRight().Text(report.TotalCompletions.ToString());
                    table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).Text("Tasa_terminación");
                    table.Cell().Border(0.5f).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1).Padding(5).AlignRight().Text(report.CompletionRate.ToString("0.####"));
                });
            });
        });
    }

    private static void AddHeader(IContainer container, string title, string? subtitle = null)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text("Universidad Privada Domingo Savio").FontSize(18).Bold().FontColor(Colors.Blue.Darken3);
                col.Item().Text("Sistema de Gestión de Aprendizaje (LMS)").FontSize(12).FontColor(Colors.Grey.Darken2);
                col.Item().Text(title).FontSize(14).SemiBold().FontColor(Colors.Blue.Darken2);
                if (subtitle != null) col.Item().Text(subtitle).FontSize(12);
            });
        });
    }

    private static void AddFooter(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().AlignRight().Text($"Generado el {DateTime.Now:yyyy-MM-dd HH:mm}").FontSize(8).FontColor(Colors.Grey.Darken2);
        });
    }
}

