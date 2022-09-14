using Blessings.OrdersApi.Models;
using MediatR;

namespace Blessings.Order.Core.CQRS.Queries.GetSet;

public class GetSetQuery : IRequest<List<SetResponse>>
{

}