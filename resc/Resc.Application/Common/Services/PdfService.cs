using HandlebarsDotNet;
using PuppeteerSharp;
using Resc.Application.Common.Interfaces;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Resc.Application.Common.Services
{
	public class PdfService : IPdfService
	{
		//PUPPETEER VERSION 2.0.0 is the only working version!
		public async Task<MemoryStream> GeneratePdfFile<T>(T payload, byte[] content, bool closeStream = true)
		{
			var outputStream = new MemoryStream();
			string template = Encoding.UTF8.GetString(content, 0, content.Length);
			var templateFunc = Handlebars.Compile(template);

			var fileContent = templateFunc.Invoke(payload);
			await this.Convert(fileContent, outputStream, "\\Resc-chromium");

			if (closeStream)
			{
				outputStream.Close();
				outputStream.Dispose();
			}

			return outputStream;
		}

		private async Task<byte[]> Convert(string content, Stream outputStream, string browserFetcherPath)
		{
			var browserFetcher = new BrowserFetcher(new BrowserFetcherOptions {
				Path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + browserFetcherPath
			});

			await browserFetcher.DownloadAsync(BrowserFetcher.DefaultRevision);

			using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions {
				Headless = true,
				ExecutablePath = browserFetcher.RevisionInfo(BrowserFetcher.DefaultRevision).ExecutablePath
			}))
			{
				var page = await browser.NewPageAsync();
				await page.SetContentAsync(content);

				var pdfStream = await page.PdfDataAsync();
				await outputStream.WriteAsync(pdfStream);
				return pdfStream;
			}
		}
	}
}
