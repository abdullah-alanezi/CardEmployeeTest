using EmployeeCard.Application.Features.Employees.Queries;
using EmployeeCard.Application.Interfaces;
using EmployeeCard.Persistence.Database;
using EmployeeCard.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// 1. إضافة الخدمات الأساسية
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// 2. تسجيل الـ Repository (ضروري جداً لعمل الـ Controller)
// تم الإبقاء عليه ليرتبط الـ Interface بالتنفيذ اليدوي الجديد
builder.Services.AddScoped<IEmployeeCardRepo, EmployeeCardRepo>();
// تسجيل MediatR والبحث عن الـ Handlers في مشروع Application
builder.Services.AddMediatR(cfg =>
     cfg.RegisterServicesFromAssembly(typeof(GetEmployeeCardQuery).Assembly));
// 3. إعداد الـ CORS (للسماح لـ Blazor بالوصول للـ API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorOrigin", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
// Add this in Program.cs
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// 4. إعداد خط المعالجة (Middleware Pipeline)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// تفعيل الـ CORS قبل الـ Authorization
app.UseCors("AllowBlazorOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();