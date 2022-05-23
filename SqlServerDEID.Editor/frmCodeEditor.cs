using CDS.CSharpScripting;
using SqlServerDEID.Common.Globals.Models;
using SqlServerDEID.Editor.Properties;
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
        private CompiledScript _compiledScript;
        private readonly ScriptGlobals _scriptGlobals;
        private string _transform;
        private bool _isTested = false;

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
        public string Transform { get => _transform; }

        public frmCodeEditor(ScriptGlobals scriptGlobals, string transform) : this()
        {
            _scriptGlobals = scriptGlobals;
            _transform = transform;
        }

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
            if (string.IsNullOrWhiteSpace(_transform))
            {
                codeEditor1.CDSScript = Resources.ExampleScript;
            }
            else
            {
                codeEditor1.CDSScript = Resources.ScriptTemplate.Replace("<<TRANSFORM>>", _transform);
            }
            codeEditor1.CDSScriptChanged += CodeEditor1_CDSScriptChanged;
            Cursor.Current = Cursors.Default;
        }

        private void CodeEditor1_CDSScriptChanged(object sender, EventArgs e)
        {
            _compiledScript = null;
            _isTested = false;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (_compiledScript == null)
                {
                    _compiledScript = ScriptCompiler.Compile<object>(codeEditor1.CDSScript, _types, _types, typeof(ScriptGlobals));
                }
                outputPanel1.CDSClear();

                if (_compiledScript.CompilationOutput.ErrorCount == 0)
                {
                    using (var console = new ConsoleOutputHook(msg => outputPanel1.CDSWrite(msg)))
                    {
                        try
                        {
                            ScriptRunner.Run(
                                compiledScript: _compiledScript,
                                globals: _scriptGlobals);
                            _isTested = true;
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
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_isTested)
            {
                if (MessageBox.Show(this,
                    "The current script is not tested. You must test scripts before saving.\r\n\r\nSelect Cancel to return to the script editor, and test the script.\r\n\r\nSelect Ok if you wish to exit without saving the script.",
                    "Un-tested script",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
                    this.DialogResult = DialogResult.Abort;
                }
                return;
            }
            var script = codeEditor1.CDSScript;

            var matches = Regex.Matches(script, @"Console\.WriteLine\((.*?)\);");

            var lastMatch = matches[matches.Count - 1];

            _transform = lastMatch.Groups[1].Value;

            this.DialogResult = DialogResult.OK;
        }

        private void btnLoadExample_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(codeEditor1.CDSScript))
            {
                if (MessageBox.Show(this,
                    "Loading the example will overwrite the current script. Do you wish to continue?",
                    "Load Example",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            codeEditor1.CDSScript = Resources.ExampleScript;
        }
    }
}
