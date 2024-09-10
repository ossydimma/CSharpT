var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
#region Configure the HTTP pipleline and routes

if(!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseDefaultFiles(); // index.html, default.html, and so on.
app.UseStaticFiles();

app.MapGet("/hello", () => $"Environment is {app.Environment.EnvironmentName}");

#endregion


// Start the web server, host the website, and wait for requests.
app.Run(); // This is a thread-blocking call.
WriteLine("This executes after the web server has stopped!");

