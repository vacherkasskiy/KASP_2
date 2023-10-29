using System.Text.Json.Serialization;

namespace scan_util.Models;

public class ScanReport
{
    [JsonPropertyName("directoryPath")] public string DirectoryPath { get; set; } = "";
    [JsonPropertyName("scannedFilesAmount")] public int ScannedFilesAmount { get; set; }
    [JsonPropertyName("jsDetectsAmount")] public int JsDetectsAmount { get; set; }
    [JsonPropertyName("deletionDetectsAmount")] public int DeletionDetectsAmount { get; set; }
    [JsonPropertyName("runDetectsAmount")] public int RunDetectsAmount { get; set; }
    [JsonPropertyName("errorsAmount")] public int ErrorsAmount { get; set; }
    [JsonPropertyName("executionTime")] public TimeSpan ExecutionTime { get; set; }

    public override string ToString()
    {
        string result =
            "====== Scan result ======\n" +
            $"Directory: {DirectoryPath}\n" +
            $"Processed files: {ScannedFilesAmount}\n" +
            $"JS detects: {JsDetectsAmount}\n" +
            $"rm -rf detects: {DeletionDetectsAmount}\n" +
            $"Rundll32 detects: {RunDetectsAmount}\n" +
            $"Errors: {ErrorsAmount}\n" +
            $"Execution time: {ExecutionTime}\n" +
            $"=========================\n";


        return result;
    }
}