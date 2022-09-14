using AutoMapper;
using Blessings.Order.Core.Persistence;
using Blessings.OrdersApi.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Blessings.Order.Core.CQRS.Queries.GetSet;

public class GetSetQueryHandler : IRequestHandler<GetSetQuery, List<SetResponse>>
{

    private readonly ISetRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetSetQueryHandler> _logger;

    public GetSetQueryHandler(ISetRepository repository, IMapper mapper, ILogger<GetSetQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<SetResponse>> Handle(GetSetQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handle Set query");
        var sets = await _repository.GetAllAsync();

        var result = _mapper.Map<List<SetResponse>>(sets);

        return result;
    }
}