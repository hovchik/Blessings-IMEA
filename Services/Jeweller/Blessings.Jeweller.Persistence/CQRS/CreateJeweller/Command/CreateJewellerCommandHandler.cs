using AutoMapper;
using Blessings.Contract;
using Blessings.Jeweller.Infrastructure.Persistence;
using MediatR;

namespace Blessings.Jeweller.Core.CQRS;

public class CreateJewellerCommandHandler : IRequestHandler<CreateJewellerCommand, CreateJewellerResponse>
{
    private readonly JewellerDbContext _context;
    private readonly IMapper _mapper;

    public CreateJewellerCommandHandler(JewellerDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CreateJewellerResponse> Handle(CreateJewellerCommand request, CancellationToken cancellationToken)
    {
        var jewellerEntity = new Domain.Jeweller
        {
            Name = request.Name,
            IsAvailable = request.IsAvailable
        };
        await _context.Jewellers.AddAsync(jewellerEntity,cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<CreateJewellerResponse>(jewellerEntity);

        return result;
    }
}