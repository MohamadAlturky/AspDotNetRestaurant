using Domain.Shared.Entities;
using SharedKernal.CQRS.Queries;

namespace Application.Meals.UseCases.GetMealEntriesByDate;
public sealed record GetMealEntriesByDateQuery(DateOnly date) : IQuery<List<MealEntry>>;
