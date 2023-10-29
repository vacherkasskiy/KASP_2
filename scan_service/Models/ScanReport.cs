namespace scan_service.Models;

public class ScanReport
{
    public string DirectoryPath { get; set; } = "";
    public int ScannedFilesAmount { get; set; }
    public int JsDetectsAmount { get; set; }
    public int DeletionDetectsAmount { get; set; }
    public int RunDetectsAmount { get; set; }
    public int ErrorsAmount { get; set; }
    public TimeSpan ExecutionTime { get; set; }
}