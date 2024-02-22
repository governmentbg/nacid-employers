using Microsoft.Extensions.DependencyInjection;
using Resc.Application.Applications;
using Resc.Application.Applications.Interfaces;
using Resc.Application.Applications.Parts;
using Resc.Application.Common.Interfaces;
using Resc.Application.Common.Services;
using Resc.Application.DomainValidations;
using Resc.Application.Emails;
using Resc.Application.Emails.Interfaces;
using Resc.Application.InstitutionSpecialities.Services;
using Resc.Application.Lists;
using Resc.Application.Lists.Employer;
using Resc.Application.Lists.Specialities;
using Resc.Application.Logging;
using Resc.Application.Nomenclatures;
using Resc.Application.Nomenclatures.Services;
using Resc.Application.Users;
using Resc.Application.Users.Interfaces;
using System.Reflection;

namespace Resc.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, Assembly assembly)
        {
            services
                .AddScoped<DomainValidationService>()
                .AddScoped<InstitutionSpecialitiesService>()
                .AddScoped<InstitutionService>()
                .AddScoped<IEnumUtility, EnumUtility>()
                .AddScoped<IExcelProcessor, ExcelProcessor>()
                .AddScoped<ITemplateService, TemplateService>()
                .AddScoped<IPdfService, PdfService>()
                .AddScoped<GeneratePdfService>()
                ;

            services
                .AddScoped<ILoggingService, DbLoggingService>()
                ;

            services
                .AddTransient<IPasswordService, PasswordService>()
                .AddTransient<IActivationService, ActivationService>()
                .AddTransient<IForgottenPasswordService, ForgottenPasswordService>()
                .AddTransient<ILoginService, LoginService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient(typeof(IEmailService), typeof(EmailService))
                ;

            services
                .AddTransient<SchoolYearService>()
                .AddTransient(typeof(INomenclatureService<>), typeof(NomenclatureService<>))
                ;

            services
                .AddTransient<IApplicationService, ApplicationService>()
                .AddTransient<IApplicationModificationService, ApplicationModificationService>()
                .AddTransient<IReportService, ReportService>()
                ;

            services
                .AddTransient<SpecialityListService>()
                .AddTransient<EmployerListService>()
                .AddTransient(typeof(IListService<,>), typeof(ListService<,>))
                ;

            services
                .AddTransient(typeof(IPartService<,>), typeof(PartService<,>))
                .AddTransient<StudentPartService>()
                .AddTransient<EmployerPartService>()
                .AddTransient<ContractPartService>()
                .AddTransient<ActualEducationPartService>()
                .AddTransient<UniversityPartService>();

            return services;
        }
    }
}
