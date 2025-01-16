namespace MunitS.Options.Storage;

public static class StorageOptionsExtensions
{
    public static void ConfigureOptions(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<StorageOptions>()
            .BindConfiguration(StorageOptions.Section)
            .Validate(ValidateOptions)
            .ValidateOnStart();
    }

    private static bool ValidateOptions(StorageOptions options)
    {
        if(!Directory.Exists(options.RootDirectory)) throw new ArgumentException($"Directory {options.RootDirectory} does not exist");

        return true;
    }   
}
