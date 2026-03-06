# MunitS

MunitS is a distributed object storage system, inspired by Amazon S3 and Cloudflare R2, built with .NET and Apache Cassandra. It provides a robust platform for managing large-scale data through a gRPC-based API, with support for multipart uploads, bucket management, and object versioning.

To see demo check [MunitS Hub Client](https://github.com/AndriiS1/munits-hub-client) and to get familiar with authentication system see [MunitS Hub Backend](https://github.com/AndriiS1/munits-hub)

## Features

These features work through gRPC interface:

- **Bucket Management**: Create, delete, and list buckets to organize your objects.
- **Object Storage**: Store and retrieve any kind of data.
- **Object Versioning**: Keep multiple versions of an object in a single bucket.
- **Scalable Metadata**: Utilizes Apache Cassandra for a highly available and scalable metadata backend.
- **Secure Uploads**: Uses signed URLs for direct part uploads via HTTP, ensuring that only authorized clients can upload data.

### Multipart upload

Efficiently upload large objects by splitting them into smaller parts.

This is the only feature that has exposed with https endpoint as it validates access with signature urls and redundant roundtrips can be very expensive.

Upload part (**PUT** `/objects/{objectId}/upload/{uploadId}/parts`): this endpoint is used to upload individual file parts as part of a multipart upload. It requires a signed URL, which can be obtained from the `GetPartUploadUrl` gRPC method. The URL includes query parameters for authorization and expiration, ensuring secure, direct-to-storage uploads.

## Getting Started

### Prerequisites

- .NET 9 SDK or later
- Docker (optionally for cassandra cluster)

### Database Setup

Before running the application, you need to create a keyspace in your Cassandra cluster. Then, execute the following CQL script to create the necessary tables.

1.  Run cassandra docker container:

```BASH
docker run --name cassandra -d -p 9042:9042 -v /home/user/MunitSCassandraCluster:/var/lib/cassandra cassandra:latest
```

`-v` value is for cassandra persistanse data, path the folder where you want to store DB data.

2.  Start cassandra interface to create keyspace and populate tables:

```BASH
docker exec -it cassandra cqlsh
```

3.  Create keyspace:

```BASH
CREATE KEYSPACE munitsspace
```

4. Populate tables:

```cql
DROP TABLE IF EXISTS bucket_counters;
CREATE TABLE bucket_counters (
    id UUID,
    objects_count COUNTER,
    size_in_bytes COUNTER,
    type_a_operations_count COUNTER,
    type_b_operations_count COUNTER,
    PRIMARY KEY ((id))
);

DROP TABLE IF EXISTS buckets_by_id;
CREATE TABLE buckets_by_id (
    id UUID,
    name TEXT,
    versioning_enabled BOOLEAN,
    versions_limit INT,
    created_at TIMESTAMP,
    PRIMARY KEY ((id))
);

DROP TABLE IF EXISTS buckets_by_name;
CREATE TABLE buckets_by_name (
    name TEXT,
    id UUID,
    PRIMARY KEY ((name))
);

DROP TABLE IF EXISTS division_counters;
CREATE TABLE division_counters (
    bucket_id UUID,
    type TEXT,
    id UUID,
    objects_count COUNTER,
    PRIMARY KEY ((bucket_id), type, id)
);

DROP TABLE IF EXISTS divisions_by_bucket_id;
CREATE TABLE divisions_by_bucket_id (
    bucket_id UUID,
    type TEXT,
    name TEXT,
    id UUID,
    objects_limit BIGINT,
    PRIMARY KEY ((bucket_id), type, id)
);

DROP TABLE IF EXISTS object_suffixes_by_parent_prefix;
CREATE TABLE object_suffixes_by_parent_prefix (
    id UUID,
    bucket_id UUID,
    parent_prefix TEXT,
    suffix TEXT,
    mime_type TEXT,
    created_at TIMESTAMP,
    type TEXT,
    PRIMARY KEY ((bucket_id), parent_prefix, type, suffix)
);

DROP TABLE IF EXISTS objects_by_upload_id;
CREATE TABLE objects_by_upload_id (
    id UUID,
    bucket_id UUID,
    upload_id UUID,
    division_id UUID,
    file_key TEXT,
    file_name TEXT,
    division_size_type TEXT,
    uploaded_at TIMESTAMP,
    initiated_at TIMESTAMP,
    upload_status TEXT,
    extension TEXT,
    mime_type TEXT,
    size_in_bytes BIGINT,
    custom_metadata MAP<TEXT, TEXT>,
    tags MAP<TEXT, TEXT>,
    PRIMARY KEY ((bucket_id), id, upload_id)
);

DROP TABLE IF EXISTS objects_by_file_key;
CREATE TABLE objects_by_file_key (
    id UUID,
    bucket_id UUID,
    file_key TEXT,
    created_at TIMESTAMP,
    PRIMARY KEY ((bucket_id), file_key)
);

DROP TABLE IF EXISTS parts_by_upload_id;
CREATE TABLE parts_by_upload_id (
    id UUID,
    bucket_id UUID,
    upload_id UUID,
    etag TEXT,
    number INT,
    PRIMARY KEY ((bucket_id), upload_id, number)
);

DROP TABLE IF EXISTS metrics_by_date;
CREATE TABLE metrics_by_date (
    bucket_id UUID,
    date DATE,
    type TEXT,
    id UUID,
    operation TEXT,
    time TIMESTAMP,
    PRIMARY KEY ((bucket_id), date, time)
) WITH CLUSTERING ORDER BY (date ASC, time ASC);
```

### Configuration

To successfully start the project you have to add env variables. Full example:

```json
{
  "Storage": {
    "RootDirectory": "/path/to/your/storage",
    "SignatureSecret": "your-strong-secret-for-signing-urls",
    "BaseUrl": "https://localhost:7055"
  },
  "DataBase": {
    "ContactPoints": "127.0.0.1",
    "Port": 9042,
    "KeySpace": "your_cassandra_keyspace_name" // munitsspace
  }
}
```

### Running the Application

1. Clone the repository:

```bash
git clone https://github.com/AndriiS1/MunitS.git
cd MunitS
```

2. Navigate to the main project directory:

```bash
cd src/MunitS
```

3. Run the application:

```BASH
dotnet run
```

The gRPC and HTTP services will now be running and accessible.

### gRPC Services

The API contracts are defined in the `.proto` files located in `src/MunitS.Protos/Protos`.

- **`BucketsService` (`bucket.proto`)**: Manages buckets and their metrics.
  - `CreateBucket`: Creates a new bucket.
  - `DeleteBucket`: Deletes an existing bucket.
  - `GetBucket`: Retrieves details for a specific bucket.
  - `GetBuckets`: Retrieves a list of buckets.
  - `GetBucketMetrics`: Fetches usage metrics for a bucket.
- **`ObjectsService` (`object.proto`)**: Manages objects and their versions.
  - `InitiateMultipartUpload`: Starts a new multipart upload and returns an `uploadId`.
  - `GetPartUploadUrl`: Generates a signed URL for uploading a part of a multipart upload.
  - `CompleteMultipartUpload`: Finalizes a multipart upload after all parts have been uploaded.
  - `AbortMultipartUpload`: Cancels a pending multipart upload.
  - `DeleteObject`: Deletes an object and all its versions.
  - `GetObject`: Retrieves an object's metadata and its versions.
  - `GetObjectsByPrefix`: Lists objects and common prefixes within a bucket.

# Links

- [MunitS Hub Client](https://github.com/AndriiS1/munits-hub-client)
- [MunitS Hub Backend](https://github.com/AndriiS1/munits-hub)
