SELECT @@SERVERNAME AS [@server_name],
	0 AS [@port],
	DB_NAME() AS [@database_name],
	'' AS [@credentials_name], 
	'false' AS [@disable_triggers],
	'false' AS [@disable_constraints], 
	'en' AS [@locale], (
	SELECT CONCAT(QUOTENAME(SCHEMA_NAME(t.[schema_id])), '.', QUOTENAME([t].[name])) AS [@name], 
		(
			SELECT QUOTENAME(c.[name]) AS [@name], 
				[typ].[type_name] AS [@type_name],
				CASE WHEN [typ].[type_name] LIKE '%char' THEN [c].[max_length] ELSE NULL END AS [@max_length], 
				CASE WHEN [typ].[type_name] IN ('decimal', 'float') THEN [c].[precision] ELSE NULL END AS [@precision], 
				CASE WHEN [typ].[type_name] IN ('decimal', 'float', 'datetime2', 'time', 'datetimeoffset') THEN [c].[scale] ELSE NULL END AS [@scale], 
				--CASE WHEN [c].is_nullable = 1 THEN 'true' ELSE NULL END AS [@is_nullable],
				CASE WHEN [c].[is_identity] = 1 THEN 'true' ELSE NULL END AS [@is_identity],
				CASE WHEN [c].[is_computed] = 1 THEN 'true' ELSE NULL END AS [@is_computed],
				CASE WHEN pk.[column_id] IS NOT NULL THEN 'true' ELSE NULL END AS [@is_pk],
				CASE WHEN pk.[column_id] IS NOT NULL THEN pk.[key_ordinal] ELSE NULL END AS [@pk_ordinal]
				, (
					SELECT * FROM (
						SELECT 
							'' AS [@transform], 
							'expression' AS [@type], 
							'' AS [@where_clause] 
						UNION ALL
						SELECT 
							'' AS [@transform], 
							'file' AS [@type], 
							'' AS [@where_clause] 
					) t
					WHERE  [c].[is_identity] = 0
						   AND [c].[is_computed] = 0
						   AND pk.[column_id] IS NULL
					FOR XML PATH('Transform'), TYPE
				) AS [Transforms]
			FROM sys.[columns] AS [c]
			CROSS APPLY (SELECT TYPE_NAME(c.system_type_id) AS [type_name]) AS [typ]
			LEFT JOIN (
				SELECT 
					ic.[object_id], 
					ic.[column_id],
					ic.[key_ordinal]
				FROM sys.[indexes] AS [i] 
				INNER JOIN sys.[index_columns] AS [ic] 
					ON [ic].[object_id] = [i].[object_id] 
						AND ic.[index_id] = i.[index_id]
				WHERE i.[is_primary_key] = 1
			) pk 
				ON [pk].[object_id] = [c].[object_id] 
					AND [pk].[column_id] = [c].[column_id]
			WHERE c.[object_id] = t.[object_id]
			ORDER BY c.[column_id]
			FOR XML PATH('Column'), TYPE
		) AS [Columns]				
		, (
			SELECT * FROM (
				SELECT 
					'' AS [@file_name],
					'pre' AS [@type]
				UNION ALL
				SELECT 
					'' AS [@file_name],
					'post' AS [@type]
			) s
			FOR XML PATH('SqlScript'), TYPE
		) AS [SqlScripts]
	FROM sys.[tables] AS [t]
	WHERE t.[is_ms_shipped] = 0
		--AND t.[object_id] = OBJECT_ID('dbo.TableName')
	ORDER BY OBJECT_SCHEMA_NAME(t.[object_id]), 
		t.[name]
	FOR XML PATH('Table'), TYPE
	) AS [Tables]	
	, (
		SELECT * FROM (
			SELECT 
				'' AS [@file_name],
				'pre' AS [@type]
			UNION ALL
			SELECT 
				'' AS [@file_name],
				'post' AS [@type]
		) s
		FOR XML PATH('Script'), TYPE
	) AS [SqlScripts]
FOR XML PATH('Database')




