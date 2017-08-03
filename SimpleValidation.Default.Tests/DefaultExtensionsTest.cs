using System.Linq;
using Shouldly;
using SimpleValidation.Core.MemberRules;
using Xunit;

namespace SimpleValidation.Default.Tests
{
    public class DefaultExtensionsTest
    {
        [Fact]
        public void Simple()
        {
	        var rules = new[]
	        {
		        new MembersFor<TestData>().Member(x => x.IntValue).GreaterThan(10),
		        new MembersFor<TestData>().Member(x => x.StringValue).NotEmpty()
	        };

	        var result = rules.Apply(new TestData());

			result.Errors.Length.ShouldBe(2);
	        var intError = result.Errors.Single(x => x.PropertyName == "IntValue");
			intError.Message.ShouldBe("Value should be greater than 10");
	        intError.PropertyValue.ShouldBe(0);

			var stringError = result.Errors.Single(x => x.PropertyName == "StringValue");
	        stringError.Message.ShouldBe("String should not be empty");
			stringError.PropertyValue.ShouldBeNull();
		}

	    public class TestData
	    {
		    public string StringValue { get; set; }
			public int IntValue { get; set; }
	    }
    }
}
