// استيراد مساحات الأسماء اللازمة للوصول إلى مكونات واجهة المستخدم
using EmployeeCardSystem.Components;

// بدء بناء تطبيق الويب وتجهيز الإعدادات الأساسية
var builder = WebApplication.CreateBuilder(args);

// تسجيل خدمة التقارير (ReportService) ليتمكن التطبيق من حقنها (Injection) في الصفحات عند الحاجة
builder.Services.AddScoped<EmployeeCardSystem.Services.ReportService>();

// إضافة خدمات Blazor Server مع تخصيص خيارات الاتصال (Hub)
builder.Services.AddServerSideBlazor().AddHubOptions(options =>
{
    // سطر مهم جداً: رفع الحد الأقصى لحجم البيانات المرسلة إلى 10 ميجابايت
    // نستخدم هذا لأن صور الموظفين قد تكون كبيرة الحجم عند رفعها عبر SignalR
    options.MaximumReceiveMessageSize = 10 * 1024 * 1024;
});

// إضافة الخدمات اللازمة لدعم مكونات Razor في المشروع
builder.Services.AddRazorComponents()
    // تفعيل خاصية المكونات التفاعلية على جهة السيرفر (Interactive Server)
    .AddInteractiveServerComponents();

// بناء كائن التطبيق (app) بعد الانتهاء من تعريف الخدمات
var app = builder.Build();

// إعداد "خط أنابيب" الطلبات (Middleware Pipeline)
// التحقق مما إذا كان التطبيق يعمل في بيئة تطوير أم إنتاج
if (!app.Environment.IsDevelopment())
{
    // استخدام صفحة خطأ مخصصة في حالة حدوث مشاكل في الإنتاج
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // تفعيل بروتوكول HSTS لزيادة أمان المتصفح في بيئة الإنتاج
    app.UseHsts();
}

// إعادة توجيه المستخدم لصفحة "غير موجود" مخصصة عند حدوث خطأ 404 مثلاً
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

// إجبار التطبيق على استخدام بروتوكول HTTPS المشفر
app.UseHttpsRedirection();

// تفعيل الحماية ضد هجمات تزوير الطلبات عبر المواقع (Antiforgery)
app.UseAntiforgery();

// السماح بالوصول إلى الملفات الثابتة (مثل الصور في wwwroot والملفات المضغوطة)
app.MapStaticAssets();

// ربط المكون الرئيسي (App) وبدء تشغيل واجهة المستخدم
app.MapRazorComponents<App>()
    // تفعيل وضع الرندرة التفاعلي للسيرفر ليتمكن الزر من تنفيذ كود C# فوراً
    .AddInteractiveServerRenderMode();

// تشغيل التطبيق والبدء في استقبال طلبات المستخدمين
app.Run();