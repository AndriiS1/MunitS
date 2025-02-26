using Cassandra;
namespace MunitS.Infrastructure.Data;

public class CassandraConnector
{
    private readonly ISession _session;

    public CassandraConnector(string contactPoints, int port, string keyspace)
    {
        var cluster = Cluster.Builder()
            .AddContactPoint(contactPoints)
            .WithPort(port)
            .Build();
        _session = cluster.Connect(keyspace);
    }

    public ISession GetSession() => _session;
}
