using FluentValidation;
using GiftManagement_Version2.Data;
using GiftManagement_Version2.Mapping;
using GiftManagement_Version2.Middlewares;
using GiftManagement_Version2.Models;
using GiftManagement_Version2.Repositories;
using GiftManagement_Version2.Services;
using GiftManagement_Version2.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Authentication
string secrcetKey = builder.Configuration["JWT:SecretKey"];
byte[] secrcetKeyByte = Encoding.UTF8.GetBytes(secrcetKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {

            ValidateIssuer = false,
            ValidateAudience = false,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secrcetKeyByte),

            ClockSkew = TimeSpan.Zero
        };

    });

//Mapper
builder.Services.AddAutoMapper(typeof(Mappers));

//Validator
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GiftValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PromotionValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RoleValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CartValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TokenValidator>();

//Repository
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

//DI Container
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IGiftServices, GiftServices>();
builder.Services.AddScoped<IPromotionServices, PromotionServices>();
builder.Services.AddScoped<IRoleServices, RoleServices>();
builder.Services.AddScoped<ICartServices, CartServices>();
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<ITokenServices, TokenServices>();
builder.Services.AddScoped<IValidator<TokenModel>, TokenValidator>();
builder.Services.AddScoped<IOrderServices, OrderServices>();
builder.Services.AddScoped<IRankingServices, RankingServices>();

IConfiguration configuration = builder.Configuration;
builder.Services.AddDbContext<MyDBContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("cnn"));
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

//Middleware custom authorization
app.UseMiddleware<AuthorizationMiddleware>();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
