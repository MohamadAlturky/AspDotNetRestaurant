using Domain.Shared.Entities;
using SharedKernal.CQRS.Queries;

namespace Application.Meals.UseCases.GetMealEntries;
public sealed record GetMealEntriesQuery(long mealId): IQuery<List<MealEntry>>;
