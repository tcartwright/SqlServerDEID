using Bogus;
using Bogus.DataSets;
using System.Text;

namespace SqlServerDEID.Common.Globals.Models
{
    public class GenderedPerson: Person
    {
        public GenderedPerson(Name.Gender gender, string locale = "en", int? seed = null)
        {
            GetDataSources(locale);
            if (seed.HasValue)
            {
                Random = new Randomizer(seed.Value);
            }

            Populate(gender);
        }

        private void GetDataSources(string locale)
        {
            DsName = Notifier.Flow(new Name(locale));
            DsInternet = Notifier.Flow(new Internet(locale));
            DsDate = Notifier.Flow(new Date
            {
                Locale = locale
            });
            DsPhoneNumbers = Notifier.Flow(new PhoneNumbers(locale));
            DsAddress = Notifier.Flow(new Address(locale));
            DsCompany = Notifier.Flow(new Company(locale));
        }

        public virtual void Populate(Name.Gender gender)
        {
            Gender = gender;
            FirstName = DsName.FirstName(Gender);
            LastName = DsName.LastName(Gender);
            FullName = string.Concat(FirstName, " ", LastName);
            UserName = DsInternet.UserName(FirstName, LastName);
            Email = DsInternet.Email(FirstName, LastName);
            Website = DsInternet.DomainName();
            Avatar = DsInternet.Avatar();
            DateOfBirth = DsDate.Past(70, Date.SystemClock().AddYears(-18));
            Phone = DsPhoneNumbers.PhoneNumber();
            Address = new CardAddress
            {
                Street = DsAddress.StreetAddress(),
                Suite = DsAddress.SecondaryAddress(),
                City = DsAddress.City(),
                State = DsAddress.State(),
                ZipCode = DsAddress.ZipCode(),
                Geo = new CardAddress.CardGeo
                {
                    Lat = DsAddress.Latitude(),
                    Lng = DsAddress.Longitude()
                }
            };
            Company = new CardCompany
            {
                Name = DsCompany.CompanyName(),
                CatchPhrase = DsCompany.CatchPhrase(),
                Bs = DsCompany.Bs()
            };
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            var format = "  {0, 15}: {1}\r\n";
            if (Gender == Name.Gender.Male)
            {
                sb.AppendLine("Male:");
            }
            else
            {
                sb.AppendLine("Female:");
            }
            sb.AppendFormat(format, "FirstName", FirstName);
            sb.AppendFormat(format, "LastName", LastName);
            
            sb.AppendFormat(format, "UserName", UserName);
            sb.AppendFormat(format, "Email", Email);
            sb.AppendFormat(format, "Website", Website);
            sb.AppendFormat(format, "Avatar", Avatar);
            sb.AppendFormat(format, "DateOfBirth", DateOfBirth);
            sb.AppendFormat(format, "Phone", Phone);

            sb.AppendLine("  Address:");
            sb.AppendFormat(format, "  Street", Address.Street);
            sb.AppendFormat(format, "  Suite", Address.Suite);
            sb.AppendFormat(format, "  City", Address.City);
            sb.AppendFormat(format, "  State", Address.State);
            sb.AppendFormat(format, "  ZipCode", Address.ZipCode);

            sb.AppendLine("  Company:");
            sb.AppendFormat(format, "  Name", Company.Name);
            sb.AppendFormat(format, "  CatchPhrase", Company.CatchPhrase);
            sb.AppendFormat(format, "  Bs", Company.Bs);
            return sb.ToString();
        }
    }
}
