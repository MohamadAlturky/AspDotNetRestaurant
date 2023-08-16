using Domain.Meals.Entities;
using SharedKernal.CQRS.Commands;

namespace Application.Meals.UseCases.Update;
public sealed record UpdateMealCommand(MealInformation meal) : ICommand;
