using Domain.Customers.Aggregate;
using SharedKernal.CQRS.LogableQuery;
using SharedKernal.CQRS.Queries;
using SharedKernal.Utilities.Result;

namespace Application.Customers.UseCases.GetAll;
public sealed record GetAllCustomersQuery(): ILogableQuery<Result<List<Customer>>>;
