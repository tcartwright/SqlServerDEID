using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerDEID.Common.Globals.Models
{
    public class CustomExpandoObject : DynamicObject
    {
        public IDictionary<string, object> Members { get; private set; } = new Dictionary<string, object>();

        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            return this.Members.Remove(binder.Name);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return this.Members.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this.Members[binder.Name] = value;
            return true;
        }

        public override string ToString()
        {
            if (this.Members.Any())
            {
                return String.Join("\r\n", this.Members.Select((k, v) => $"{k.Key, 15}: {GetValue(k.Value)}"));
            }
            else
            {
                return base.ToString();
            }

        }

        private string GetValue(object value)
        {
            if (value == null) return "null";
            return Convert.ToString(value);
        }
    }
}
