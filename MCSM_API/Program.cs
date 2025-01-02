using MCSM_API.Configurations;
using MCSM_Data.Entities;
using MCSM_Utility.Settings;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using MCSM_Data.Mapping;
using Hangfire;
using MCSM_API.Hubs;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));

builder.Services.AddDbContext<McsmDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    }
);

builder.Services.AddSwaggerGenNewtonsoftSupport();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                          policy.WithOrigins(
                              "http://127.0.0.1:5173",
                              "https://hammer-asia.vercel.app",
                          "https://www.thaiplumtrial.xyz",
                          "http://localhost:5173");
                          policy.AllowCredentials();
                      });
});


builder.Services.AddHangfireServices(builder.Configuration);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR(options =>
{
    options.KeepAliveInterval = TimeSpan.FromSeconds(10); 
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
});

builder.Services.AddDependenceInjection();
builder.Services.AddSwagger();
builder.Services.AddAutoMapper(typeof(GeneralProfile));



var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
    app.UseSwaggerUI();
//}



app.UseJwt();


app.AddHangfireDashboard();
var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();
app.Services.AddHangfireJobs(recurringJobManager);



app.UseExceptionHandling();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chat-hub");
app.MapHub<AccountHub>("/list-user");
app.MapHub<ConversationHub>("/conversation");

app.Run();
