namespace Infrastructure.ForgetPasswordHandling.VerificationCodeGenerators;
internal class VerificationCodeGenerator : IVerificationCodeGenerator
{
	private readonly Random _random = new Random();

	public VerificationCodeGenerator()
	{
	}

	public string GenerateVerificationCode()
	{
		string code = "";
		for(int index = 0; index < 6; index++)
		{
			code += _random.Next(1,9).ToString();
		}
		return code;
	}
}
