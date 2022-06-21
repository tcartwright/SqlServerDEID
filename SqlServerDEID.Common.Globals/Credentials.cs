using Meziantou.Framework.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SqlServerDEID.Common.Globals
{
    public static class Credentials
    {
        public static void WriteCredential(string applicationName, string userName, string password)
        {
            CredentialManager.WriteCredential(applicationName, userName, password, CredentialPersistence.LocalMachine);
        }

        public static void RemoveCredential(string applicationName)
        {
            CredentialManager.DeleteCredential(applicationName);
        }

        public static IList<string> ListCredentials()
        {
            var credentials = CredentialManager.EnumerateCredentials()
                .Where(cr => cr.CredentialType == CredentialType.Generic && !Regex.IsMatch(cr.ApplicationName, @"git.*|microsoft.*|onedrive.*|xbox.*", RegexOptions.IgnoreCase))
                .OrderBy(cr => cr.ApplicationName)
                .Select(cr => cr.ApplicationName)
                .ToList();
            return credentials;
        }
    }
}
