using DostavaHrane.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(sgo => {

    var o = new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "Dostava Hrane API",
        Version = "v1",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
        {
            Email = "k_berisa@hotmail.com",
            Name = "Kristijan Berisa"
        },
        Description = "Ovo je dokumentacija za Dostava Hrane API",
        License = new Microsoft.OpenApi.Models.OpenApiLicense()
        {
            Name = "Edukacijska licenca"
        }
    };
    sgo.SwaggerDoc("v1", o);
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    sgo.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

});

builder.Services.AddCors(opcije =>
{
    opcije.AddPolicy("CorsPolicy",
        builder =>
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});




builder.Services.AddDbContext<DostavaHraneContext>(o =>
    o.UseSqlServer(
        builder.Configuration.
        GetConnectionString(name: "DostavaHraneContext")
        )
    );



var app = builder.Build();

app.UseSwagger(opcije =>
{
    opcije.SerializeAsV2 = true;
});
app.UseSwaggerUI(opcije =>
{
    opcije.ConfigObject.
    AdditionalItems.Add("requestSnippetsEnabled", true);
});


app.UseHttpsRedirection();


app.MapControllers();
app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseDefaultFiles();
app.UseDeveloperExceptionPage();
app.MapFallbackToFile("index.html");

app.Run();

