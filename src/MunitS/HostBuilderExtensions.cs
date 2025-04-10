namespace MunitS;

public static class HostBuilderExtensions
{
    public static void ConfigureCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.WithOrigins()
                        .WithMethods(HttpMethods.Put)
                        .AllowAnyHeader();
                });
        });
    }
}
