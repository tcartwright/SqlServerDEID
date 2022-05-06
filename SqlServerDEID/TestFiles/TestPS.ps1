[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)]
    $Faker, 
    [Parameter(Mandatory=$true)]
    $ColumnInfo, 
    [Parameter(Mandatory=$true)]
    $RowValues
)
<#
    NOTE: only the very last output will be used for the column value. All others will be discarded.
#>

<#
    This is the column info class, it will contain info about the current column being transformed, which can allow for the same script to handle multiple columns
    public class ColumnInfo
    {
        public string Name { get; internal set; }
        public string DataType { get; internal set; }
        public short MaxLength { get; internal set; }
        public byte Precision { get; internal set; }
        public byte Scale { get; internal set; }
    }
#>

Write-Host "I Love Lamp"

Write-Verbose "I Love Lucy"

Write-Output "$($ColumnInfo.Name) $($ColumnInfo.DataType) $($ColumnInfo.MaxLength)"

# the $RowValues object contains all of the values from the row, all of the columns can be accessed as properties 
Write-Output "$($RowValues.FirstName) $($RowValues.LastName)"

# the faker object comes from here https://github.com/bchavez/Bogus and can be used to implement any of the faker methods it has
Write-Output $Faker.Name.FullName('Female')




