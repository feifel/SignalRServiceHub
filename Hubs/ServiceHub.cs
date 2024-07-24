using Microsoft.AspNetCore.SignalR;

namespace SignalRServiceHub.Hubs;

public class ServiceHub : Hub
{
    public readonly ILogger<ServiceHub> Logger;
    public ServiceHub(ILogger<ServiceHub> logger)
    {
        Logger = logger;
    }
    //TODO: remove this
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task ServiceLookup(string serviceType)
    {
        await Clients.All.SendAsync("ServiceLookup", serviceType, Context.ConnectionId);
    }

    public async Task ProvideService(string serviceType, string serviceName, string connectionId)
    {
        await Clients.Client(connectionId).SendAsync("ProvideService", serviceName, Context.ConnectionId);
    }

    public async Task<string> ExecuteCommand(string cmd, string connectionId)
    {
        try
        {
            Logger.LogInformation($"ExecuteCommand: {cmd}");
            var res = await Clients.Client(connectionId).InvokeAsync<string>("ExecuteCommand", cmd, Context.ConnectionId, CancellationToken.None);
            Logger.LogInformation($"ExecuteCommand Result: {res}");
            return res;
        }
        catch (Exception ex)
        {
            throw new Exception($"Exception when running command: {cmd}{Environment.NewLine}{ex.Message}", ex);
        }
    }
}