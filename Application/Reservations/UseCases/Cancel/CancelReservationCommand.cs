using SharedKernal.CQRS.Commands;

namespace Application.Reservations.UseCases.Cancel;
public record CancelReservationCommand(long reservationId) : ICommand<string>;
