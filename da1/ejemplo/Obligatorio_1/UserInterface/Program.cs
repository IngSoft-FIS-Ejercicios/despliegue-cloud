using Blazored.LocalStorage;
using Domain;
using Logic;
using Repository;
using IRepository;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazorBootstrap();

// Add db context
builder.Services.AddDbContext<SqlContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));
builder.Services.AddScoped<IRepository<Comment>, RepositoryDBComment>();
builder.Services.AddScoped<IRepositoryTrash<Trashpaper>, RepositoryDBTrashpaper>();
builder.Services.AddScoped<IRepositoryPreFull<Notification>, RepositoryDBNotification>();
builder.Services.AddScoped<IRepositoryFull<PanelTask>, RepositoryDBPanelTask>();
builder.Services.AddScoped<IRepositoryFull<Panel>, RepositoryDBPanel>();
builder.Services.AddScoped<IRepositoryFull<Team>, RepositoryDBTeam>();
builder.Services.AddScoped<IRepositoryFull<Epic>, RepositoryDBEpic>();
builder.Services.AddScoped<IRepositoryUser<User>, RepositoryDBUser>();
builder.Services.AddBlazoredLocalStorage();

// Add services to the container.

builder.Services.AddScoped<UserLogic>();
builder.Services.AddScoped<TeamLogic>();
builder.Services.AddScoped<SessionLogic>();
builder.Services.AddScoped<PanelTaskLogic>();
builder.Services.AddScoped<PanelLogic>();
builder.Services.AddScoped<ImportLogic>();
builder.Services.AddScoped<GraphicReport>();
builder.Services.AddScoped<EpicLogic>();
builder.Services.AddScoped<TrashpaperLogic>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}




app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();