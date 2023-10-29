using KASP_2_API.Models;

namespace KASP_2_API.Services;

public class ScannerService
{
    private const string JsScript = "<script>evil_script()</script>";
    private const string DeletionScript = "rm -rf %userprofile%\\Documents";
    private const string RunDllScript = "Rundll32 sys.dll SysEntry";
    
    private static async Task<FileTypes> CheckFileForViruses(string filePath)
    {
        try
        {
            string fileContent = await File.ReadAllTextAsync(filePath);

            if (Path.GetExtension(filePath) == ".js" && fileContent.Contains(JsScript))
                return FileTypes.JsScript;
            if (fileContent.Contains(DeletionScript))
                return FileTypes.DeletionScript;
            if (fileContent.Contains(RunDllScript)) 
                return FileTypes.RunDllScript;
            
            return FileTypes.Secure;
        }
        catch (Exception e)
        {
            return FileTypes.Unstable;
        }
    }
    
    private static async Task ReadFilesRecursively(string directoryPath, ScanReport report)
    {
        var files = Directory.GetFiles(directoryPath);
        foreach (var file in files)
        {
            var fileType = await CheckFileForViruses(file);
            report.ScannedFilesAmount += 1;
            
            switch (fileType)
            {
                case FileTypes.Secure:
                    break;
                case FileTypes.JsScript:
                    report.JsDetectsAmount += 1;
                    break;
                case FileTypes.DeletionScript:
                    report.DeletionDetectsAmount += 1;
                    break;
                case FileTypes.RunDllScript:
                    report.RunDetectsAmount += 1;
                    break;
                case FileTypes.Unstable:
                    report.ErrorsAmount += 1;
                    break;
            }
        }
            
        var subdirectories = Directory.GetDirectories(directoryPath);
        foreach (var subdirectory in subdirectories)
        {
            await ReadFilesRecursively(subdirectory, report);
        }
    }
    
    public async Task<ScanReport> ScanDirectory(string relativePath)
    {
        string path = Path.Combine(
            Path.GetPathRoot(Environment.SystemDirectory)!,
            relativePath);
        
        if (!Directory.Exists(path)) 
            throw new IOException("Directory does not exists");
        
        var report = new ScanReport();
        DateTime startTime = DateTime.Now;

        await ReadFilesRecursively(path, report);
        
        DateTime endTime = DateTime.Now;
        report.ExecutionTime = endTime - startTime;

        return report;
    }
}