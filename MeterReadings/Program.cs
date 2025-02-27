using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
                   options
                       
                   #if DEBUG
                       .UseSeeding((context, _) => {
                           //add seeding logic here
                       } )
                       .UseAsyncSeeding(async (context, _, cancellationToken) => {
                           await Task.CompletedTask;
                           //add duplicate seeding logic here
                       })   
                   #endif
                       .UseSqlServer(builder.Configuration["ConnectionString"],
                       sqlServerOptionsAction: sqlOptions =>
                       {
                           sqlOptions.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
                           //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                           sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                       })
         
                   );

builder.Services.AddScoped<IMeterReadingService, MeterReadingService>();
builder.Services.AddSingleton<IRepository<Account>, AccountsListRepository>();
builder.Services.AddSingleton<IMeterReadingValidator, MeterReadingValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

//required until seeding sql Server is completed
#if DEBUG
    AccountsDatabaseSeed.SeedDatabase(app.Services.GetRequiredService<IRepository<Account>>());
#endif

app.MapMeterReadingEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.Run();

