using FileStorageNetCore.Models;
using Resc.Data.Common.Models;
using System;
using System.Collections.Generic;

namespace Resc.Data.Applications.Register
{
	public class ApplicationCommit : Commit
	{
		public ApplicationLot Lot { get; set; }
		public StudentPart StudentPart { get; set; }
		public EmployerPart EmployerPart { get; set; }
		public UniversityPart UniversityPart { get; set; }
		public ContractPart ContractPart { get; set; }
		public ActualEducationPart ActualEducationPart { get; set; }

		public List<ApplicationModification> ApplicationModification { get; set; } = new List<ApplicationModification>();

		public int? ApplicationTerminationId { get; set; }
		public ApplicationTermination ApplicationTermination { get; set; }

		public ApplicationCommit()
			:base()
		{

		}

		public ApplicationCommit(ApplicationCommit commit)
			:base(commit)
		{
			this.StudentPart = new StudentPart(commit.StudentPart);
			this.EmployerPart = new EmployerPart(commit.EmployerPart);
			this.UniversityPart = new UniversityPart(commit.UniversityPart);
			this.ContractPart = new ContractPart(commit.ContractPart);
			this.ActualEducationPart = new ActualEducationPart(commit.ActualEducationPart);
			
			if (commit.ApplicationModification != null)
			{
				foreach (var modification in commit.ApplicationModification)
				{
					this.ApplicationModification.Add(new ApplicationModification(modification.ModificationDate, modification.Reason, modification.AnnexFile));
				}
			}

			if (commit.ApplicationTermination != null)
			{
				this.ApplicationTermination = new ApplicationTermination(commit.ApplicationTermination.TerminationDate, commit.ApplicationTermination.TerminationReasonId, commit.ApplicationTermination.AnnexFile);
			}
		}

		public void AddApplicationModification(DateTime modificationDate, string reason, AttachedFile annexFile)
		{
			this.ApplicationModification.Add(new ApplicationModification(modificationDate, reason, annexFile));
		}

		public void AddApplicationTermination(DateTime terminationDate, int terminationReasonId, AttachedFile annexFile)
		{
			this.ApplicationTermination = new ApplicationTermination(terminationDate, terminationReasonId, annexFile);
		}
	}
}
