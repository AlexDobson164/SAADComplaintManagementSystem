using System.Text.RegularExpressions;

public static class Validation
{
    public static bool ValidateEmail(string email)
    {
        // i took this regex from here - https://regex101.com/r/SOgUIV/2
        return Regex.IsMatch(email, "^((?!\\.)[\\w\\-_.]*[^.])(@\\w+)(\\.\\w+(\\.\\w+)?[^.\\W])$");
    }
    public static bool ValidatePostcode(string postcode)
    {
        // offical Postcode Regex can be found here, but it is too long and throws and error if i use it - https://github.com/stemount/gov-uk-official-postcode-regex-helper
        // i took this regex from here - https://regex101.com/r/cU7sU7/1
        return Regex.IsMatch(postcode, "^([a-zA-Z]{1,2}\\d{1,2}[a-zA-Z]{0,2})\\s*?(\\d[a-zA-Z]{2})$");
    }
}
