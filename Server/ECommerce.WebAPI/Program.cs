using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Core;
using ECommerce.Business;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ECommerceWebAPI;
using Microsoft.AspNetCore.Diagnostics;
using Npgsql;
using Microsoft.OpenApi.Models;
using Ecommerce.WebAPI.src.Authorization;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy=>
        {
                policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });  
});

builder.Services.AddControllers();
builder.Services.Configure<RouteOptions>(options=>options.LowercaseUrls=true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddSwaggerGen(
//     options =>
//     {
//         options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
//         {
//             Description = "Bearer token authentication",
//             Name = "Authorization", 
//             In = ParameterLocation.Header,
//             Scheme = "Bearer"
//         }
//         );
//         // options.OperationFilter<SecurityRequirementsOperationFilter>();
//     }
// );

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepo, UserRepo>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepo, ProductRepo>(); 

builder.Services.AddScoped<ICategoryService,CategoryService>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();

builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddScoped<IPurchaseRepo,PurchaseRepo>();

builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IReviewRepo,ReviewRepo>();


builder.Services.AddScoped<IPasswordHashRepo, PasswordHashRepo>();
builder.Services.AddScoped<IAuthService, AuthService>(); 
builder.Services.AddScoped<ITokenService, TokenService>(); 

// builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.MapEnum<Role>();
dataSourceBuilder.MapEnum<Status>();
var dataSource = dataSourceBuilder.Build();

// add database context service
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(dataSource)
           .UseSnakeCaseNamingConvention()
           .AddInterceptors(new TimeStampInterceptor());
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin"));
    options.AddPolicy("CustomerPolicy", policy =>
        policy.RequireRole("Customer"));
    // options.AddPolicy("AdminOrOwner", policy => policy.Requirements.Add(new AdminOrOwnerRequirement()));
});


var app = builder.Build();
app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
