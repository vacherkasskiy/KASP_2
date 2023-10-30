using scan_service.Models;

namespace scan_service.Services.Interfaces;

public interface IScannerService
{
    public Task<ScanReport> ScanDirectory(string relativePath);
}