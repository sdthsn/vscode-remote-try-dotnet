using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Urls.Add("http://localhost:5000");

app.MapGet("/", () => "Hello World!");
app.MapGet("/{cityName}/weather", GetWeatherByCity);
app.MapGet("/image", () =>
{
    var relativeImagePath = "Essentials_Single_Jersey_Big_Logo_Tee_Black_IC9347_01_laydown.png";
    var wwwroot = builder.Environment.WebRootPath; // This will get the wwwroot path automatically.

    var imagePath = Path.Combine(wwwroot, "images", relativeImagePath);
    Debug.WriteLine(imagePath);

    if (File.Exists(imagePath))
    {
        string mimeType = "application/octet-stream";
        var imageBytes = File.ReadAllBytes(imagePath);
        string ext = Path.GetExtension(imagePath).ToLowerInvariant();
        Debug.WriteLine(ext);
        // You can add more file type mappings as needed
        if (ext == ".jpg" || ext == ".jpeg")
        {
            mimeType = "image/jpeg";
        }
        else if (ext == ".png")
        {
            mimeType = "image/png";
        }
        return Results.File(imageBytes, mimeType);
    }
    else
    {
        return Results.NotFound();
    }
});

app.Run();


Weather GetWeatherByCity(string cityName)
{
    app.Logger.LogInformation($"Weather requested for {cityName}.");
    var weather = new Weather(cityName);
    return weather;
}

public record Weather
{
    public string City { get; set; }

    public Weather(string city)
    {
        City = city;
        Conditions = "Cloudy";
        // Temperature here is in celsius degrees, hence the 0-40 range.
        Temperature = new Random().Next(0,40).ToString();
    }

    public string Conditions { get; set; }
    public string Temperature { get; set; }
}