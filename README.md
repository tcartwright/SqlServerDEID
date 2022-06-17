# SqlServerDEID

NOTE: This documentation is in the process of being fleshed out. 

SqlServerDEID is an application that can DEID (De-Identify) sensitive PCI, HIPPA or GDPR data within your SQL Server database. There are two applications:

- SqlServerDEID.exe: Command line utility that uses a transform file to actually perform the DEID process. Run the application from a command window to see the current command line help.
- SqlServerDEID.Editor.exe: Windows application that can be used to create and test the transform files. The editor is not required to edit transform files, but it greatly eases doing so.

The fake data is being supplied by the [Bogus library here](https://github.com/bchavez/Bogus).

## Transform file structure:
A transform file connects to a single database on a server. It can be saved in either json or xml format. Pseudo format:

	- Database (properties: ServerName, Port, DatabaseName, CredentialsName, Locale, PreScript, PostScript, ScriptTimeout)
		- ScriptingImports (0 to many)
			- NameSpace 
		- Tables (1 to many)
			- Table (properties: Name, DisableTriggers, DisableConstraints, PreScript, PostScript, ScriptTimeout)
				- Columns (1 to many)
					- Column (properties: Name)
						- Transforms (0 to many)
							- Transform (properties: Transform, TransformType, WhereClause)


## Transform object properties
- Database
    - ServerName: The server name of the SQL Server.
	- Port: The port of the SQL Server, defaults to 1433.
	- DatabaseName: The database name.
	- CredentialsName: If missing or empty, then a trusted connection will be used. If supplied, it must match a generic credential name found in the windows [Credential Manager](https://support.microsoft.com/en-us/windows/accessing-credential-manager-1b5c916a-6a16-889f-8581-fc16e8165ac0).
	- Locale: This is the locale that will be used for the faker library. Please see [here](https://github.com/bchavez/Bogus#locales) for supported locales.
	- PreScript: A path to a SQL script that will be run before any table transforms are applied. Will not be used when testing from the transform test form. **
	- PostScript: A path to a SQL script that will be run after any table transforms are applied. Will not be used when testing from the transform test form. **
	- ScriptTimeout: The timeout in seconds before either the postscript or the prescript will throw a timeout exception.

- ScriptingImports 
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
		- The SQL for the where clause is a cut down syntax. See [DataColumn.Expression](https://docs.microsoft.com/en-us/dotnet/api/system.data.datacolumn.expression?view=net-6.0#expression-syntax) for more information.
		- When multiple transforms are applied to the same column then where clauses are required. 

## C# Expression Examples:
- Faker.Name.FullName(Name.Gender.Female)
	- Generates a full name for a female. Can pass in Male or just remove the gender parameter to generate a random name.
- Faker.Date.Past(70, DateTime.Now.AddYears(-18)) 
	- Generates a birthdate between 80 and 18 years of age
- Faker.Person.Ssn() 
	- Requires the Bogus.Extensions.UnitedStates namespace to be imported. Generates a US SSN.
- String.Concat(Male.FullName, "<", Male.Email, ">") 
	- Generates a email address with fullname. Concatenates mulitple faker values. 
- Faker.Address.ZipCode("#####") 
	- Generates a five digit zip code using a format

## PowerShell file example:
	[CmdletBinding()]
	param (
		[Parameter(Mandatory=$true)]
		$Faker, 
		[Parameter(Mandatory=$true)]
		$Column, 
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

## Objects available within both the C# and PowerShell
- Faker: Object that can be used to call any of the [Faker API methods](https://github.com/bchavez/Bogus#bogus-api-support) to generate fake data to replace the real data with.
- Column: Object that represents the information about the database table column currently being transformed. Can be possibly used within a PS script to branch off name. 
- RowValues: No intellisense is available for this object. It is a dynamic object that will hold the current row values from the row being transformed. Transforms are applied by column and then by row. If you wish to use the column value of a column **after** its been transformed, you must ensure that the column transforms occur in the proper order. It is not possible to re-arrange the column transform order in the editor. Changing the order has to be done manually. In the below example, the fullname must occur **after** the other name columns used so as to retrieve the new fake values. If placed prior to the other name columns then it would get the original sensitive data.
    - Example (this is a contrived example. The fullname column would normally be a computed column): 
<pre>
    &lt;Column Name="[fname1]">
		&lt;Transforms>
			&lt;Transform Transform="Faker.Name.FirstName().ToUpper()" TransformType="expression" WhereClause="" />
		&lt;/Transforms>
	&lt;/Column>
	&lt;Column Name="[middle1]">
		&lt;Transforms>
			&lt;Transform Transform="Faker.Name.LastName().Substring(0, 1).ToUpper()" TransformType="expression" WhereClause="[Middle1] IS NOT NULL AND LEN([Middle1]) = 1" />
			&lt;Transform Transform="Faker.Name.LastName().ToUpper()" TransformType="expression" WhereClause="[Middle1] IS NOT NULL AND LEN([Middle1]) &gt; 1" />
		&lt;/Transforms>
	&lt;/Column>
	&lt;Column Name="[lname1]">
		&lt;Transforms>
			&lt;Transform Transform="Faker.Name.LastName().ToUpper()" TransformType="expression" WhereClause="" />
		&lt;/Transforms>
	&lt;/Column>
	&lt;Column Name="[fullname]">
		&lt;Transforms>
			&lt;Transform Transform="String.Concat(RowValues.fname1, &quot; &quot;, RowValues.middle1, &quot; &quot;, RowValues.lname1)" TransformType="expression" WhereClause="[middle1] IS NOT NULL" />
			&lt;Transform Transform="String.Concat(RowValues.fname1, &quot; &quot;, RowValues.lname1)" TransformType="expression" WhereClause="[middle1] IS NULL" />
		&lt;/Transforms>
	&lt;/Column>
</pre>
- Male: A gendered person where all of the personal data is co-related.
- Female: A gendered person where all of the personal data is co-related.

### Miscellaneous information
** All paths to SQL files, and PowerShell files can be absolute or relative. Relative paths are relative to the file location of the transform file itself.
