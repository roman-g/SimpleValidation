﻿using System;

namespace SimpleValidation.Core.Common
{
	public class ValidationException: Exception
	{
		public ValidationException(string message) : base(message)
		{
		}
	}
}