﻿using Domain.Customers.Aggregate;
using Domain.Meals.Aggregate;
using Domain.Shared.Entities;
using Presentation.ApiModels.Customers;
using Presentation.ApiModels.MealEntry;
using Presentation.ApiModels.Meals;
using Presentation.ApiModels.PricingRecords;

namespace Presentation.Mappers;

public class Mapper : IMapper
{

	public MealDTO Map(Meal meal) => new MealDTO
	{
		Id = meal.Id,
		Description = meal.Description,
		Name = meal.Name,
		NumberOfCalories = meal.NumberOfCalories,
		ImagePath = meal.ImagePath,
		Type = meal.Type
	};


	public Meal Map(MealDTO mealDTO) => new Meal(mealDTO.Id)
	{
		Type = mealDTO.Type,
		NumberOfCalories = mealDTO.NumberOfCalories,
		ImagePath = mealDTO.ImagePath,
		Name = mealDTO.Name,
		Description = mealDTO.Description,
	};


	public Customer Map(CustomerDTO customerDTO) => new Customer(customerDTO.Id)
	{
		SerialNumber = customerDTO.SerialNumber,
		Balance = customerDTO.Balance,
		FirstName = customerDTO.FirstName,
		LastName = customerDTO.LastName,
		Category = customerDTO.Category,
		BelongsToDepartment = customerDTO.BelongsToDepartment,
		Notes = customerDTO.Notes,
		IsActive = customerDTO.IsActive,
		IsRegular = customerDTO.IsRegular,
		Eligible = customerDTO.Eligible
	};


	public CustomerDTO Map(Customer customer) => new CustomerDTO()
	{
		Id = customer.Id,
		SerialNumber = customer.SerialNumber,
		Balance = customer.Balance,
		FirstName = customer.FirstName,
		LastName = customer.LastName,
		Category = customer.Category,
		BelongsToDepartment = customer.BelongsToDepartment,
		Notes = customer.Notes,
		IsActive = customer.IsActive,
		IsRegular = customer.IsRegular,
		Eligible = customer.Eligible
	};

	public MealEntry Map(MealEntryDTO entry)
	{
		throw new NotImplementedException();
	}

	public MealEntryDTO Map(MealEntry entry)
	{
		return new MealEntryDTO()
		{
			Id = entry.Id,
			AtDay = entry.AtDay,
			CustomerCanCancel = entry.CustomerCanCancel,
			Meal = entry.Meal is not null? this.Map(entry.Meal) : null,
			MealId = entry.MealId,
			PreparedCount = entry.PreparedCount,
			ReservationsCount = entry.ReservationsCount
		};
	}

	public PricingRecord Map(PricingRecordDTO recordDTO)
	{
		return new PricingRecord()
		{
			Id = recordDTO.Id,
			CustomerTypeValue = recordDTO.CustomerTypeValue,
			MealTypeValue = recordDTO.MealTypeValue,
			Price = recordDTO.Price
		};
	}

	public PricingRecordDTO Map(PricingRecord record)
	{
		return new PricingRecordDTO()
		{
			Id = record.Id,
			CustomerTypeValue = record.CustomerTypeValue,
			MealTypeValue = record.MealTypeValue,
			Price = record.Price
		};
	}
}
