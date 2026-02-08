using System.Linq.Expressions;

namespace RaspberryPi.Domain.Core;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Criteria { get; }
    Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; }
    bool AsNoTracking { get; }

    int? Skip { get; }
    int? Take { get; }
}