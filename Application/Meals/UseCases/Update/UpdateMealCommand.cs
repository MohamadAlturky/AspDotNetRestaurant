using Domain.Meals.Aggregate;
using SharedKernal.CQRS.Commands;

namespace Application.Meals.UseCases.Update;
public sealed record UpdateMealCommand(Meal meal) : ICommand;
