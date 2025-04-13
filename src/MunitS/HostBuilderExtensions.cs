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
                    policy.WithMethods(HttpMethods.Put)
                        .AllowAnyHeader()
                        .AllowAnyOrigin();
                });
        });
    }
}
