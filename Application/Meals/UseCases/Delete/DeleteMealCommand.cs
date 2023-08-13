using SharedKernal.CQRS.Commands;

namespace Application.Meals.UseCases.Delete;
public sealed record DeleteMealCommand(long mealId) : ICommand;
