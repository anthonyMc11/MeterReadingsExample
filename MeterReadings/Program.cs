using MeterReadings.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
                   options
                       .UseSeeding((context, _) => {
                           //add seeding logic here
                       } )
                       .UseAsyncSeeding(async (context, _, cancellationToken) => {
                           //add duplicate seeding logic here
                       })
                       .UseSqlServer(builder.Configuration["ConnectionString"],
                       sqlServerOptionsAction: sqlOptions =>
                       {
                           sqlOptions.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
                           //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                           sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                       })
         
                   );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => {  
    return Results.Ok("Hello world");
});


app.Run();

