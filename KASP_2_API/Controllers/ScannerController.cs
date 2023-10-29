using KASP_2_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace KASP_2_API.Controllers;

[ApiController]
[Route("[controller]")]
public class ScannerController : ControllerBase
{
    private readonly ScannerService _service;

    public ScannerController(ScannerService service)
    {
        _service = service;
    }
    
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [Route("/scanner/scan")]
    public async Task<IActionResult> ScanDirectory(string path)
    {
        try
        {
            var report = await _service.ScanDirectory(path);
            return Ok(report);
        }
        catch (IOException e)
        {
            return StatusCode(
                StatusCodes.Status400BadRequest,
                e.Message);
        }
    }
}