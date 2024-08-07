using FsCheck;
using FsCheck.Xunit;

namespace PropertyTesting
{
    public class UnitTest1
    {
        [Property(Verbose = true)]
        public bool AdditionIsCommutative(int x, int y) => (x + y) == (y + x);

        [Property(Verbose = true)]
        public bool MultiplicationByIdIsAlwaysZero(int x) => (x * 0) == 0.0;

        [Property(Verbose = true)]
        public bool AddElementToArrayResultsAppend(List<int> xs, int x)
        {
            xs.Add(x);
            return xs.Last() == x;
        }

        [Property(Verbose = true)]
        public bool AddElementToArrayPreservesOriginal(List<int> xs, int x)
        {
            var original = xs.ToArray();

            xs.Add(x);

            return xs.Take(original.Length).SequenceEqual(original);
        }


        [Property(Verbose = true)]
        public bool ReversePreservesSequenceOnRepeat(List<int> xs)
        {
            var ys = xs.AsEnumerable().Reverse();

            var zs = ys.Reverse();

            return zs.SequenceEqual(xs);
        }

        [Property(Verbose = true)]
        public bool ReverseFlipsSequence(List<int> xs)
        {
            xs = xs.Distinct().ToList();
            if (xs.Count <= 1) return true; // ignore too-small sequences 

            var ys = xs.AsEnumerable().Reverse().ToList();

            return !ys.SequenceEqual(xs);
        }


        [Property(Verbose = true)]
        public bool JsonSerialisationProducesArray(List<int> xs)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(xs);

            var jo = Newtonsoft.Json.Linq.JArray.Parse(json);

            var ys = jo.SelectTokens(".[*]").Select(jt => jt.ToObject<int>()).ToArray();

            return ys.SequenceEqual(xs);
        }

        [Property(Verbose = true)]
        public bool JsonSerialisationIsSymmetric(List<int> xs)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(xs);

            var ys = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(json);

            return ys.SequenceEqual(xs);
        }

        [Property(Verbose = true)]
        public bool JsonComposedTests(List<int> xs)
        {
            return JsonSerialisationProducesArray(xs)
                   && JsonSerialisationIsSymmetric(xs);
        }

        [Property(Verbose = true, MaxTest = 1000, Arbitrary = new[] { typeof(AlphaNumericString) })]
        public bool StringsLengthIsNonNegative(string value) => value.Length >= 0;

        [Property(Verbose = true, MaxTest = 1000)]
        public bool RandomStringsAreValidated(string value)
        {
            bool ValidateSut(string x) => true; // we don't care what the result, we're demonstrating a validation function

            return ValidateSut(value);
        }

        [Property(Verbose = true, Arbitrary = new[] { typeof(RandomFirstName) })]
        public bool ConstrainedSetArbitrary(string? firstName)
        {
            return firstName != null;
        }

    }
}