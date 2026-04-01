namespace Business.Services.Reports;

public class ReportExportResult
{
    public byte[] Bytes { get; set; } = [];
    public string ContentType { get; set; } = "application/octet-stream";
    public string FileName { get; set; } = "report.bin";
}

