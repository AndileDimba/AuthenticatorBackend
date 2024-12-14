using AuthenticatorBackend.Data;
using AuthenticatorBackend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(); // Add this if not already present

// Add Authorization and Authentication services
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// Add CORS services if you'll be calling from Vue.js
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173") // Update this with your Vue.js app URL
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Add your database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add your services
builder.Services.AddScoped<UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowVueApp");

// Add routing and authorization middleware in the correct order
app.UseRouting();
app.UseAuthentication(); // Add this before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();