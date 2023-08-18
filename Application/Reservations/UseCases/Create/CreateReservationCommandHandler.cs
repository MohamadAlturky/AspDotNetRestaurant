using Application.Reservations.UseCases.Cancel;
using Domain.Customers.Aggregate;
using Domain.MealEntries.Aggregate;
using Domain.Pricing.Aggregate;
using Domain.Reservations.Aggregate;
using Domain.Reservations.Repositories;
using Domain.Reservations.Services;
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
	private readonly IReservationsService _reservationsService;
	private readonly CustomerRepositoryProxy _customerRepositoryProxy;
	private readonly MealEntryRepositoryProxy _mealRepositoryProxy;
	private readonly PricingRepositoryProxy _pricingRepositoryProxy;

	public CreateReservationCommandHandler(IUnitOfWork unitOfWork,
		IReservationRepository reservationRepository,
		CustomerRepositoryProxy customerRepositoryProxy,
		MealEntryRepositoryProxy mealRepositoryProxy,
		PricingRepositoryProxy pricingRepositoryProxy,
		IReservationsService reservationsService)
	{
		_unitOfWork = unitOfWork;
		_reservationRepository = reservationRepository;
		_customerRepositoryProxy = customerRepositoryProxy;
		_mealRepositoryProxy = mealRepositoryProxy;
		_pricingRepositoryProxy = pricingRepositoryProxy;
		_reservationsService = reservationsService;
	}

	public async Task<Result<CreateReservationResponse>> Handle(CreateReservationCommand request,
		CancellationToken cancellationToken)
	{
		try
		{
			MealEntry? entry = _mealRepositoryProxy.GetMealEntry(request.orderedMealId);

			if (entry is null || entry.MealInformation is null)
			{
				return Result.Failure<CreateReservationResponse>(new Error("entry.Meal is null", "entry.Meal is null"));
			}

			Customer? customer = _customerRepositoryProxy.GetCustomerById(request.customerId);

			if (customer is null)
			{
				return Result.Failure<CreateReservationResponse>(new Error("(customer is null)", "(customer is null)"));
			}

			PricingRecord? pricingRecord = _pricingRepositoryProxy
				.GetPriceByCustomerTypeJoinMealType(customer.Category, entry.MealInformation.Type);


			if (pricingRecord is null)
			{
				return Result.Failure<CreateReservationResponse>(new Error("(pricingRecord is null)", "(pricingRecord is null)"));
			}

			bool isCustomerHasAReservationOnThisEntry =
				_reservationRepository.CheckIfCustomerHasAMealReservation(request.customerId,
						request.orderedMealId);

			Reservation? firstQualifiedReservationOnWaitingToCancel =
				_reservationRepository.GetFirstOnWaitingToCancelWhereHisBalanceIsEnough(entry.Id);


			Reservation reservation;
			if (firstQualifiedReservationOnWaitingToCancel is null)
			{
				reservation = _reservationsService.Create(entry, customer,
												 pricingRecord, request.customerId,
												 request.orderedMealId,
												 isCustomerHasAReservationOnThisEntry);
			}
			else
			{
				(Reservation, Reservation) exchangableReservations = _reservationsService.CreateButExchange(entry, customer,
											 pricingRecord, request.customerId,
											 request.orderedMealId,
											 isCustomerHasAReservationOnThisEntry,
											 firstQualifiedReservationOnWaitingToCancel);

				reservation = exchangableReservations.Item1;
				
				firstQualifiedReservationOnWaitingToCancel = exchangableReservations.Item2;
			
				_reservationRepository.Update(firstQualifiedReservationOnWaitingToCancel);
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
