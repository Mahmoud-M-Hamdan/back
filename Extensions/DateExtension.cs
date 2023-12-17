namespace Api.Extensions
{
    public static class DateExtension
    {

        public static int GetAgeFromBirth(this DateOnly dat)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow).Year;
            var age = today - dat.Year;
            return age;
        }
    }
}