using OlA3Redis;
using OlA3Redis.Dtos;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Redis connection
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetSection("Redis")["ConnectionString"];
    return ConnectionMultiplexer.Connect(configuration??"localhost:6379");
});

// Add Redis facade
builder.Services.AddScoped<IRedisService, RedisService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Define endpoints
app.MapPost("/set", async (IRedisService redisService, SetRequest request) =>
{
    var result = await redisService.SetValueAsync(request.Key, request.Value, TimeSpan.FromHours(24));
    return result ? Results.Ok("Set successfully") : Results.Problem("Failed to set value");
});


app.MapGet("/get/{key}", async (IRedisService redisService, string key) =>
{
    var value = await redisService.GetValueAsync(key);
    return value is null ? Results.NotFound() : Results.Ok(value);
});

app.MapPut("/update", async (IRedisService redisService, string key, string value) =>
{
    var result = await redisService.UpdateValueAsync(key, value, TimeSpan.FromHours(24));
    return result ? Results.Ok("Updated successfully") : Results.NotFound("Key does not exist");
});

app.MapDelete("/delete/{key}", async (IRedisService redisService, string key) =>
{
    var result = await redisService.DeleteValueAsync(key);
    return result ? Results.Ok("Deleted successfully") : Results.NotFound();
});

app.Run();