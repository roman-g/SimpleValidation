namespace SimpleValidation.Core.Builders
{
	public static class MakeValidator
	{
		public static IValidatorBuilderForClass<T> For<T>()
		{
			return new GenericCarrier<T>();
		}

		private class GenericCarrier<T> : IValidatorBuilderForClass<T>
		{
		}
	}
}