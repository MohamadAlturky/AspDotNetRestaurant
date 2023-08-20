using Infrastructure.Authentication.Models;
using Infrastructure.DataAccess.UserPersistence;
using Infrastructure.ForgetPasswordHandling.Models;
using Infrastructure.ForgetPasswordHandling.Repository;
using Infrastructure.ForgetPasswordHandling.VerificationCodeGenerators;
using Infrastructure.Mail.Abstraction;
using Infrastructure.Mail.Configuration;
using Infrastructure.Mail.Model;
using Microsoft.AspNetCore.Mvc;
using SharedKernal.Repositories;
using SharedKernal.Utilities.Result;

namespace Infrastructure.ForgetPasswordHandling.ForgetPasswordServices;
internal class ForgetPasswordService : IForgetPasswordService
{

	private readonly IEmailSender _emailSender;
	private readonly IUserPersistenceService _userPersistenceService;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IVerificationCodeGenerator _verificationCodeGenerator;
	private readonly IForgetPasswordRepository _forgetPasswordRepository;

	public ForgetPasswordService(IEmailSender emailSender,
				IUserPersistenceService userPersistenceService,
				IUnitOfWork unitOfWork,
				IVerificationCodeGenerator verificationCodeGenerator,
				IForgetPasswordRepository forgetPasswordRepository)
	{
		_emailSender = emailSender;
		_userPersistenceService = userPersistenceService;
		_unitOfWork = unitOfWork;
		_verificationCodeGenerator = verificationCodeGenerator;
		_forgetPasswordRepository = forgetPasswordRepository;
	}
	
	public async Task<Result<long>> SendCodeVIAMailAsync(int serialNumber)
	{
		try
		{
			string code = _verificationCodeGenerator.GenerateVerificationCode();
			User? user = _userPersistenceService.GetUser(serialNumber);

			if (user is null)
			{
				throw new Exception("if(user is null)");
			}


			ForgetPasswordEntry entry = new ForgetPasswordEntry()
			{
				Email = user.HiastMail,
				SerialNumber = serialNumber,
				ValidationToken = code,
				UserId = user.Id
			};
			_forgetPasswordRepository.SaveInforamtion(entry);
			await _unitOfWork.SaveChangesAsync();

			await _emailSender.SendEmailAsync(new EmailMessage()
			{
				Content = code,
				Subject = "ForgetPassword"

			}, new MailAccount()
			{
				Email = user.HiastMail
			});

			return Result.Success(entry.Id);
		}
		catch (Exception exception)
		{
			return Result.Failure<long>(new SharedKernal.Utilities.Errors.Error("", exception.Message));
		}
	}

	public async Task<Result> VerifyCode(long entryId, string verificationCode)
	{
		Models.ForgetPasswordEntry? entry = await _forgetPasswordRepository.GetForgetPasswordEntryAsync(entryId);

		if (entry is null)
		{
			return Result.Failure(new SharedKernal.Utilities.Errors.Error("", "if(entry is null)"));
		}
		if (entry.ValidationToken == verificationCode)
		{
			return Result.Success();
		}

		return Result.Failure(new SharedKernal.Utilities.Errors.Error("", "nooo if (entry.ValidationToken == verificationCode)"));
	}
}
