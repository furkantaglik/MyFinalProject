using FluentValidation;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CrossCC.Validation
{
	public static class ValidationTool
	{
		public static void Validate(IValidator validator, object entity)
		{
			var context = new ValidationContext<object>(entity);
			var result = validator.Validate(context);
			if (!result.IsValid)
			{
				throw new ValidationException(result.Errors);
			}
		}
	}
}
