using Hangfire;
using Hangfire.Redis.StackExchange;
using HangFire.Api.Configuration;
using HangFire.Api.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHangfire(options =>
{
    var connectionString = builder.Configuration.GetValue<string>("RedusConection");
    var redis = ConnectionMultiplexer.Connect(connectionString);
    options.UseRedisStorage(redis, options: new RedisStorageOptions { Prefix = $"HANG_FIRE" });
});
builder.Services.AddHangfireServer();
builder.Services.AddHostedService<MonitorService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = HangFireDashboard.AuthAuthorizationFilters()
});
app.Run();