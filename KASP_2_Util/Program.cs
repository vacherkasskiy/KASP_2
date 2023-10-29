using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Net;
using System.Text;
using System.Text.Json;
using KASP_2_Util.Models;

namespace KASP_2_Util;

static class Program
{
    static async Task PrintAddScanDirectoryTaskResponse(string baseUrl, string path)
    {
        using var httpClient = new HttpClient();
        
        var requestData = new { path };
        var jsonRequestData = JsonSerializer.Serialize(requestData);
        var content = new StringContent(jsonRequestData, Encoding.UTF8, "application/json");
        var addScanDirectoryTaskResponse = await httpClient.PostAsync($"{baseUrl}/scanner/add_task", content);
        var message = await addScanDirectoryTaskResponse.Content.ReadAsStringAsync();

        Console.WriteLine(message);
    }
    
    static async Task PrintGetScanDirectoryTaskStatusResponse(string baseUrl, int taskId)
    {
        using var httpClient = new HttpClient();
        var getScanDirectoryTaskStatusResponse = await httpClient.GetAsync($"{baseUrl}/scanner/get_task_status?taskId={taskId}");
        var message = await getScanDirectoryTaskStatusResponse.Content.ReadAsStringAsync();
        if (getScanDirectoryTaskStatusResponse.StatusCode != HttpStatusCode.OK)
        {
            Console.WriteLine(message);
            return;
        }
        
        var taskResult = JsonSerializer.Deserialize<ScanReport>(message)!;
        Console.WriteLine(taskResult);
    }
    
    static async Task Main(string[] args)
    {
        const string baseUrl = "https://localhost:7297"; // поменять на свой
        var rootCommand = new RootCommand("Scanner Utility");
        
        var addTaskCommand = new Command("scan", "Add a new task for scanner");
        addTaskCommand.AddArgument(new Argument<string>("path", "Directory path which will be scanned"));
        addTaskCommand.Handler = CommandHandler.Create<string>(async (path) =>
        {
            await PrintAddScanDirectoryTaskResponse(baseUrl, path);
        });
        
        var getStatusCommand = new Command("status", "Get task status");
        getStatusCommand.AddArgument(new Argument<int>("taskId", "Task id which status will be returned"));
        getStatusCommand.Handler = CommandHandler.Create<int>(async (taskId) =>
        {
            await PrintGetScanDirectoryTaskStatusResponse(baseUrl, taskId);
        });
        
        rootCommand.AddCommand(addTaskCommand);
        rootCommand.AddCommand(getStatusCommand);
        await rootCommand.InvokeAsync(args);
    }
}