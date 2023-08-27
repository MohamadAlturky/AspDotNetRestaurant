namespace Application.IdentityChecker;
public interface IExecutorIdentityProvider
{
	string GetExecutorSerialNumber();
	string GetExecutorId();
	string GetMacAddress();
}
