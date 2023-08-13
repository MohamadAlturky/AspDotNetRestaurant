using SharedKernal.CQRS.Commands;

namespace Application.Meals.UseCases.CancellationPermissions;
public sealed record EditCancellationPermissionsCommand(
	long mealId, long preparedMealId, bool cancellationState) : ICommand;
