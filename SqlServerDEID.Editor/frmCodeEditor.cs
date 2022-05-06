using CDS.CSharpScripting;
using SqlServerDEID.Common.Globals.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlServerDEID.Editor
{
    public partial class frmCodeEditor : Form
    {
        public delegate string Dump();
        private readonly StringComparer _stringComparer = StringComparer.OrdinalIgnoreCase;
        private Type[] _types = null;

        private readonly List<Type> _typeList = new List<Type>  
            {
                typeof(int),
                typeof(Task),
                typeof(ArrayList),
                typeof(List<>),
                typeof(ASCIIEncoding),
                typeof(Enumerable),
                typeof(Bogus.Faker),
                typeof(Bogus.DataSets.Name),
                typeof(ExpandoObject),
                typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo)
            };

        CompiledScript _compiledScript;

        public frmCodeEditor()
        {
            InitializeComponent();
        }

        private void frmCodeEditor_Load(object sender, EventArgs e)
        {
            var assembly = typeof(Bogus.Faker).Assembly;
            var allTypes = assembly.GetTypes();
            var types = allTypes.Where(t => t.IsClass && !string.IsNullOrWhiteSpace(t.Namespace) && Regex.IsMatch(t.Namespace, "Bogus.Extensions", RegexOptions.IgnoreCase)).ToList();
            _typeList.AddRange(types);

            _types = _typeList.ToArray();
            codeEditor1.CDSInitialize(_types, _types, typeof(ScriptGlobals));
            codeEditor1.CDSScript = Properties.Resources.InitScript;
            codeEditor1.CDSScriptChanged += CodeEditor1_CDSScriptChanged;
        }

        private void CodeEditor1_CDSScriptChanged(object sender, EventArgs e)
        {
            _compiledScript = null;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (_compiledScript == null)
            {
                _compiledScript = ScriptCompiler.Compile<object>(codeEditor1.CDSScript, _types, _types, typeof(ScriptGlobals));
            }
            outputPanel1.CDSClear();
            if (_compiledScript.CompilationOutput.ErrorCount == 0)
            {
                var scriptGlobals = new ScriptGlobals
                {
                    RowValues = new CustomExpandoObject()
                };
                scriptGlobals.RowValues.FirstName = "Tim";
                scriptGlobals.RowValues.MiddleName = null;
                scriptGlobals.RowValues.LastName = "Cartwright";
                scriptGlobals.RowValues.DOB = DateTime.Parse("1/1/1955");

                using (var console = new ConsoleOutputHook(msg => outputPanel1.CDSWrite(msg)))
                {
                    try
                    {
                        ScriptRunner.Run(
                            compiledScript: _compiledScript,
                            globals: scriptGlobals);
                    }
                    catch (Exception exception)
                    {
                        outputPanel1.CDSClear();
                        outputPanel1.CDSWriteLine(exception.ToString());
                    }
                }
            }
            else
            {
                outputPanel1.CDSWrite(String.Join("\r\n", _compiledScript.CompilationOutput.Messages));
            }
        }
    }
}
