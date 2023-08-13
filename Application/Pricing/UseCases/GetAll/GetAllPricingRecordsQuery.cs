using Domain.Shared.Entities;
using SharedKernal.CQRS.Queries;

namespace Application.Pricing.UseCases.GetAll;
public sealed record GetAllPricingRecordsQuery() : IQuery<List<PricingRecord>>;
