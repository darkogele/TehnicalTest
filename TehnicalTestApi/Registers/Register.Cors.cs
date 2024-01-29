namespace TechnicalTestApi.Registers;

public static partial class Register
{
    public static IServiceCollection RegisterCors(this IServiceCollection services)
    {
        services.AddCors(options => options.AddPolicy("AllowAll", builder =>
        {
            builder.AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                //.WithOrigins("https://localhost:4201") // If we need to add specific Origin Only 
                .SetIsOriginAllowed(_ => true);
        }));

        return services;
    }

    public static IApplicationBuilder UseCORS(this IApplicationBuilder app)
    {
        app.UseCors("AllowAll");

        return app;
    }
}