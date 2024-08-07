using FsCheck;

namespace PropertyTesting
{
    public static class AlphaNumericString
    {
        private static bool IsAlphanumeric(string value) => value.All(c => !char.IsLetterOrDigit(c));

        private static bool IsNotNullOrEmpty(string? value) => !String.IsNullOrEmpty(value);

        public static Arbitrary<string> Generate()
        {
            return Arb.Default.String().Filter(s => s != null && !IsNotNullOrEmpty(s) && IsAlphanumeric(s));
        }
    }

    public static class RandomFirstName
    {
        private static readonly IList<string> Names = new[]
        {
            "Tony", "Jay", "Bogdan", "Tim", "Trist"
        };

        public static Arbitrary<string> Generate()
        {
            return Gen.Choose(0, Names.Count - 1).Select(x => Names[x]).ToArbitrary();
        }
    }
}
