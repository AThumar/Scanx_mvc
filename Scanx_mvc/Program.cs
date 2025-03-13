var builder = WebApplication.CreateBuilder(args);

// ✅ Add MVC Controllers
builder.Services.AddControllers();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// ✅ Allow Serving Static Files (like PDFs)
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// ✅ Map Controllers
app.MapControllers();

app.Run();
