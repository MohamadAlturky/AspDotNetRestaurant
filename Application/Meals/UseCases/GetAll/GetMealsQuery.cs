using Domain.Meals.Entities;
using SharedKernal.CQRS.Queries;

namespace Application.UseCases.Meals.GetAll;
public sealed record GetMealsQuery() : IQuery<List<MealInformation>>;

