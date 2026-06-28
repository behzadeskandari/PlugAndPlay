namespace Framework.Abstractions;

public interface IPluginMigration
{
    Task<Result> MigrateAsync(CancellationToken cancellationToken = default);
}
