using System;
using SimpleValidation.Core;
using Xunit;

namespace SimpleValidation.Priority.Test
{
    public class PriorityExtensionsTest
	{
        [Fact]
        public void Simple()
        {
			var sample = new TestClass();
			//new RuleFor<TestClass>().Member(x => x.Field1).Rule((str, _) => !string.IsNullOrEmpty(str))
        }

		public class TestClass
		{
			public string Field1;
			public string Field2;
		}
    }
}
