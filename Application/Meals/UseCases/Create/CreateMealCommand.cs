using Domain.Meals.Aggregate;
using SharedKernal.CQRS.Commands;

namespace Application.UseCases.Meals.Create;
public sealed record CreateMealCommand(Meal Meal) : ICommand;
