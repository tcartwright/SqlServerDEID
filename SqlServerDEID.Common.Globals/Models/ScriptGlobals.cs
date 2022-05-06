using Bogus;
using Bogus.DataSets;

namespace SqlServerDEID.Common.Globals.Models
{
    /// <summary>
    /// This class will be used to pass globals into the C# scriptlets, and parameters into the PowerShell scripts
    /// </summary>
    public class ScriptGlobals
    {
        private readonly string _locale;

        public ScriptGlobals(string locale = "en")
        {

            _locale = locale;
            Faker = new Faker(_locale);
            Male = new GenderedPerson(Name.Gender.Male, _locale);
            Female = new GenderedPerson(Name.Gender.Female, _locale);
            Column = new ColumnInfo();
        }

        public string DatabaseName { get; set; }
        public string TableName { get; set; }
        public ColumnInfo Column { get; set; }
        public Faker Faker { get; }
        public dynamic RowValues { get; set; }
        public GenderedPerson Female { get; }
        public GenderedPerson Male { get; }
        public string Locale { get { return _locale; } }
    }
}
