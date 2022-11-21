using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region FixedWindow
//builder.Services.AddRateLimiter(options =>
//{
//    options.AddFixedWindowLimiter("Fixed", opt =>
//    {
//        opt.Window = TimeSpan.FromSeconds(10);
//        opt.PermitLimit = 4;
//        opt.QueueLimit = 2;
//        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;

//    });
//});
#endregion

#region Sliding Window

//builder.Services.AddRateLimiter(options =>
//{
//    options.AddSlidingWindowLimiter("Sliding", opt =>
//    {
//        opt.Window = TimeSpan.FromSeconds(10);
//        opt.PermitLimit = 4;
//        opt.QueueLimit = 2;
//        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
//        opt.SegmentsPerWindow = 2;

//    });
//});

#endregion

#region tokenBucket
//builder.Services.AddRateLimiter(options =>
//{
//    options.AddTokenBucketLimiter("Token", opt =>
//    {
//        opt.TokenLimit =4;
//        opt.QueueLimit = 2;
//        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
//        opt.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
//        opt.TokensPerPeriod = 4;
//        opt.AutoReplenishment = true;

//    });
//});
#endregion

#region Concurrency
builder.Services.AddRateLimiter(options =>
{
    options.AddConcurrencyLimiter("Concurrency", opt =>
    {
        opt.PermitLimit = 10;
        opt.QueueLimit = 2;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
      

    });

    options.OnRejected = (context, CancellationToken) =>
    {
        //log operations
        return new();
    };
});

#endregion
var app = builder.Build();

app.UseRateLimiter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

