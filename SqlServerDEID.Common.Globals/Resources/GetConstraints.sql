SELECT
    [a].[table_view],
    [a].[object_type],
    [a].[constraint_type],
    [a].[constraint_name],
    [a].[details],
	CASE 
		WHEN [a].[constraint_type] IN ('Foreign key', 'Check constraint') THEN CONCAT('ALTER TABLE ', [a].[table_view], ' nocheck constraint ', QUOTENAME([a].[constraint_name]), ';')
		ELSE NULL
	END AS [disable_script],
	CASE 
		WHEN [a].[constraint_type] IN ('Foreign key', 'Check constraint') THEN CONCAT('ALTER TABLE ', [a].[table_view], ' check constraint ', QUOTENAME([a].[constraint_name]), ';')
		ELSE NULL
	END AS [enable_script]
FROM (
    SELECT
        QUOTENAME(SCHEMA_NAME([t].[schema_id])) + '.' + QUOTENAME([t].[name]) AS [table_view],
		[t].[object_id],
        CASE
            WHEN [t].[type] = 'U' THEN 'Table'
            WHEN [t].[type] = 'V' THEN 'View'
        END AS [object_type],
        CASE
            WHEN [c].[type] = 'PK' THEN 'Primary key'
            WHEN [c].[type] = 'UQ' THEN 'Unique constraint'
            WHEN [i].[type] = 1 THEN 'Unique clustered index'
            WHEN [i].[type] = 2 THEN 'Unique index'
        END AS [constraint_type],
        ISNULL([c].[name], [i].[name]) AS [constraint_name],
        SUBSTRING([D].[column_names], 1, LEN([D].[column_names]) - 1) AS [details]
    FROM [sys].[objects] AS [t]
    LEFT OUTER JOIN [sys].[indexes] AS [i]
        ON [t].[object_id] = [i].[object_id]
    LEFT OUTER JOIN [sys].[key_constraints] AS [c]
        ON [i].[object_id] = [c].[parent_object_id]
           AND [i].[index_id] = [c].[unique_index_id]
    CROSS APPLY (
        SELECT [col].[name] + ', '
        FROM [sys].[index_columns] AS [ic]
        INNER JOIN [sys].[columns] AS [col]
            ON [ic].[object_id]     = [col].[object_id]
               AND [ic].[column_id] = [col].[column_id]
        WHERE
            [ic].[object_id]    = [t].[object_id]
            AND [ic].[index_id] = [i].[index_id]
        ORDER BY [col].[column_id]
        FOR XML PATH('')
    ) AS [D]([column_names])
    WHERE
        [i].[is_unique]         = 1
        AND [t].[is_ms_shipped] <> 1
    UNION ALL
    SELECT
        QUOTENAME(SCHEMA_NAME([fk_tab].[schema_id])) + '.' + QUOTENAME([fk_tab].[name]) AS [foreign_table],
		[fk_tab].[object_id],
        'Table',
        'Foreign key',
        [fk].[name] AS [fk_constraint_name],
        SCHEMA_NAME([pk_tab].[schema_id]) + '.' + [pk_tab].[name]
    FROM [sys].[foreign_keys] AS [fk]
    INNER JOIN [sys].[tables] AS [fk_tab]
        ON [fk_tab].[object_id]             = [fk].[parent_object_id]
    INNER JOIN [sys].[tables] AS [pk_tab]
        ON [pk_tab].[object_id]             = [fk].[referenced_object_id]
    INNER JOIN [sys].[foreign_key_columns] AS [fk_cols]
        ON [fk_cols].[constraint_object_id] = [fk].[object_id]
    WHERE
        [fk].[is_not_trusted]  = 0
        AND [fk].[is_disabled] = 0
    UNION ALL
    SELECT
        QUOTENAME(SCHEMA_NAME([t].[schema_id])) + '.' + QUOTENAME([t].[name]),
		[t].[object_id],
        'Table',
        'Check constraint',
        [con].[name] AS [constraint_name],
        [con].[definition]
    FROM [sys].[check_constraints] AS [con]
    LEFT OUTER JOIN [sys].[objects] AS [t]
        ON [con].[parent_object_id]     = [t].[object_id]
    LEFT OUTER JOIN [sys].[all_columns] AS [col]
        ON [con].[parent_column_id]     = [col].[column_id]
           AND [con].[parent_object_id] = [col].[object_id]
    WHERE
        [con].[is_not_trusted]  = 0
        AND [con].[is_disabled] = 0
    UNION ALL
    SELECT
        QUOTENAME(SCHEMA_NAME([t].[schema_id])) + '.' + QUOTENAME([t].[name]),
		[t].[object_id],
        'Table',
        'Default constraint',
        [con].[name],
        [col].[name] + ' = ' + [con].[definition]
    FROM [sys].[default_constraints] AS [con]
    LEFT OUTER JOIN [sys].[objects] AS [t]
        ON [con].[parent_object_id]     = [t].[object_id]
    LEFT OUTER JOIN [sys].[all_columns] AS [col]
        ON [con].[parent_column_id]     = [col].[column_id]
           AND [con].[parent_object_id] = [col].[object_id]
) AS [a]
WHERE a.[object_id] = OBJECT_ID(@table_name)
	AND [a].[constraint_type] IN ('Check constraint', 'Foreign key')
ORDER BY
    [a].[table_view],
    [a].[constraint_type],
    [a].[constraint_name];


