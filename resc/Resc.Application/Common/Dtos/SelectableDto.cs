using System;
using System.Linq.Expressions;

namespace Resc.Application.Common.Dtos
{
	public interface IMapping<TFrom, TTo>
	{
		public abstract Expression<Func<TFrom, TTo>> Map();
	}
}
