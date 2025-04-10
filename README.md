Tables population script:

```
DROP TABLE IF EXISTS buckets_by_id;
CREATE TABLE buckets_by_id (
    id UUID,
    name TEXT,
    versioning_enabled BOOLEAN,
    versions_limit INT,
    objects_count BIGINT,
    size_in_bytes BIGINT,
    PRIMARY KEY ((id))
);

DROP TABLE IF EXISTS buckets_by_name;
CREATE TABLE buckets_by_name (
    name TEXT,
    id UUID,
    PRIMARY KEY ((name))
);

DROP TABLE IF EXISTS divisions_by_bucket_id;
CREATE TABLE divisions_by_bucket_id (
    bucket_id UUID,
    type TEXT,
    name TEXT,
    id UUID,
    objects_count BIGINT,
    objects_limit BIGINT,
    path TEXT,
    PRIMARY KEY ((bucket_id, type), id)
);

DROP TABLE IF EXISTS objects_by_file_key;
CREATE TABLE objects_by_file_key (
    id UUID,
    bucket_id UUID,
    version_id UUID,
    upload_id UUID,
    file_key TEXT,
    file_name TEXT,
    uploaded_at TIMESTAMP,
    initiated_at TIMESTAMP,
    extension TEXT,
    upload_status TEXT,
    path TEXT,
    PRIMARY KEY ((bucket_id), file_key, version_id)
) WITH CLUSTERING ORDER BY (version_id ASC);

DROP TABLE IF EXISTS objects_by_parent_prefix;
CREATE TABLE objects_by_parent_prefix (
    id UUID,
    bucket_id UUID,
    file_name TEXT,
    parent_prefix TEXT,
    initiated_at TIMESTAMP,
    uploaded_at TIMESTAMP,
    PRIMARY KEY ((bucket_id, parent_prefix), file_name)
);

DROP TABLE IF EXISTS folder_prefixes_by_parent_prefix;
CREATE TABLE folder_prefixes_by_parent_prefix (
    bucket_id UUID,	
    id UUID,
    parent_prefix TEXT,
    prefix TEXT,
    PRIMARY KEY ((bucket_id, parent_prefix), prefix)
);

DROP TABLE IF EXISTS metadata_by_object_id;
CREATE TABLE metadata_by_object_id (
    bucket_id UUID,
    version_id UUID,
    object_id UUID,
    content_type TEXT,
    size_in_bytes BIGINT,
    custom_metadata MAP<TEXT, TEXT>,
    tags MAP<TEXT, TEXT>,
    PRIMARY KEY ((bucket_id, object_id), version_id)
);
```

