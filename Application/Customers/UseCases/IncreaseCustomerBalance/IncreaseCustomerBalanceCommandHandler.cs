using Domain.Customers.Aggregate;
using Domain.Customers.Exceptions;
using Domain.Customers.Repositories;
using SharedKernal.CQRS.Commands;
using SharedKernal.Repositories;
using SharedKernal.Utilities.Result;
using System;

namespace Application.UseCases.Customers.IncreaseCustomerBalance;
internal class IncreaseCustomerBalanceCommandHandler : ICommandHandler<IncreaseCustomerBalanceCommand>
{
	private readonly ICustomerRepository _customerRepository;

	private readonly IUnitOfWork _unitOfWork;

	public IncreaseCustomerBalanceCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
	{
		_customerRepository = customerRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Handle(IncreaseCustomerBalanceCommand request, CancellationToken cancellationToken)
	{
		if (request.valueToAddToTheBalance <= 0)
		{
			return Result.Failure(new SharedKernal.Utilities.Errors.Error("", "if (request.valueToAddToTheBalance <= 0)"));
		}

		try
		{
			Customer? customer = _customerRepository.GetBySerialNumber(request.serialNumber);

			if (customer is null)
			{
				#warning needs refactoring
				throw new Exception();
			}

			customer.IncreaseBalance(request.valueToAddToTheBalance);

			_customerRepository.Update(customer);

			await _unitOfWork.SaveChangesAsync();

			return Result.Success();
		}
		catch (NegativeBalanceException)
		{
			return Result.Failure(new SharedKernal.Utilities.Errors.Error("", ""));

			//return Result.Failure(CustomerErrorsDictionary.DataBaseIgnoredToIncreaseTheBalance("لا يوجد شخص بلرقم الذاتي المطلوب"));
		}
		catch (Exception exception)
		{
			return Result.Failure(new SharedKernal.Utilities.Errors.Error("", exception.Message));
		}
	}
}
