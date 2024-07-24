using SignalRServiceHub.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(); 

// To allow Progrssive Blazor Apps to access the service
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
builder.Services.AddControllers();

builder.Services.AddSignalR();
var app = builder.Build();

app.MapHub<ServiceHub>("/servicehub");

// To allow Progrssive Blazor Apps to access the service
app.UseCors("AllowAllOrigins");
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints?.MapControllers();
});

app.Run();
