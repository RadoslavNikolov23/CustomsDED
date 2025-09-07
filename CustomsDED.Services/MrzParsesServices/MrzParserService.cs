namespace CustomsDED.Services.MrzParsesServices
{
    using System.Globalization;

    using CustomsDED.Common.Enums;
    using CustomsDED.Common.Helpers;
    using CustomsDED.Data.Models;

    public static class MrzParserService
    {
        public static async Task<Person?> ParseMRZCodeAsync(string[] lines)
        {
            Person person = new Person();

            try
            {
                string documentTypeStr = lines[0].Substring(0, 2);

                if (lines.Length < 3 && (documentTypeStr != null && (documentTypeStr == "ID" || documentTypeStr == "P<")))
                {
                    string issuingCountry = lines[0].Substring(2, 3);

                    if (documentTypeStr == "ID" && (issuingCountry != null && issuingCountry == "ROU"))
                    {
                        string line1 = lines[0].Replace(" ", "").PadRight(36, '<');
                        string line2 = lines[1].Replace(" ", "").PadRight(36, '<');

                        person.DocumentType = documentTypeStr;

                        // Line 1
                        person.IssuingCountry = issuingCountry;

                        // Line 1: Names
                        var names = line1.Substring(5).Split(new[] { "<<" }, StringSplitOptions.None);
                        person.LastName = names[0].Replace("<", " ").Trim();
                        person.FirstName = names[1].Replace("<", " ").Trim();

                        // Line 2
                        person.DocumentNumber = line2.Substring(0, 8);

                        person.Nationality = line2.Substring(10, 3);


                        string dobRaw = line2.Substring(13, 6);   // YYMMDD


                        person.DateOfBirth = ParseDOBDate(dobRaw);

                        string sexStr = line2.Substring(20, 1);
                        bool isValidSec = Enum.TryParse<SexType>(sexStr, out SexType sexEnum);
                        person.SexType = isValidSec ? sexEnum : null;


                        var expirationDateRaw = line2.Substring(21, 6);  // Expiry date

                        person.ExpirationDate = ParseExpirationDate(expirationDateRaw);

                        person.PersonalNumber = $"1{dobRaw}{line2.Substring(29, 7)}";


                    }
                    else if (documentTypeStr == "P<" && (issuingCountry != null && issuingCountry == "TUR"))
                    {
                        //TUR

                        string line1 = lines[0].Replace(" ", "").PadRight(44, '<');
                        string line2 = lines[1].Replace(" ", "").PadRight(44, '<');

                        person.DocumentType = "Passport";

                        // Line 1
                        person.IssuingCountry = issuingCountry;

                        // Line 1: Names
                        var names = line1.Substring(5).Split(new[] { "<<" }, StringSplitOptions.None);
                        person.LastName = names[0].Replace("<", " ").Trim();
                        person.FirstName = names[1].Replace("<", " ").Trim();

                        //Line 2:

                        string documentNumber = line2.Substring(0, 9);
                        person.DocumentNumber = documentNumber;

                        string nationality = line2.Substring(10, 3);
                        person.Nationality = nationality;

                        string dobRaw = line2.Substring(13, 6);   // YYMMDD


                        person.DateOfBirth = ParseDOBDate(dobRaw);

                        string sexStr = line2.Substring(20, 1);
                        bool isValidSec = Enum.TryParse<SexType>(sexStr, out SexType sexEnum);
                        person.SexType = isValidSec ? sexEnum : null;

                        var expirationDateRaw = line2.Substring(21, 6);  // Expiry date

                        person.ExpirationDate = ParseExpirationDate(expirationDateRaw);
                        person.PersonalNumber = line2.Substring(28, 11);
                    }
                    else if (documentTypeStr == "P<" && (issuingCountry != null && issuingCountry == "BGR"))
                    {
                        //TUR and BUL

                        var line1 = lines[0].Replace(" ", "").PadRight(44, '<');
                        var line2 = lines[1].Replace(" ", "").PadRight(44, '<');

                        person.DocumentType = "Passport";

                        // Line 1
                        person.IssuingCountry = issuingCountry;

                        // Line 1: Names
                        var names = line1.Substring(5).Split(new[] { "<<" }, StringSplitOptions.None);
                        person.LastName = names[0].Replace("<", " ").Trim();
                        if (names.Length > 1)
                        {
                            var firstNames = names[1].Split(new[] { "<" }, StringSplitOptions.None);

                            if (firstNames.Length == 1)
                            {
                                person.FirstName = firstNames[0].Replace("<", " ").Trim();
                            }
                            else
                            {
                                person.FirstName = firstNames[0].Replace("<", " ").Trim();
                                person.MiddleName = firstNames[1].Replace("<", " ").Trim();

                            }
                        }


                        //Line 2:

                        string documentNumber = line2.Substring(0, 9);
                        person.DocumentNumber = documentNumber.Replace("<", "").Trim();

                        string nationality = line2.Substring(10, 3);
                        person.Nationality = nationality;

                        string dobRaw = line2.Substring(13, 6);   // YYMMDD


                        person.DateOfBirth = ParseDOBDate(dobRaw);

                        string sexStr = line2.Substring(20, 1);
                        bool isValidSec = Enum.TryParse<SexType>(sexStr, out SexType sexEnum);
                        person.SexType = isValidSec ? sexEnum : null;

                        var expirationDateRaw = line2.Substring(21, 6);  // Expiry date

                        person.ExpirationDate = ParseExpirationDate(expirationDateRaw);
                        person.PersonalNumber = line2.Substring(28, 10);

                    }
                }
                else if (lines.Length == 3 && (documentTypeStr != null && documentTypeStr == "ID"))
                {
                    if (documentTypeStr == "ID")
                    {
                        var line1 = lines[0].PadRight(30, '<');
                        var line2 = lines[1].PadRight(30, '<');
                        var line3 = lines[2].PadRight(30, '<');

                        person.DocumentType = documentTypeStr;
                        // Line 1
                        person.IssuingCountry = line1.Substring(2, 3);
                        person.DocumentNumber = line1.Substring(5, 9);

                        // Line 2
                        string dobRaw = line2.Substring(0, 6);   // YYMMDD


                        person.DateOfBirth = ParseDOBDate(dobRaw);

                        string sexStr = line2.Substring(7, 1);
                        bool isValidSec = Enum.TryParse<SexType>(sexStr, out SexType sexEnum);
                        person.SexType = isValidSec ? sexEnum : null;

                        var expirationsDateRaw = line2.Substring(8, 6);  // Expiry date

                        person.ExpirationDate = ParseExpirationDate(expirationsDateRaw);
                        person.Nationality = line2.Substring(15, 3);
                        person.PersonalNumber = line2.Substring(18, 10);

                        // Line 3: Names
                        var names = line3.Split(new[] { "<<" }, StringSplitOptions.None);
                        person.LastName = names[0].Replace("<", " ").Trim();
                        if (names.Length > 1)
                        {
                            var firstNames = names[1].Split(new[] { "<" }, StringSplitOptions.None);

                            if (firstNames.Length == 1)
                            {
                                person.FirstName = firstNames[0].Replace("<", " ").Trim();
                            }
                            else
                            {
                                person.FirstName = firstNames[0].Replace("<", " ").Trim();
                                person.MiddleName = firstNames[1].Replace("<", " ").Trim();

                            }
                        }

                    }
                }
                return person;
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in ParseMRZCodeAsync, in the MrzParserService class.");
                throw;
            }

        }

        private static DateTime? ParseDOBDate(string rawDate)
        {
            string dateFormats = "yyMMdd";

            bool isDateValid = DateTime.TryParseExact
             (rawDate, dateFormats, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime dateTimeParse);

            if (isDateValid)
            {
                return dateTimeParse;
            }

            return null;

        }

        private static DateTime? ParseExpirationDate(string rawDate)
        {
            string dateFormats = "yyMMdd";

            var culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();

            // Use GregorianCalendar with a higher TwoDigitYearMax
            var calendar = new GregorianCalendar
            {
                TwoDigitYearMax = 2099 // years 00-99 will map to 2000–2099
            };

            culture.DateTimeFormat.Calendar = calendar;

            bool isDateValid = DateTime.TryParseExact
              (rawDate, dateFormats, culture,
                 DateTimeStyles.None, out DateTime dateTimeParse);

            if (isDateValid)
            {
                return dateTimeParse;
            }

            return null;

        }
    }
}
