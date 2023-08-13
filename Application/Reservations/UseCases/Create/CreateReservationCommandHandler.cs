using Application.Reservations.UseCases.Cancel;
using Domain.Customers.Aggregate;
using Domain.Reservations.Aggregate;
using Domain.Reservations.Repositories;
using Domain.Shared.Entities;
using Domain.Shared.Proxies;
using SharedKernal.CQRS.Commands;
using SharedKernal.Repositories;
using SharedKernal.Utilities.Errors;
using SharedKernal.Utilities.Result;

namespace Application.Reservations.UseCases.Create;
internal class CreateReservationCommandHandler : ICommandHandler<CreateReservationCommand, CreateReservationResponse>
{

	private readonly IUnitOfWork _unitOfWork;
	private readonly IReservationRepository _reservationRepository;
	private readonly CustomerRepositoryProxy _customerRepositoryProxy;
	private readonly MealRepositoryProxy _mealRepositoryProxy;
	private readonly PricingRepositoryProxy _pricingRepositoryProxy;

	public CreateReservationCommandHandler(IUnitOfWork unitOfWork,
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

	public async Task<Result<CreateReservationResponse>> Handle(CreateReservationCommand request,
		CancellationToken cancellationToken)
	{
		try
		{
			MealEntry? entry = _mealRepositoryProxy.GetMealEntry(request.orderedMealId);

			if (entry is null || entry.Meal is null)
			{
				return Result.Failure<CreateReservationResponse>(new Error("entry.Meal is null", "entry.Meal is null"));
			}

			Customer? customer = _customerRepositoryProxy.GetCustomerById(request.customerId);

			if (customer is null)
			{
				return Result.Failure<CreateReservationResponse>(new Error("(customer is null)", "(customer is null)"));
			}

			PricingRecord? pricingRecord = _pricingRepositoryProxy
				.GetPriceByCustomerTypeJoinMealType(customer.Category, entry.Meal.Type);


			if (pricingRecord is null)
			{
				return Result.Failure<CreateReservationResponse>(new Error("(pricingRecord is null)", "(pricingRecord is null)"));
			}

			bool isCustomerHasAReservationOnThisEntry =
				_reservationRepository.CheckIfCustomerHasAMealReservation(request.customerId,
						request.orderedMealId);

			Reservation? firstReservationOnWaitingToCancel =
				_reservationRepository.GetFirstOnWaitingToCancel(entry.Id);


			Reservation reservation;
			if (firstReservationOnWaitingToCancel is null)
			{
				reservation = Reservation.Create(entry, customer,
												 pricingRecord, request.customerId,
												 request.orderedMealId,
												 isCustomerHasAReservationOnThisEntry);
			}
			else
			{
				(Reservation, Reservation) exchangableReservations = Reservation.Exchange(entry, customer,
											 pricingRecord, request.customerId,
											 request.orderedMealId,
											 isCustomerHasAReservationOnThisEntry,
											 firstReservationOnWaitingToCancel);

				reservation = exchangableReservations.Item1;
				
				firstReservationOnWaitingToCancel = exchangableReservations.Item2;
			
				_reservationRepository.Update(firstReservationOnWaitingToCancel);
			}

			_reservationRepository.Add(reservation);

			await _unitOfWork.SaveChangesAsync();

			return Result.Success(new CreateReservationResponse(reservation.ReservationStatus, reservation.Id));
		}

		catch (Exception exception)
		{
			return Result.Failure<CreateReservationResponse>(new Error("", exception.Message));
		}
	}
}
