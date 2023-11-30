using FreeCourse.Services.Catalog.Services.CategoryService;
using FreeCourse.Services.Catalog.Services.CourseService;
using FreeCourse.Services.Catalog.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(x =>
{
    x.Filters.Add(new AuthorizeFilter());
});

// MongoDb baðlantý ayarlarýný appsettings.json dosyasýndan alýyoruz.
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddSingleton<IDatabaseSettings>(sp =>
{
return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value; 
});
// Maplementasyonu için AutoMapper kütüphanesini kullanýyoruz.
builder.Services.AddAutoMapper(typeof(Program));

// Servis baðýmlýlýklarýný ekliyoruz.
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICourseService, CourseService>();

// Claim bazlý yetkilendirme için gerekli olan policy'leri ekliyoruz.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.Authority = builder.Configuration["IdentityServerUrl"];
    opt.Audience = "resource_catalog";
    opt.RequireHttpsMetadata = false;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
