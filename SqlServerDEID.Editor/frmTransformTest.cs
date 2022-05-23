using SqlServerDEID.Common.Globals.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlServerDEID.Editor
{
    public partial class frmTransformTest : Form
    {
        private Database _database;

        public frmTransformTest()
        {
            InitializeComponent();
        }

        public frmTransformTest(Database database, DatabaseTable testTable) : this()
        {
            _database = new Database()
            {
                DatabaseName = database.DatabaseName,
                ServerName = database.ServerName,
                CredentialsName = database.CredentialsName,
                Locale = database.Locale,
                Port = database.Port,
                ScriptingImports = database.ScriptingImports
            };

            _database.Tables.Add(testTable);
        }
    }
}
