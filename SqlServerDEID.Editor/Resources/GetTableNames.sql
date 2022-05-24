SELECT UPPER(CONCAT(QUOTENAME(OBJECT_SCHEMA_NAME(t.[object_id])), '.', QUOTENAME(t.[name]))) AS [TableName]
FROM [{{DB_NAME}}].[sys].[tables] AS [t]
WHERE [t].[is_ms_shipped] = 0
ORDER BY [TableName]
