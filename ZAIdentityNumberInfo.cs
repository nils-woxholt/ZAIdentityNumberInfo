using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAIdentityNumberInfo
{
    public enum GenderType
    {
        Unknown,
        Female,
        Male
    }
    public enum CitizenshipType
    {
        Unknown,
        Citizen,
        PermanentResident
    }
    public class IDInfo
    {
        
        public string IDNumber { get; set; }
        public bool Valid { get; set; }
//        public DateTime DateOfBirth { get; set; }
        public GenderType Gender { get; set; }
        public CitizenshipType Citizenship { get; set; }
        public string ErrorMessage { get; set; }

    }

    public class IdentityInfo
    {
        public IDInfo Validate(string idIn)
        {
            idIn = idIn.Trim();

            IDInfo idInfo = new IDInfo { IDNumber = idIn } ;

            // check length
            if (idIn.Length != 13)
            {

                idInfo.ErrorMessage = "ID Number must be 13 characters";
                return idInfo;
            }

            // must be all numbers
            if (!long.TryParse(idIn, out long result))
            {

                idInfo.ErrorMessage = "ID Number must be 13 numeric characters";
                return idInfo;
            }

            // invalid citizenship digit 11 can only be 0 or 1
            if (!(idIn.Substring(10,1) == "0" | idIn.Substring(10,1) == "1"))
            {

                idInfo.ErrorMessage = "Invalid citizenship: digit 11 can only be 0 or 1";
                return idInfo;
            }

            // digit 12 should be an 8 
            if (idIn.Substring(11, 1) != "8")
            {
                idInfo.ErrorMessage = "The 12th digit should always be an 8 on a currnet ID number (pre 1980's numbers could contain 0-7)";
                return idInfo;
            }

            // validate checksum
            if (!ValidateChecksum(idIn))
            {
                idInfo.ErrorMessage = "Invalid ID Number - checksum failed";
                return idInfo;
            }
            else
            {
                idInfo.Valid = true;
            };

            //// set date
            //idInfo.DateOfBirth = new DateTime(
            //    Convert.ToInt16(idIn.Substring(0, 2)),
            //    Convert.ToInt16(idIn.Substring(2, 2)),
            //    Convert.ToInt16(idIn.Substring(4, 2))
            //    );

            // set gender
            if (Convert.ToInt16(idIn.Substring(6, 4)) < 5000)
            {
                idInfo.Gender = GenderType.Female;
            }
            else
            {
                idInfo.Gender = GenderType.Male;
            }

            // set citizenship
            if (idIn.Substring(10, 1) == "0")
            {
                idInfo.Citizenship = CitizenshipType.Citizen;
            }
            else
            {
                idInfo.Citizenship = CitizenshipType.PermanentResident;
            }

            return idInfo;
        }

        public bool ValidateChecksum(string idNumber)
        {
            // Luhn algorithm check 
            // source: https://en.wikipedia.org/wiki/Luhn_algorithm#C#

            int sum = 0;
            bool alternate = false;
            for (int i = idNumber.Length - 1; i >= 0; i--)
            {
                char[] nx = idNumber.ToArray();
                int n = int.Parse(nx[i].ToString());

                if (alternate)
                {
                    n *= 2;

                    if (n > 9)
                    {
                        n = (n % 10) + 1;
                    }
                }
                sum += n;
                alternate = !alternate;
            }
            return (sum % 10 == 0);
        }
    }

}
