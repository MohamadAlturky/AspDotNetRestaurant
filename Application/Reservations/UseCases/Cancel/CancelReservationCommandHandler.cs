using Domain.Customers.Aggregate;
using Domain.Meals.Aggregate;
using Domain.Reservations.Aggregate;
using Domain.Reservations.Repositories;
using Domain.Shared.Proxies;
using SharedKernal.CQRS.Commands;
using SharedKernal.Repositories;
using SharedKernal.Utilities.Errors;
using SharedKernal.Utilities.Result;

namespace Application.Reservations.UseCases.Cancel;
internal class CancelReservationCommandHandler : ICommandHandler<CancelReservationCommand, string>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IReservationRepository _reservationRepository;
	private readonly CustomerRepositoryProxy _customerRepositoryProxy;
	private readonly MealRepositoryProxy _mealRepositoryProxy;
	private readonly PricingRepositoryProxy _pricingRepositoryProxy;

	public CancelReservationCommandHandler(IUnitOfWork unitOfWork,
		IReservationRepository reservationRepository,
		CustomerRepositoryProxy customerRepositoryProxy,
		MealRepositoryProxy mealRepositoryProxy,
		PricingRepositoryProxy pricingRepositoryProxy)
	{
		_unitOfWork = unitOfWork;
		_reservationRepository = reservationRepository;
		_customerRepositoryProxy = customerRepositoryProxy;
		_mealRepositoryProxy = mealRepositoryProxy;
		_pricingRepositoryProxy = pricingRepositoryProxy;
	}

	public async Task<Result<string>> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
	{
		try
		{
			Reservation? reservation = _reservationRepository.GetById(request.reservationId);

			if (reservation is null)
			{
				return Result.Failure<string>(new Error("if (reservation is null)", "if (reservation is null)"));
			}

			MealEntry? entry = _mealRepositoryProxy.GetMealEntry(reservation.MealEntryId);

			if (entry is null)
			{
				return Result.Failure<string>(new Error("entry is null", "entry is null"));
			}

			Customer? customer = _customerRepositoryProxy.GetCustomerById(reservation.CustomerId);

			if (customer is null)
			{
				return Result.Failure<string>(new Error("if (customer is null)", "if (customer is null)"));
			}

			Reservation? firstWaitingReservation = _reservationRepository.GetFirstWaitingReservationsOnEntry(entry.Id);



			if (firstWaitingReservation is not null)
			{
				reservation.CancelAndGiveMealTo(entry,customer, firstWaitingReservation);
				_reservationRepository.Update(reservation);
				_reservationRepository.Update(firstWaitingReservation);
			}
			else
			{
				reservation.Cancel(entry, customer);
				_reservationRepository.Update(reservation);
			}

			await _unitOfWork.SaveChangesAsync();

			return Result.Success(reservation.ReservationStatus);
		}

		catch (Exception exception)
		{
			return Result.Failure<string>(new Error("___", exception.Message));
		}
	}
}
