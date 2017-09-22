using Shouldly;
using SimpleValidation.Core.Common;
using Xunit;

namespace SimpleValidation.Core.Tests
{
	public class SimpleValidatorTests
	{
		[Fact]
		public void ValidatorReturnsEmptyArrayWhenFunctionReturnsNull()
		{
			SimpleValidator.Make<string, string>(s => null)("").ShouldBeEmpty();
		}
	}
}