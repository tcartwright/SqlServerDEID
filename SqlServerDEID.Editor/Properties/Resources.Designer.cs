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
        ///   Looks up a localized string similar to IF OBJECT_ID(&apos;tempdb..#TransformTest&apos;) IS NOT NULL 
        ///	DROP TABLE #TransformTest; 
        ///	
        ///SELECT TOP (1000) * 
        ///INTO #TransformTest 
        ///FROM (
        ///{{QUERY}}
        ///) t; 
        ///
        ///SELECT * FROM [#TransformTest] AS [tt].
        /// </summary>
        internal static string GetTransformData {
            get {
                return ResourceManager.GetString("GetTransformData", resourceCulture);
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
    }
}
