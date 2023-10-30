using Microsoft.AspNetCore.Mvc;
using scan_service.Models;
using scan_service.Requests;
using scan_service.Services;
using scan_service.Services.Interfaces;

namespace scan_service.Controllers;

[ApiController]
[Route("[controller]")]
public class ScannerController : ControllerBase
{
    private readonly IScannerService _service;
    private static int _taskId = 1;
    private static readonly Dictionary<int, Task<ScanReport>> Tasks = new ();

    public ScannerController(IScannerService service)
    {
        _service = service;
    }
    
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [Route("/scanner/add_task")]
    public IActionResult AddScanDirectoryTask(AddScanDirectoryTaskRequest request)
    {
        Tasks.Add(_taskId++, _service.ScanDirectory(request.Path));
        return Ok($"Task created with ID: {_taskId - 1}");
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
        if (Tasks[taskId].Exception != null && 
            Tasks[taskId].Exception!.InnerException is DirectoryNotFoundException)
            return StatusCode(
                StatusCodes.Status400BadRequest, 
                Tasks[taskId].Exception!.InnerException!.Message);

        return Ok(Tasks[taskId].Result);
    }
}