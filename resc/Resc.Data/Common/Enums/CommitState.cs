using System.ComponentModel;

namespace Resc.Data.Common.Enums
{
	public enum CommitState
	{
		[Description("чернова")]
		InitialDraft = 1,

		[Description("в процес на промяна")]
		Modification = 2,

		[Description("изпратен за вписване")]
		Actual = 3,

		[Description("актуално с инициирана промяна")]
		ActualWithModification = 4,

		[Description("предишно състояние")]
		History = 5,

		[Description("изтрит")]
		Deleted = 6,

		//not used
		[Description("готов за вписване")]
		CommitReady = 7,

		[Description("вписан")]
		Entered = 8,

		[Description("вписан с инициирана промяна")]
		EnteredWithModification = 9,

		[Description("В процес на редакция")]
		EnteredModification = 10,

		[Description("вписан с изменение")]
		EnteredWithChange = 11,

		[Description("прекратен")]
		Terminated = 12,

		[Description("изтекъл")]
		Expired = 13
	}
}
