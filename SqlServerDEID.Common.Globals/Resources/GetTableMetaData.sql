SELECT
    CONCAT(QUOTENAME(SCHEMA_NAME([t].[schema_id])), '.', QUOTENAME([t].[name])) AS [table_name],
    [c].[name] AS [column_name],
    [c].[type_name],
    [c].[max_length],
    [c].[precision],
    [c].[scale],
    [c].[is_nullable],
    [c].[is_identity],
    [c].[is_computed],
    [c].[is_pk],
    [c].[pk_ordinal]
FROM [sys].[tables] AS [t]
INNER JOIN (
    SELECT
        [c].[name],
        [c].[column_id],
        [c].[object_id],
        [typ].[type_name],
        CASE
            WHEN [typ].[type_name] LIKE '%char' THEN [c].[max_length]
            ELSE NULL
        END AS [max_length],
        CASE
            WHEN [typ].[type_name] IN ('decimal', 'float') THEN [c].[precision]
            ELSE NULL
        END AS [precision],
        CASE
            WHEN [typ].[type_name] IN ('decimal', 'float', 'datetime2', 'time', 'datetimeoffset') THEN [c].[scale]
            ELSE NULL
        END AS [scale],
        [c].[is_nullable],
        [c].[is_identity],
        [c].[is_computed],
        CASE
            WHEN [pk].[column_id] IS NOT NULL THEN 1
            ELSE 0
        END AS [is_pk],
        CASE
            WHEN [pk].[column_id] IS NOT NULL THEN [pk].[key_ordinal]
            ELSE null
        END AS [pk_ordinal]
    FROM [sys].[columns] AS [c]
    CROSS APPLY (SELECT TYPE_NAME([c].[system_type_id]) AS [type_name]) AS [typ]
    LEFT JOIN (
        SELECT
            [ic].[object_id],
            [ic].[column_id],
            [ic].[key_ordinal]
        FROM [sys].[indexes] AS [i]
        INNER JOIN [sys].[index_columns] AS [ic]
            ON [ic].[object_id]    = [i].[object_id]
               AND [ic].[index_id] = [i].[index_id]
        WHERE [i].[is_primary_key] = 1
    ) AS [pk]
        ON [pk].[object_id]     = [c].[object_id]
           AND [pk].[column_id] = [c].[column_id]
) AS [c]
    ON [c].[object_id] = [t].[object_id]
WHERE
    [t].[is_ms_shipped] = 0
    AND [t].[object_id] = OBJECT_ID(@table_name)
ORDER BY [c].[column_id];

