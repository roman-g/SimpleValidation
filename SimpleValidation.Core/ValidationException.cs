﻿using System;

namespace SimpleValidation.Core
{
	public class ValidationException: Exception
	{
		public ValidationException(string message) : base(message)
		{
		}
	}
}