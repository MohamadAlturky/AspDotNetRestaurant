using SharedKernal.ValueObjects;

namespace Domain.Customers.ValueObjects;
public class SerialNumber : ValueObject<int>
{
	public SerialNumber(int value) : base(value) { }

	protected override void Validate(int value)
	{
		// no validation needed
	}
}
