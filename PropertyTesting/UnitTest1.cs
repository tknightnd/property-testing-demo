using FsCheck;
using FsCheck.Xunit;

namespace PropertyTesting
{
    public class UnitTest1
    {
        [Property(Verbose = true)]
        public bool Addition_Is_Commutative(int x, int y) => (x + y) == (y + x);

        [Property(Verbose = true)]
        public bool Multiplication_By_Id_Is_AlwaysZero(int x) => (x * 0) == 0.0;

        [Property(Verbose = true)]
        public bool Add_Element_To_Array_Results_In_Append(List<int> xs, int x)
        {
            xs.Add(x);
            return xs.Last() == x;
        }

        [Property(Verbose = true)]
        public bool Add_Element_To_Array_Preserves_Original(List<int> xs, int x)
        {
            var original = xs.ToArray();

            xs.Add(x);

            return xs.Take(original.Length).SequenceEqual(original);
        }
        
        [Property(Verbose = true)]
        public bool Reverse_Preserves_Sequence_On_Repeat(List<int> xs)
        {
            var ys = xs.AsEnumerable().Reverse();

            var zs = ys.Reverse();

            return zs.SequenceEqual(xs);
        }

        [Property(Verbose = true)]
        public bool Reverse_Flips_Sequence(List<int> xs)
        {
            xs = xs.Distinct().ToList();
            if (xs.Count <= 1) return true; // ignore too-small sequences 

            var ys = xs.AsEnumerable().Reverse().ToList();

            return !ys.SequenceEqual(xs);
        }


        [Property(Verbose = true)]
        public bool Json_Serialisation_Produces_Array(List<int> xs)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(xs);

            var jo = Newtonsoft.Json.Linq.JArray.Parse(json);

            var ys = jo.SelectTokens(".[*]").Select(jt => jt.ToObject<int>()).ToArray();

            return ys.SequenceEqual(xs);
        }

        [Property(Verbose = true)]
        public bool Json_Serialisation_Is_Symmetric(List<int> xs)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(xs);

            var ys = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(json);

            return ys.SequenceEqual(xs);
        }

        [Property(Verbose = true)]
        public bool Json_Composed_Tests(List<int> xs)
        {
            return Json_Serialisation_Produces_Array(xs)
                   && Json_Serialisation_Is_Symmetric(xs);
        }

        [Property(Verbose = true, MaxTest = 1000, Arbitrary = new[] { typeof(AlphaNumericString) })]
        public bool Strings_Length_Is_NonNegative(string value) => value.Length >= 0;

        [Property(Verbose = true, MaxTest = 1000)]
        public bool Random_Strings_Are_Validated(string value)
        {
            bool ValidateSut(string x) => true; // we don't care what the result, we're demonstrating a validation function

            return ValidateSut(value);
        }

        [Property(Verbose = true, Arbitrary = new[] { typeof(RandomFirstName) })]
        public bool Constrained_Set_Arbitrary(string? firstName)
        {
            return firstName != null;
        }

    }
}