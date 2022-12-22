using System.Collections;
using System.Collections.Generic;

namespace NuGet.CommandLine.Test.Caching
{
    public class CachingValidations : IEnumerable<CachingValidation>
    {
        private readonly IDictionary<CachingValidationType, CachingValidation> _validations
            = new Dictionary<CachingValidationType, CachingValidation>();

        public CachingValidation this[CachingValidationType key]
        {
            get { return _validations[key]; }
            set { _validations[key] = value; }
        }

        public void Add(CachingValidationType type, bool isTrue)
        {
            this[type] = new CachingValidation(type, isTrue);
        }

        public IEnumerator<CachingValidation> GetEnumerator()
        {
            return _validations.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Assert(CachingValidationType type, bool isTrue)
        {
            CachingValidation validation;

            Xunit.Assert.True(_validations.TryGetValue(type, out validation), $"No validation of type '{type}' was found.");

            if (isTrue)
            {
                Xunit.Assert.True(validation.IsTrue, $"The validation '{validation.Message}' ('{validation.Type}') was expected to be true and was not.");
            }
            else
            {
                Xunit.Assert.False(validation.IsTrue, $"The validation '{validation.Message}' ('{validation.Type}') was expected to be false and was not.");
            }
        }
    }
}
