using System.Linq;
using Shouldly;
using SimpleValidation.Core.Builders;
using SimpleValidation.Core.Combination;
using Xunit;

namespace SimpleValidation.Core.Tests
{
    public class ValidatorBuilderForClassTest
    {
        [Fact]
        public void Simple()
        {
            var ruleWithMapping = MakeValidator.For<string>()
                                               .Ensure(x => x == "good", x => $"{x}_failed");
            ruleWithMapping("bad").Single().ShouldBe("bad_failed");
            ruleWithMapping("good").ShouldBeEmpty();

            var ruleWithStaticFail = MakeValidator.For<string>()
                                                  .Ensure(x => x == "good", "failed");
            ruleWithStaticFail("bad").Single().ShouldBe("failed");
            ruleWithStaticFail("good").ShouldBeEmpty();
        }

        [Fact]
        public void Union()
        {
            var builder = MakeValidator.For<string>();
            var rule = builder.Union(x => x.Ensure(_ => false, "1"),
                                     x => x.Ensure(_ => false, "2"));
            rule("").ShouldBe(new[] {"1", "2"});
        }

        [Fact]
        public void Order()
        {
            var builder = MakeValidator.For<int>();
            var rule = builder.Order(x => x.Ensure(y => y > 0, "1"),
                                     x => x.Ensure(y => y > 1, "2"));
            rule(0).ShouldBe(new[] {"1"});
            rule(1).ShouldBe(new[] {"2"});
            rule(2).ShouldBeEmpty();
        }

        [Fact]
        public void Custom()
        {
            var builder = MakeValidator.For<int>();
            var rule = builder.With(x => x.Ensure(y => y > 0, "1")
                                            .Then(x.Ensure(y => y > 1, "2")));
            rule(0).ShouldBe(new[] {"1"});
            rule(1).ShouldBe(new[] {"2"});
            rule(2).ShouldBeEmpty();
        }
    }
}