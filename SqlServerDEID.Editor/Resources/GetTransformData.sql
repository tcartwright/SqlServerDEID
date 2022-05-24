﻿IF OBJECT_ID('tempdb..{{TEMP_TABLE}}') IS NOT NULL DROP TABLE {{TEMP_TABLE}}; 
	
SELECT TOP (500) * 
INTO {{TEMP_TABLE}} 
FROM (
{{QUERY}}
) t; 

SELECT * FROM [{{TEMP_TABLE}}] AS [tt]