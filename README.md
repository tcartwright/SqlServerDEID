NOTE: This documentation is in the process of being fleshed out. 

SqlServerDEID is an application that can DEID (De-Identify) sensitive PCI, HIPPA or GDPR data within your SQL Server database. There are two applications:

- SqlServerDEID.exe: Command line utility that uses a transform file to actually perform the DEID process. Run the application from a command window to see the current command line help.
- SqlServerDEID.Editor.exe: Windows application that can be used to create and test the transform files. 

The faker data is being supplied by the [Bogus library here](https://github.com/bchavez/Bogus).

Transforms:
A transform file connects to a single database on a server. It can be saved in either json or xml format. Pseudo format:

	- Database (properties: ServerName, Port, DatabaseName, CredentialsName, Locale, PreScript, PostScript, ScriptTimeout)
		- ScriptingImports
			- NameSpace 
		- Tables 
			- Table (properties: Name, DisableTriggers, DisableConstraints, PreScript, PostScript, ScriptTimeout)
				- Columns
					- Column (properties: Name)
						- Transforms 
							- Transform (properties: Transform, TransformType, WhereClause)


- Database
	- ServerName: The server name of the SQL Server.
	- Port: The port of the SQL Server, defaults to 1433.
	- DatabaseName: The database name.
	- CredentialsName: If missing or empty, then a trusted connection will be used. If supplied, it must match a generic credential name found in the windows [Credential Manager](https://support.microsoft.com/en-us/windows/accessing-credential-manager-1b5c916a-6a16-889f-8581-fc16e8165ac0).
	- Locale: This is the locale that will be used for the faker library. Please see [here](https://github.com/bchavez/Bogus#locales) for supported locales.
	- PreScript: A path to a SQL script that will be run before any table transforms are applied. Will not be used when testing from the transform test form. **
	- PostScript: A path to a SQL script that will be run after any table transforms are applied. Will not be used when testing from the transform test form. **
	- ScriptTimeout: The timeout in seconds before either the postscript or the prescript will throw a timeout exception.

- ScriptingImports (0 to many)
	- NameSpace: Addtional namespaces can be brought in for access to additional built in functions in the C# expressions. There are also custom namespaces the Bogus library supports for additonal functionality. See [here](https://github.com/bchavez/Bogus#api-extension-methods) for more information. For example, to be able to use the Faker.Person.Ssn() function you must import the namespace: Bogus.Extensions.UnitedStates.

- Table 
	- Name: The name of the database table. Must include schema.
	- DisableTriggers: If true will disable all triggers before the table transform, and re-enable them after.
	- DisableConstraints: If true will disable all constraints before the table transform, and re-enable them after. (TODO:) Will only re-enable constraints that were originally enabled. 
	- PreScript: A path to a SQL script that will be run before the table transforms are applied. Will not be used when testing from the transform test form. **
	- PostScript: A path to a SQL script that will be run after the table transforms are applied. Will not be used when testing from the transform test form. **
	- ScriptTimeout: The timeout in seconds before either the postscript or the prescript will throw a timeout exception.

- Column 
	- Name: The name of the column.

- Transform 
	- Transform: The actual transform to be applied to the column. Can either be:
		- C# expression. An editor can be used to construct the C# expression. For Faker API support see [here](https://github.com/bchavez/Bogus#bogus-api-support).
		- Path to a PowerShell file. **
	- TransformType: The type of expression to run. (expression, PowerShell)
	- WhereClause: A SQL where clause used to limit the rows that the transform is applied to.
		- The SQL is a cut down syntax. See [DataColumn.Expression](https://docs.microsoft.com/en-us/dotnet/api/system.data.datacolumn.expression?view=net-6.0) for more information.
		- When multiple transforms are applied to the same column then where clauses are required. 


** All paths to SQL files, and PowerShell files can be absolute or relative. Relative paths are relative to the file location of the transform file itself.


An example PowerShell file:

	[CmdletBinding()]
	param (
		[Parameter(Mandatory=$true)]
		$Faker, 
		[Parameter(Mandatory=$true)]
		$ColumnInfo, 
		[Parameter(Mandatory=$true)]
		$RowValues, 
		[Parameter()]
		$Male, 
		[Parameter(Mandatory=$true)]
		$Female
	)
	<#
		NOTE: only the very last output will be used for the column value. All others will be discarded.
	#>

	Write-Output $Faker.Name.FullName('Female')