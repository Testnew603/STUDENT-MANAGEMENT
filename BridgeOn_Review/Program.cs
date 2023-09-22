using BridgeOn_Review.Services.Admin;
using BridgeOn_Review.Services.Advisor;
using BridgeOn_Review.Services.Attendance;
using BridgeOn_Review.Services.Batch;
using BridgeOn_Review.Services.Domain;
using BridgeOn_Review.Services.Leave;
using BridgeOn_Review.Services.Login;
using BridgeOn_Review.Services.Mentor;
using BridgeOn_Review.Services.Project;
using BridgeOn_Review.Services.Review;
using BridgeOn_Review.Services.Reviewer;
using BridgeOn_Review.Services.Student;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

builder.Services.AddScoped<IBatchServices, BatchServices>();
builder.Services.AddSingleton<IAdvisorService, AdvisorServices>();
builder.Services.AddSingleton<IAttendanceService, AttendanceServices>();
builder.Services.AddSingleton<IDomainServices, DomainServices>();
builder.Services.AddSingleton<ILeaveServices, LeaveServices>();
builder.Services.AddSingleton<IMentorServices, MentorServices>();
builder.Services.AddSingleton<IProjectServices, ProjectServices>();
builder.Services.AddSingleton<IReviewerServices, ReviewerServices>();
builder.Services.AddSingleton<IReviewServices, ReviewServices>();
builder.Services.AddSingleton<IStudentServices, StudentServices>();
builder.Services.AddSingleton<ILoginServices, LoginServices>();
builder.Services.AddSingleton<IAdminServices, AdminServices>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
