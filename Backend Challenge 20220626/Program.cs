using System.Reflection;
using Data;
using IoC;
using Jobs;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

builder.Services.AddSwaggerGen(options =>
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Product API",
        Description = "An ASP.NET Core Web API for viweing products from OpenFoodFacts",
        Contact = new OpenApiContact
        {
            Name = "Júnior Nasicmento",
            Email = "junior.nascm@gmail.com"
        },
        License = new OpenApiLicense
        {
            Name = "GPL-3.0",
            Url = new Uri("https://www.gnu.org/licenses/gpl-3.0.txt")
        }
    })
);

builder.Services.Configure<OpenFoodFactsDataBaseSettings>(builder.Configuration.GetSection("OpenFoodFactsDatabaseSettings"));

builder.Services.AddLocalServices(builder.Configuration);

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    var jobKey = new JobKey("ScrappingJob");
    q.AddJob<ScrappingJob>(options => options.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("ScrappingJob-trigger")
        .WithCronSchedule(builder.Configuration.GetSection("JobsConfiguration:CronTime").Value!)
        .StartNow()
    );
});

builder.Services.AddQuartzServer(options => options.WaitForJobsToComplete = true);

var app = builder.Build();

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
