using Shouldly;
using Xunit;

namespace SimpleValidation.Core.Tests
{
    public class PredicateHelpersTest
    {
        [Fact]
        public void Simple()
        {
	        var rule = PredicateHelpers.WrapWithPredicate<string, string>(
				x => !string.IsNullOrEmpty(x),
		        input => ValidationResult<string>.Fail("Empty string"));

			rule("filled").IsFail.ShouldBeFalse();
	        var fail = rule("");
	        fail.IsFail.ShouldBeTrue();
			fail.FailValue.ShouldBe("Empty string");
		}
    }
}
