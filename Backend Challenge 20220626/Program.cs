using Data;
using IoC;
using Jobs;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
