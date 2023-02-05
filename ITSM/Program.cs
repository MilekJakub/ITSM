using ITSM.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BoardsContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Main")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.Run();

#region Comments

//options =>
//{
//    options.SwaggerDoc("1.0.0", new OpenApiInfo
//    {
//        Version = "1.0.0",
//        Title = "ITSM",
//        Description = "IT Service Managment tool for reporting bugs",
//        Contact = new OpenApiContact()
//        {
//            Name = "Swagger Codegen Contributors",
//            Url = new Uri("https://github.com/swagger-api/swagger-codegen"),
//            Email = "apiteam@swagger.io"
//        },
//        TermsOfService = new Uri("http://swagger.io/terms/")
//    });
//}

#endregion