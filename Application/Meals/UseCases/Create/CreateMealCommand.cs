using Domain.Meals.Entities;
using SharedKernal.CQRS.Commands;

namespace Application.UseCases.Meals.Create;
public sealed record CreateMealCommand(MealInformation Meal) : ICommand;
