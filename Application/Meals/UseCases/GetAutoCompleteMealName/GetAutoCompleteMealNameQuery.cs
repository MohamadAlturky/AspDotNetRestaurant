using Domain.Meals.ValueObjects;
using SharedKernal.CQRS.Queries;

namespace Application.Meals.UseCases.GetAutoCompleteMealName;
public record GetAutoCompleteMealNameQuery(string partOfMealName,MealType mealType) 
	:IQuery<List<AutoCompleteModel>>;
