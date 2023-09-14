using DataTrackingSystem;
using DataTrackingSystem.API_Endpoints;
using DataTrackingSystem.Data.Models;
using DataTrackingSystem.Data.ViewModel.Input;
using DataTrackingSystem.DtoProfile;
using DataTrackingSystem.Repository;
using DataTrackingSystem.Repository.IRepository;
using DataTrackingSystem.Service;
using DataTrackingSystem.Service.IService;
using DataTrackingSystem.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// here we will write the connection string .............
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlDatabase"));
});
// here we will add the identity.....
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

//add scoped here -------------------
builder.Services.AddScoped<ITokenHandler, DataTrackingSystem.Service.TokenHandler>();  
builder.Services.AddScoped<ILoginRegisterService, LoginRegisterService>();
builder.Services.AddScoped<IShippingRepository, ShippingRepository>();
builder.Services.AddScoped<IInvitaionRepository, InvitationRepository>();
builder.Services.AddScoped<ITrackingRepository, TrackingRepository>();
builder.Services.AddScoped<UnitOfWork>();

// use singleton for the validation
builder.Services.AddSingleton<LoginValidator>();
builder.Services.AddSingleton<RegisterValidator>();
builder.Services.AddSingleton<ShippingValidator>();
builder.Services.AddSingleton<IEmailService, EmailService>();
//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();




// add mapper
builder.Services.AddAutoMapper(typeof(MappingProile));


var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JwtToken:SecretKey"));
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = true;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();




// add cors to the api.
builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: "AllowOrigin",
            builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                                    .AllowAnyHeader()
                                    .AllowAnyMethod();
            });
    });


builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    Array.Empty<string>()
                }
            });
    });
    var app = builder.Build();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    // here we will write the login api endpoint.
    app.MapGroup("/api").LoginRegisterApi().WithTags("Login Register Endpoint");
    // here we wil write the shipping api endpoint
    app.MapGroup("/api").ShippingApi().WithTags("Shipping Endpoint");

    app.MapGroup("/api").InvitationEndpointApi().WithTags("Invitation Endpoint").RequireAuthorization();



    // here we will call email code...
    app.MapGet("/email", (IEmailService email) =>
    {
        var mailrequest = new MailRequest()
        {
            ToEmail = "rguleria27@gmail.com",
            Subject = "email verification",
            Body = "hellossssss"


        };
        email.SendEmail(mailrequest);
        return Results.Ok("email send ");

    });



    app.UseCors("AllowOrigin");

    app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.Run();


