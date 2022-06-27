﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SqlServerDEID.Editor.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SqlServerDEID.Editor.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Additional namespaces can be imported to provide access to additional classes or 
        ///functions in the C# expressions or the PowerShell scripts.
        ///
        ///These namespaces will be brought in by default:
        ///
        ///- Bogus
        ///- Bogus.DataSets
        ///- System
        ///- System.Text
        ///- System.Text.RegularExpressions.
        /// </summary>
        internal static string AddionalNameSpaces {
            get {
                return ResourceManager.GetString("AddionalNameSpaces", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A generic credential must be created in the Credential Manager to be available in this dropdown.
        ///
        ///Use &quot;Trusted Connection&quot; to connect as the currently logged on user..
        /// </summary>
        internal static string Credentials {
            get {
                return ResourceManager.GetString("Credentials", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to If enabled all constraints that are enabled will be disabled before running any DEID transforms against the table. All previouly disabled constraints will be re-enabled afterwards..
        /// </summary>
        internal static string disableconstraints_column {
            get {
                return ResourceManager.GetString("disableconstraints.column", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to If enabled all triggers will be disabled before running any DEID transforms against the table. All triggers will be re-enabled afterwards..
        /// </summary>
        internal static string disabletriggers_column {
            get {
                return ResourceManager.GetString("disabletriggers.column", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ////* 
        ///This https://github.com/bchavez/Bogus is the faker library provided. Please refer to its documentation for more information about how to use the various faker methods or properties.
        ///
        ///Notes: 
        ///	- There must be at least one Console.WriteLine(...)
        ///	- The contents of the very last Console.WriteLine(...) will be used as the contents of the expression when editing is completed
        ///		- All other content will be discarded.
        ///		- Do not use the parameters overload of the last Console.WriteLine
        ///	- Keep express [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ExampleScript {
            get {
                return ResourceManager.GetString("ExampleScript", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT UPPER(CONCAT(QUOTENAME(OBJECT_SCHEMA_NAME(t.[object_id])), &apos;.&apos;, QUOTENAME(t.[name]))) AS [TableName]
        ///FROM [{{DB_NAME}}].[sys].[tables] AS [t]
        ///WHERE [t].[is_ms_shipped] = 0
        ///ORDER BY [TableName]
        ///.
        /// </summary>
        internal static string GetTableNames {
            get {
                return ResourceManager.GetString("GetTableNames", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IF OBJECT_ID(&apos;tempdb..{{TEMP_TABLE}}&apos;) IS NOT NULL DROP TABLE {{TEMP_TABLE}}; 
        ///	
        ///SELECT TOP (@rows) * 
        ///INTO {{TEMP_TABLE}} 
        ///FROM (
        ///{{QUERY}}
        ///) t; 
        ///
        ///SELECT * FROM [{{TEMP_TABLE}}] AS [tt].
        /// </summary>
        internal static string GetTransformData {
            get {
                return ResourceManager.GetString("GetTransformData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This column determines whether or not the column will appear in the transform document, and also be available in the RowValues object..
        /// </summary>
        internal static string isselected_column {
            get {
                return ResourceManager.GetString("isselected.column", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The locale to use when generating faker data..
        /// </summary>
        internal static string Locale {
            get {
                return ResourceManager.GetString("Locale", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The name of the database object..
        /// </summary>
        internal static string name_column {
            get {
                return ResourceManager.GetString("name.column", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The port to connect to SQL Server on. 0 and 1433 are synonymous..
        /// </summary>
        internal static string PortNumber {
            get {
                return ResourceManager.GetString("PortNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This sql script will be run after all table transforms are run. A fully qualified or relative path can be used. 
        ///
        ///All relative paths will be relative to the transform configuration file..
        /// </summary>
        internal static string PostScript {
            get {
                return ResourceManager.GetString("PostScript", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This sql script will be run before all table transforms are run. A fully qualified or relative path can be used. 
        ///
        ///All relative paths will be relative to the transform configuration file..
        /// </summary>
        internal static string PreScript {
            get {
                return ResourceManager.GetString("PreScript", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Gets the wait time (in seconds) before terminating the attempt to execute the PreScript or PostScript and generating an error..
        /// </summary>
        internal static string Scriptimeout {
            get {
                return ResourceManager.GetString("Scriptimeout", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///
        ///Console.WriteLine(&lt;&lt;TRANSFORM&gt;&gt;);.
        /// </summary>
        internal static string ScriptTemplate {
            get {
                return ResourceManager.GetString("ScriptTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Allows for testing of the transforms without making any actual table changes. All changes are tested against a global temp table..
        /// </summary>
        internal static string testtransform_column {
            get {
                return ResourceManager.GetString("testtransform.column", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The transform can either be a C# expression or a path to a powershell file. If pointing to a powershell file path, then make the path relative to where the transform file will be saved, or use an absolute path..
        /// </summary>
        internal static string transform_column {
            get {
                return ResourceManager.GetString("transform.column", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Determines whether or not a C# expression will be used, or a powershell file..
        /// </summary>
        internal static string transformtype_column {
            get {
                return ResourceManager.GetString("transformtype.column", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The where clause is data expression SQL where clause without the WHERE statement. If there are multiple transforms for a single column then the WHERE clause is required for each one..
        /// </summary>
        internal static string whereclause_column {
            get {
                return ResourceManager.GetString("whereclause.column", resourceCulture);
            }
        }
    }
}
