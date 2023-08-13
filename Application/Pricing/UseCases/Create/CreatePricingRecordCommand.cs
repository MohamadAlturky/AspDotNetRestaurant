using Domain.Shared.Entities;
using SharedKernal.CQRS.Commands;

namespace Application.Pricing.UseCases.Create;
public sealed record CreatePricingRecordCommand(PricingRecord record) : ICommand;