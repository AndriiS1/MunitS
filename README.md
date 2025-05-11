Tables population script:

```
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

