﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Resc.Application.Emails.Interfaces;
using Resc.Data.Emails;
using Resc.Data.Emails.Enums;
using Resc.Hosting.Infrastructure.Configurations;
using Resc.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Hosting.BackgroundServices.Emails
{
    public class EmailJob : IHostedService, IDisposable
    {
		private readonly IServiceProvider serviceProvider;
		private readonly EmailConfiguration emailConfiguration;
		private Timer timer;

		public EmailJob(IServiceProvider serviceProvider, IOptions<EmailConfiguration> options)
		{
			this.serviceProvider = serviceProvider;
			this.emailConfiguration = options.Value;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			this.timer = new Timer(this.DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(this.emailConfiguration.JobPeriod));

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			timer?.Change(Timeout.Infinite, 0);

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			this.timer?.Dispose();
		}

		private void DoWork(object state)
		{
			using (var scope = this.serviceProvider.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
				var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

				var pendingEmails = emailService.GetPendingEmails(this.emailConfiguration.JobLimit);

				foreach (var email in pendingEmails)
				{
					bool isSent = emailService.SendEmail(email, this.emailConfiguration);
					foreach (EmailAddressee addressee in email.Addressees)
					{
						if (addressee.Status == EmailStatus.InProcess)
						{
							addressee.Status = isSent ? EmailStatus.Sent : EmailStatus.Failed;
						}
					}
				}

				dbContext.SaveChanges();
			}
		}
	}
}
