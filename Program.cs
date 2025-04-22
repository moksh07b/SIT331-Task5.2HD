using Gallery.Models;
using Gallery.Persistence;
using Gallery.Interfaces;
using Microsoft.EntityFrameworkCore;
using Gallery.Controllers;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using Gallery.Authentication;
using System.Security.Claims;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers().AddJsonOptions(options =>{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

builder.Services.AddDbContext<GalleryContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", default);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Member"));
    options.AddPolicy("ArtistOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Artist"));
});

builder.Services.AddScoped<ICommentRepository, CommentEF>();
builder.Services.AddScoped<IArtifactRepository, ArtifactEF>();
builder.Services.AddScoped<IArtistRepository, ArtistEF>();
builder.Services.AddScoped<IExhibitionRepository, ExhibitionEF>();
builder.Services.AddScoped<IUserRepository, UserEF>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gallery API",
        Description = "New backend service that provides resources for Gallery API.",
        Contact = new OpenApiContact
        {
            Name = "Moksh Bansal",
            Email = "mokshbansal07@gmail.com"
        },
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

});


builder.Services.AddOpenApi();

var app = builder.Build();
if(app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseHttpsRedirection();

app.MapGet("", () => "Hello World!" );

app.UseRouting();


app.UseAuthorization();

app.MapControllers();

app.Run();
