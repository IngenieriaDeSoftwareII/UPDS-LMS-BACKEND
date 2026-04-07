using Business.DTOs.Responses.Reports;
using ClosedXML.Excel;
using Data.Enums;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Business.Services.Reports;

public class ReportExportService : IReportExportService
{
    static ReportExportService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
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
        // Header
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
        // Header
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
        container.Page(page =>
        {
            page.Margin(24);
            page.Size(PageSizes.A4);
            page.DefaultTextStyle(x => x.FontSize(10));
            page.Header().Element(c => AddHeader(c, "Reporte de Cursos (Administrador)", $"Rango: {report.From:yyyy-MM-dd} a {report.To:yyyy-MM-dd}"));
            page.Footer().Element(c => AddFooter(c));
            page.Content().Column(col =>
            {
                col.Item().PaddingTop(20).Text("Detalle de Cursos").Bold().FontSize(14);

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
                        header.Cell().Background(Colors.Blue.Lighten4).Border(1).BorderColor(Colors.Blue.Darken1).Text("ID").SemiBold();
                        header.Cell().Background(Colors.Blue.Lighten4).Border(1).BorderColor(Colors.Blue.Darken1).Text("Curso").SemiBold();
                        header.Cell().Background(Colors.Blue.Lighten4).Border(1).BorderColor(Colors.Blue.Darken1).Text("Docente").SemiBold();
                        header.Cell().Background(Colors.Blue.Lighten4).Border(1).BorderColor(Colors.Blue.Darken1).AlignRight().Text("Inscr.").SemiBold();
                        header.Cell().Background(Colors.Blue.Lighten4).Border(1).BorderColor(Colors.Blue.Darken1).AlignRight().Text("Canc.").SemiBold();
                        header.Cell().Background(Colors.Blue.Lighten4).Border(1).BorderColor(Colors.Blue.Darken1).AlignRight().Text("Term.").SemiBold();
                    });

                    foreach (var c in report.Courses.Take(200))
                    {
                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).Text(c.CourseId.ToString());
                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).Text(c.Title);
                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).Text(c.TeacherName ?? "-");
                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).AlignRight().Text(c.TotalEnrollments.ToString());
                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).AlignRight().Text(c.TotalCancellations.ToString());
                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).AlignRight().Text(c.TotalCompletions.ToString());
                    }
                });
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
                col.Item().PaddingTop(20).Text("Detalle de Docentes").Bold().FontSize(14);

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
                        header.Cell().Background(Colors.Blue.Lighten4).Border(1).BorderColor(Colors.Blue.Darken1).Text("ID").SemiBold();
                        header.Cell().Background(Colors.Blue.Lighten4).Border(1).BorderColor(Colors.Blue.Darken1).Text("Docente").SemiBold();
                        header.Cell().Background(Colors.Blue.Lighten4).Border(1).BorderColor(Colors.Blue.Darken1).AlignRight().Text("Cursos").SemiBold();
                        header.Cell().Background(Colors.Blue.Lighten4).Border(1).BorderColor(Colors.Blue.Darken1).AlignRight().Text("Inscr.").SemiBold();
                        header.Cell().Background(Colors.Blue.Lighten4).Border(1).BorderColor(Colors.Blue.Darken1).AlignRight().Text("Canc.").SemiBold();
                        header.Cell().Background(Colors.Blue.Lighten4).Border(1).BorderColor(Colors.Blue.Darken1).AlignRight().Text("Term.").SemiBold();
                    });

                    foreach (var t in report.Teachers.Take(200))
                    {
                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).Text(t.TeacherId.ToString());
                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).Text(t.TeacherName);
                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).AlignRight().Text(t.TotalCourses.ToString());
                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).AlignRight().Text(t.TotalEnrollments.ToString());
                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).AlignRight().Text(t.TotalCancellations.ToString());
                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).AlignRight().Text(t.TotalCompletions.ToString());
                    }
                });
            });
        });
    }

    private static void BuildTeacherSummaryPdf(IDocumentContainer container, TeacherSummaryReportDto report)
    {
        container.Page(page =>
        {
            page.Margin(24);
            page.Size(PageSizes.A4);
            page.DefaultTextStyle(x => x.FontSize(10));
            page.Header().Element(c => AddHeader(c, "Reporte de Resumen Docente", $"Docente: {report.TeacherName} | Rango: {report.From:yyyy-MM-dd} a {report.To:yyyy-MM-dd}"));
            page.Footer().Element(c => AddFooter(c));
            page.Content().Column(col =>
            {
                col.Item().PaddingTop(20).Text("Resumen de Actividad").Bold().FontSize(14);

                col.Item().PaddingTop(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Blue.Lighten4).Border(1).BorderColor(Colors.Blue.Darken1).Text("Métrica").SemiBold();
                        header.Cell().Background(Colors.Blue.Lighten4).Border(1).BorderColor(Colors.Blue.Darken1).AlignRight().Text("Valor").SemiBold();
                    });

                    table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).Text("Cursos");
                    table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).AlignRight().Text(report.TotalCourses.ToString());
                    table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).Text("Inscritos");
                    table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).AlignRight().Text(report.TotalEnrollments.ToString());
                    table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).Text("Cancelados");
                    table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).AlignRight().Text(report.TotalCancellations.ToString());
                    table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).Text("Terminados");
                    table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).AlignRight().Text(report.TotalCompletions.ToString());
                    table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).Text("Tasa Terminación");
                    table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).AlignRight().Text(report.CompletionRate.ToString("0.####"));
                });
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
                col.Item().Text("Reporte: Cursos del Docente").Bold().FontSize(16);
                col.Item().Text($"Docente: {report.TeacherName}");
                col.Item().Text($"Rango: {report.From:yyyy-MM-dd} a {report.To:yyyy-MM-dd}").FontColor(Colors.Grey.Darken2);

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
                        header.Cell().Text("ID").SemiBold();
                        header.Cell().Text("Curso").SemiBold();
                        header.Cell().AlignRight().Text("Inscr.").SemiBold();
                        header.Cell().AlignRight().Text("Canc.").SemiBold();
                        header.Cell().AlignRight().Text("Term.").SemiBold();
                        header.Cell().AlignRight().Text("Tasa").SemiBold();
                    });

                    foreach (var c in report.Courses.Take(200))
                    {
                        table.Cell().Text(c.CourseId.ToString());
                        table.Cell().Text(c.Title);
                        table.Cell().AlignRight().Text(c.TotalEnrollments.ToString());
                        table.Cell().AlignRight().Text(c.TotalCancellations.ToString());
                        table.Cell().AlignRight().Text(c.TotalCompletions.ToString());
                        table.Cell().AlignRight().Text(c.CompletionRate.ToString("0.####"));
                    }
                });
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
                col.Item().Text("Reporte: Curso (Detalle Docente)").Bold().FontSize(16);
                col.Item().Text($"Docente: {report.TeacherName}");
                col.Item().Text($"Curso: {report.CourseTitle}");
                col.Item().Text($"Rango: {report.From:yyyy-MM-dd} a {report.To:yyyy-MM-dd}").FontColor(Colors.Grey.Darken2);

                col.Item().PaddingTop(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(2);
                    });

                    table.Cell().Text("Inscritos").SemiBold();
                    table.Cell().AlignRight().Text(report.TotalEnrollments.ToString());
                    table.Cell().Text("Cancelados").SemiBold();
                    table.Cell().AlignRight().Text(report.TotalCancellations.ToString());
                    table.Cell().Text("Terminados").SemiBold();
                    table.Cell().AlignRight().Text(report.TotalCompletions.ToString());
                    table.Cell().Text("Tasa_terminación").SemiBold();
                    table.Cell().AlignRight().Text(report.CompletionRate.ToString("0.####"));
                });
            });
        });
    }

    private static void AddHeader(IContainer container, string title, string subtitle = null)
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

