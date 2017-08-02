namespace SimpleValidation.Core
{
	public class ValidationResult<TFail>
	{
		public bool IsFail { get; private set; }
		private TFail value;

		public TFail FailValue
		{
			get
			{
				if (!IsFail)
					throw new ValidationException("Can't get the fail value because the result is success");
				return value;
			}
			private set => this.value = value;
		}

		public static ValidationResult<TFail> Success()
		{
			return new ValidationResult<TFail>
			{
				IsFail = false
			};
		}

		public static ValidationResult<TFail> Fail(TFail failValue)
		{
			return new ValidationResult<TFail>
			{
				IsFail = true,
				FailValue = failValue
			};
		}

		
	}
}