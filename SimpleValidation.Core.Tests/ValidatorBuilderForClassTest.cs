using System.Linq;
using Shouldly;
using SimpleValidation.Core.Builders;
using Xunit;

namespace SimpleValidation.Core.Tests
{
	public class ValidatorBuilderForClassTest
	{
		[Fact]
		public void Simple()
		{
			var ruleWithMapping = MakeValidator.For<string>()
											   .Make(x => x == "good", x => $"{x}_failed");
			ruleWithMapping("bad").Single().ShouldBe("bad_failed");
			ruleWithMapping("good").ShouldBeEmpty();

			var ruleWithStaticFail = MakeValidator.For<string>()
											   .Make(x => x == "good", "failed");
			ruleWithStaticFail("bad").Single().ShouldBe("failed");
			ruleWithStaticFail("good").ShouldBeEmpty();
		}
	}
}