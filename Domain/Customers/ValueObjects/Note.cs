using SharedKernal.ValueObjects;

namespace Domain.Customers.ValueObjects;
public class Note : ValueObject<string>
{
	public Note(string value) : base(value) { }

	protected override void Validate(string value)
	{
		// no validation needed
	}
}
