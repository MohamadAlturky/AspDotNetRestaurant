﻿using Infrastructure.Authentication.PasswordHashing;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Presentation.Mappers;
using System.Net.Mail;
using SharedResources;
using SharedResources.RecourcesKeys;
using SharedResources.LocalizationProviders;
using Domain.Shared.Utilities;

namespace Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DevelopmentController : APIController
{
	private readonly IHashHandler _hashHandler;
	private readonly IStringLocalizer<SharedResourcesProvider> _localizer;
	public DevelopmentController(ISender sender, IMapper mapper, IHashHandler hashHandler, IStringLocalizer<SharedResourcesProvider> localizer) : base(sender, mapper)
	{
		_hashHandler = hashHandler;
		_localizer = localizer;
	}
	//[HttpPost("SeedAdmin")]
	//public async Task<IActionResult> SeedAdmin(string password)
	//{
	//	try
	//	{
	//		// admin123 
	//		if (_hashHandler.GetHash(password) != "JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=")
	//		{
	//			throw new Exception("خطأ في كلمة المرور");
	//		}

	//		var response = await _sender.Send(new SeedAdminCommad());

	//		return Ok(response);
	//	}
	//	catch (Exception exception)
	//	{
	//		return BadRequest(exception.Message);
	//	}
	//}

	//[HttpPost("PricingSeeder")]
	//public async Task<IActionResult> SeedPricing(string password)
	//{
	//	try
	//	{
	//		// admin123 
	//		if (_hashHandler.GetHash(password) != "JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=")
	//		{
	//			throw new Exception("خطأ في كلمة المرور");
	//		}

	//		_pricingSeeder.Seed();

	//		return Ok("DONE");
	//	}
	//	catch (Exception exception)
	//	{
	//		return BadRequest(exception.Message);
	//	}
	//}


	[HttpPost("SendEmail")]
	public IActionResult SendEmail()
	{
		try
		{
			var fromAddress = new MailAddress("m799678@gmail.com");
			var toAddress = new MailAddress("alaamayya222@gmail.com");
			//const string fromPassword = "mmm123456789";
			//const string subject = "Test email";
			//const string body = "This is a test email.";

			//var smtpClient = new SmtpClient
			//{
			//	Host = "smtp.gmail.com",
			//	Port = 25,
			//	EnableSsl = false,
			//	DeliveryMethod = SmtpDeliveryMethod.Network,
			//	UseDefaultCredentials = false,
			//	Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
			//};

			//using (var message = new MailMessage(fromAddress, toAddress)
			//{
			//	Subject = subject,
			//	Body = body
			//})
			//{
			//	smtpClient.Send(message);
			//}

			//SmtpClient client = new SmtpClient();

			//MailAddress from = new MailAddress("jane@contoso.com",
			//   "Jane " + (char)0xD8 + " Clayton",
			//System.Text.Encoding.UTF8);


			//MailAddress to = new MailAddress("ben@contoso.com");


			//MailMessage message = new MailMessage(fromAddress, toAddress);
			//message.Body = "This is a test email message sent by an application. ";


			//string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
			//message.Body += Environment.NewLine + someArrows;
			//message.BodyEncoding = System.Text.Encoding.UTF8;
			//message.Subject = "test message 1" + someArrows;
			//message.SubjectEncoding = System.Text.Encoding.UTF8;



			//string userState = "test message1";

			//client.SendAsync(message, userState);


			return Ok();
		}
		catch (Exception exception)
		{
			return BadRequest(exception.Message);
		}
	}


	[HttpGet("TestLocalization")]
	public IActionResult TestLocalization()
	{
		return Ok(_localizer["Name"]);
	}

	[HttpGet("TestLocalizationProvider")]
	public IActionResult TestLocalizationProvider()
	{
		return Ok(LocalizationProvider
			.GetLocalizer()[DomainResourcesKeys.Name]);
	}

	[HttpGet("Week")]
	public IActionResult Week()
	{
		//DateTime dt = DateTime.Now;
		//List<DayOfWeek> dayOfWeeks = new List<DayOfWeek>();
		//foreach (DayOfWeek dayOfWeek in Enum.GetValues(typeof(DayOfWeek)))
		//{
		//	if(dt.DayOfWeek == dayOfWeek)
		//	{
		//		return Ok(dayOfWeek.ToString());
		//	}
		//}
		//return NotFound();

		List<DateTime> dateTimes = new List<DateTime>();
		for(int i = 0; i < 10; i++)
		{
			dateTimes.Add(Date.LastSaturdayFrom(DateTime.Now.AddDays(i)));
		}

		return Ok(dateTimes);
	}
}