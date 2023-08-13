
namespace Presentation.ApiModels.PricingRecords;

public class PricingRecordDTO
{
	public long Id { get; set; }
	public int Price { get; set; }
	public string CustomerTypeValue { get; set; } = string.Empty;
	public string MealTypeValue { get; set; } = string.Empty;
}
