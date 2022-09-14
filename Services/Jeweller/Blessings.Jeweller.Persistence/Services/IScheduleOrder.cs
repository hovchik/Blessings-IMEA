using Blessings.Jeweller.Domain;

namespace Blessings.JewellerApi.Services;

public interface IScheduleOrder<T> where T : EntityBase
{
    Task Add(T schedule, CancellationToken cancellationToken);
    Task<T> GetById(int id, CancellationToken cancellationToken);
    Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken);
    Task Update(T schedule, CancellationToken cancellationToken);
    Task Delete(T schedule, CancellationToken cancellationToken);
}