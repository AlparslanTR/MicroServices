using FreeCourse.Services.Catalog.Services.CategoryService;
using FreeCourse.Services.Catalog.Services.CourseService;
using FreeCourse.Services.Catalog.Settings;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// MongoDb bağlantı ayarlarını appsettings.json dosyasından alıyoruz.
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddSingleton<IDatabaseSettings>(sp =>
{
return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value; 
});
// Maplementasyonu için AutoMapper kütüphanesini kullanıyoruz.
builder.Services.AddAutoMapper(typeof(Program));

// Servis bağımlılıklarını ekliyoruz.
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICourseService, CourseService>();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
