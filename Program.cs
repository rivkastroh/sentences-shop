using ListSentence.interfases;
using ListSentence.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Middelware;
using Serilog;


//בניית בונה
var builder = WebApplication.CreateBuilder(args);

// בסוף הזרקתי בתוך המחלקות//הזרקה
// builder.Services.AddSingleton<ISetenceService, SentenceService>();
// builder.Services.AddSingleton<ITokenServices, UserTokenServices>();
// builder.Services.AddSingleton<IUserService, UserService>();
// //---------------------------------------------------------------

// הגדרת שירותי אימות
builder.Services.AddTokenService();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(cfg =>
{
    var tokenService = builder.Services.BuildServiceProvider().GetRequiredService<ITokenServices>();
    cfg.RequireHttpsMetadata = false;
    cfg.TokenValidationParameters = tokenService.GetTokenValidationParameters();
});

builder.Services.AddAuthorization(cfg =>
{
    cfg.AddPolicy("Admin", policy => policy.RequireClaim("type", "Admin"));
    cfg.AddPolicy("User", policy => policy.RequireClaim("type", "User", "Admin"));
    // cfg.AddPolicy("ClearanceLevel1", policy => policy.RequireClaim("ClearanceLevel", "1", "2"));
    // cfg.AddPolicy("ClearanceLevel2", policy => policy.RequireClaim("ClearanceLevel", "2"));
});

//הגדרת שירותים נוספים
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sentence", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    { 
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference 
                            { Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                        },
                        new string[] {}
                    }
                });
            });
//------------------------------------------------------------------------------------
builder.Services.AddSentenceService();
builder.Services.AddUserService();


//עבור לוגים
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.DateFormatPath(
        pathFormat: "Logs/log-{date:format=yyyy-MM-dd}.txt"
    )
    .CreateLogger();

builder.Host.UseSerilog();
//-------------------------------------

//בנית האפליקציה
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sentence v1"));
}

app.UseLoger();
app.UseFirstMiddelware();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();




//Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//הוספת משתנה ע"י האפליקציה
//ITokenServices tokenServices = app.Services.GetRequiredService<ITokenServices>();
//שימושים נוספים
// Configure the HTTP request pipeline.
//שינויים באימות
// app.Use(async (context, next) =>
// {
//     var jwtBearerOptions = new JwtBearerOptions
//     {
//         RequireHttpsMetadata = false,
//         TokenValidationParameters = tokenServices.GetTokenValidationParameters()
//     };
//     await next();
// });



//כאן לכאורה צריך להוציא מהערה
app.UseRouting();

//שימוש באימות
app.UseAuthentication();

