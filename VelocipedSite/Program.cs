using System.Net;
using Microsoft.AspNetCore.Mvc;
using VelocipedSite.ActionFilters;
using VelocipedSite.DAL.Extensions;
using VelocipedSite.HostedServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews()
    .AddMvcOptions(options =>
    {
        options.Filters.Add<ExceptionFilterAttribute>();
        options.Filters.Add(new ResponseTypeAttribute((int)HttpStatusCode.BadRequest));
        options.Filters.Add(new ResponseTypeAttribute((int)HttpStatusCode.InternalServerError));
        options.Filters.Add(new ProducesResponseTypeAttribute((int)HttpStatusCode.OK));
    });

builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(x => x.FullName);
});
builder.Services
    .AddDalInfrastructure(builder.Configuration)
    .AddDalRepositories();
builder.Services.AddHostedService<ExpiredTokenCleanerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");
app.MigrateUp();
app.Run();