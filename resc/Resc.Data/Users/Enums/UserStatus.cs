﻿using System.ComponentModel;

namespace Resc.Data.Users.Enums
{
	public enum UserStatus
	{
		[Description("Активен")]
		Active = 1,

		[Description("Неактивиран")]
		Inactive = 2,

		[Description("Деактивиран")]
		Deactivated = 3
	}
}
