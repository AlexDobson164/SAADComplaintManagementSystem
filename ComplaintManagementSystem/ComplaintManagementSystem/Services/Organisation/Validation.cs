using System.Text.RegularExpressions;

public static class Validation
{
    public static bool ValidateEmail(string email)
    {
        // i took this regex from here - https://regex101.com/r/SOgUIV/2
        return Regex.IsMatch(email, "^((?!\\.)[\\w\\-_.]*[^.])(@\\w+)(\\.\\w+(\\.\\w+)?[^.\\W])$");
    }
}
