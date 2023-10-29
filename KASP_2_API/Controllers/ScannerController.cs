using KASP_2_API.Models;
using KASP_2_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace KASP_2_API.Controllers;

[ApiController]
[Route("[controller]")]
public class ScannerController : ControllerBase
{
    private readonly ScannerService _service;
    private static int _taskId = 1;
    private static readonly Dictionary<int, Task<ScanReport>> Tasks = new ();

    public ScannerController(ScannerService service)
    {
        _service = service;
    }
    
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [Route("/scanner/add_task")]
    public IActionResult AddScanDirectoryTask(string path)
    {
        try
        {
            Tasks.Add(_taskId++, _service.ScanDirectory(path));
            return Ok($"Task created with ID: {_taskId - 1}");
        }
        catch (IOException e)
        {
            return StatusCode(
                StatusCodes.Status400BadRequest,
                e.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(202)]
    [ProducesResponseType(400)]
    [Route("/scanner/get_task_status")]
    public IActionResult GetScanDirectoryTaskStatus(int taskId)
    {
        if (!Tasks.ContainsKey(taskId))
            return StatusCode(
                StatusCodes.Status400BadRequest,
                "Wrong task id provided");
        if (!Tasks[taskId].IsCompleted)
            return StatusCode(
                StatusCodes.Status202Accepted, 
                "Scan task in progress, please wait");

        return Ok(Tasks[taskId].Result);
    }
}