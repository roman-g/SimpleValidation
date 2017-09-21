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

		[Fact]
		public void Union()
		{
			var builder = MakeValidator.For<string>();
			var rule = builder.Union(x => x.Make(_ => false, "1"),
									 x => x.Make(_ => false, "2"));
			rule("").ShouldBe(new[] { "1", "2" });
		}
	}
}