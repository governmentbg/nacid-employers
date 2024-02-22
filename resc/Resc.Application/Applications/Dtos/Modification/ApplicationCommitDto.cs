using FileStorageNetCore.Models;
using Resc.Application.Applications.Dtos.Create;
using Resc.Application.Common.Dtos;
using Resc.Application.Lists.Dtos;
using Resc.Application.Nomenclatures.Dtos;
using Resc.Data.Applications.Register;
using Resc.Data.Nomenclatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Resc.Application.Applications.Dtos.Modification
{
    public class ApplicationCommitDto : CommitDto
    {
        public PartDto<StudentDto> StudentPart { get; set; }
        public PartDto<UniversityDto> UniversityPart { get; set; }
        public PartDto<EmployerDto> EmployerPart { get; set; }
        public PartDto<ContractDto> ContractPart { get; set; }
        public PartDto<ActualEducationDto> ActualEducationPart { get; set; }

        public List<ApplicationModificationDto> ApplicationModification { get; set; } = new List<ApplicationModificationDto>();
        public ApplicationTerminationDto ApplicationTermination { get; set; }

        public int CreatorUserId { get; set; }
        public string RegisterNumber { get; set; }

        public static Expression<Func<ApplicationCommit, ApplicationCommitDto>> SelectExpression => e => new ApplicationCommitDto {
            Id = e.Id,
            CreatorUserId = e.Lot.CreatorUserId,
            LotId = e.LotId,
            State = e.State,
            RegisterNumber = e.Lot.RegisterNumber,
            ChangeStateDescription = e.ChangeStateDescription,
            StudentPart = new PartDto<StudentDto> {
                Id = e.StudentPart.Id,
                Entity = new StudentDto {
                    FirstName = e.StudentPart.Entity.FirstName,
                    MiddleName = e.StudentPart.Entity.MiddleName,
                    LastName = e.StudentPart.Entity.LastName,
                    UIN = e.StudentPart.Entity.UIN,
                    Email = e.StudentPart.Entity.Email,
                    PhoneNumber = e.StudentPart.Entity.PhoneNumber,

                    EducationType = e.StudentPart.Entity.EducationType,
                    Status = e.StudentPart.Entity.Status,
                    GraduationDate = e.StudentPart.Entity.GraduationDate,
                },
                State = e.StudentPart.State
            },
            UniversityPart = new PartDto<UniversityDto> {
                Id = e.UniversityPart.Id,
                Entity = new UniversityDto {
                    Institution = e.UniversityPart.Entity.Institution != null
                                            ? new NomenclatureDto<Institution> {
                                                Id = e.UniversityPart.Entity.Institution.Id,
                                                Name = e.UniversityPart.Entity.Institution.Name
                                            }
                                            : null,
                    SpecialityListItem = new SpecialitySelectDto {
                        Id = e.UniversityPart.Entity.SpecialityListItem.Id,
                        ResearchArea = e.UniversityPart.Entity.SpecialityListItem.ResearchArea != null
                                            ? new NomenclatureDto<ResearchArea> {
                                                Id = e.UniversityPart.Entity.SpecialityListItem.ResearchArea.Id,
                                                Name = e.UniversityPart.Entity.SpecialityListItem.ResearchArea.Name
                                            }
                                            : null,
                        Speciality = e.UniversityPart.Entity.SpecialityListItem.Speciality != null
                                            ? new NomenclatureDto<Speciality> {
                                                Id = e.UniversityPart.Entity.SpecialityListItem.Speciality.Id,
                                                Name = e.UniversityPart.Entity.SpecialityListItem.Speciality.Name
                                            }
                                            : null,
                        EducationalQualification = e.UniversityPart.Entity.SpecialityListItem.EducationalQualification != null
                                            ? new NomenclatureDto<EducationalQualification> {
                                                Id = e.UniversityPart.Entity.SpecialityListItem.EducationalQualification.Id,
                                                Name = e.UniversityPart.Entity.SpecialityListItem.EducationalQualification.Name
                                            }
                                            : null,
                        EducationFormType = e.UniversityPart.Entity.SpecialityListItem.EducationFormType != null
                                            ? new NomenclatureDto<EducationFormType> {
                                                Id = e.UniversityPart.Entity.SpecialityListItem.EducationFormType.Id,
                                                Name = e.UniversityPart.Entity.SpecialityListItem.EducationFormType.Name
                                            }
                                            : null,
                    },
                    Rector = e.UniversityPart.Entity.Rector,
                },

                State = e.UniversityPart.State
            },
            EmployerPart = new PartDto<EmployerDto> {
                Id = e.EmployerPart.Id,
                Entity = new EmployerDto {
                    Representative = e.EmployerPart.Entity.Representative,
                    Email = e.EmployerPart.Entity.Email,
                    PhoneNumber = e.EmployerPart.Entity.PhoneNumber,
                    EmployerListItem = e.EmployerPart.Entity.EmployerListItem
                },
                State = e.EmployerPart.State
            },
            ContractPart = new PartDto<ContractDto> {
                Id = e.ContractPart.Id,
                Entity = new ContractDto {
                    SigningDate = e.ContractPart.Entity.SigningDate,
                    EndDate = e.ContractPart.Entity.EndDate,
                    Number = e.ContractPart.Entity.Number,
                    Term = e.ContractPart.Entity.Term,
                    EmploymentTerm = e.ContractPart.Entity.EmploymentTerm,
                    TaxType = e.ContractPart.Entity.TaxType,
                    AttachedFile = e.ContractPart.Entity.ContractFile != null
                        ?   new AttachedFile {
                                Key = e.ContractPart.Entity.ContractFile.Key,
                                Hash = e.ContractPart.Entity.ContractFile.Hash,
                                Size = e.ContractPart.Entity.ContractFile.Size,
                                Name = e.ContractPart.Entity.ContractFile.Name,
                                MimeType = e.ContractPart.Entity.ContractFile.MimeType,
                                DbId = e.ContractPart.Entity.ContractFile.DbId,
                            }
                        : null,
                    Contacts = e.ContractPart.Entity.Contacts.Select(s => new ContactPersonDto {
                        Id = s.Id,
                        Name = s.Name,
                        Email = s.Email,
                        PhoneNumber = s.PhoneNumber,
                        Type = s.Type
                    })
                },
                State = e.ContractPart.State
            },
            ActualEducationPart = new PartDto<ActualEducationDto> {
                Id = e.ActualEducationPart.Id,
                Entity = new ActualEducationDto {
                    EducationalQualification = e.ActualEducationPart.Entity.EducationalQualification != null
                                            ? new NomenclatureDto<EducationalQualification> {
                                                Id = e.ActualEducationPart.Entity.EducationalQualification.Id,
                                                Name = e.ActualEducationPart.Entity.EducationalQualification.Name
                                            }
                                            : null,
                    Status = e.ActualEducationPart.Entity.Status,
                    CourseYear = e.ActualEducationPart.Entity.CourseYear,
                    GraduationDate = e.ActualEducationPart.Entity.GraduationDate,
                    EducationType = e.ActualEducationPart.Entity.EducationType
                },
                State = e.ActualEducationPart.State
            },
            ApplicationModification = e.ApplicationModification != null
                ? e.ApplicationModification.Select(x => new ApplicationModificationDto {
                ModificationDate = x.ModificationDate,
                Reason = x.Reason,
                AnnexFile = x.AnnexFile
			}).ToList()
            : null,
            ApplicationTermination = e.ApplicationTermination != null
               ? new ApplicationTerminationDto {
                        TerminationDate = e.ApplicationTermination.TerminationDate,
                        TerminationReason = new NomenclatureDto<TerminationReason> {
                            Id = e.ApplicationTermination.TerminationReason.Id,
                            Name = e.ApplicationTermination.TerminationReason.Name
                        },
                        AnnexFile = e.ApplicationTermination.AnnexFile != null
                                            ? new AttachedFile {
                                                Key = e.ApplicationTermination.AnnexFile.Key,
                                                Hash = e.ApplicationTermination.AnnexFile.Hash,
                                                Size = e.ApplicationTermination.AnnexFile.Size,
                                                Name = e.ApplicationTermination.AnnexFile.Name,
                                                MimeType = e.ApplicationTermination.AnnexFile.MimeType,
                                                DbId = e.ApplicationTermination.AnnexFile.DbId,
                                            }
                                            : null,
                    }
               : null
        };
    }
}
