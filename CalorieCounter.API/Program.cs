using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SharedLibrary.Configurations;
using SharedLibrary.Services;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using CalorieCounter.API.Filters;
using CalorieCounter.Service.Validations;
using CalorieCounter.Service.Mapping;
using CalorieCounter.Repository;
using CalorieCounter.Core.Models;
using CalorieCounter.Core;
using CalorieCounter.API.Modules;

var builder = WebApplication.CreateBuilder(args);

//var builder = WebApplication.CreateBuilder(new WebApplicationOptions
//{
//    ApplicationName = typeof(Program).Assembly.FullName,
//    ContentRootPath = Directory.GetCurrentDirectory(),
//    EnvironmentName = Environments.Staging,
//    WebRootPath = "http://0.0.0.0:6363"
//});


// Canlýya alýrken alttaki 2 satýrý yorumdan çýkar. !!!
//builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());
//builder.WebHost.UseUrls("http://0.0.0.0:6363");




//builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<CityValidator>());



// Add services to the container.

builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<DenemeValidator>());

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standart Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type =SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();

});

builder.Services.AddScoped(typeof(NotFoundFilter<>));
builder.Services.AddAutoMapper(typeof(MapProfile));



builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });

});

builder.Services.AddIdentity<UserApp, IdentityRole>(Opt =>
{
    Opt.User.RequireUniqueEmail = true;
    Opt.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();




builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));

builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));




builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
{
    var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<CustomTokenOption>();
    opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience[0],
        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});



builder.Host.UseServiceProviderFactory
    (new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));


var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}
await Configure(app);



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Daha sonra yoruma al!!!!
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors(options =>
           options.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader());

app.Run();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();



async Task Configure(WebApplication host)
{
    using var scope = host.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        var dbContext = services.GetRequiredService<AppDbContext>();

        if (dbContext.Database.IsNpgsql())
        {
            dbContext.Database.Migrate();
        }
        var userManager = services.GetRequiredService<UserManager<UserApp>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await CalorieCounter.Repository.Seeds.DefaultRoles.SeedAsync(userManager, roleManager);
        await CalorieCounter.Repository.Seeds.DefaultUsers.SeedSuperAdminAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        //Log some error
        throw;
    }
}