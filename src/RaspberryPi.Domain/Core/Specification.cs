using System.Linq.Expressions;

namespace RaspberryPi.Domain.Core;

public abstract class Specification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>>? Criteria { get; protected set; }
    public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; protected set; }
    public bool AsNoTracking { get; protected set; } = true;

    public int? Skip { get; protected set; }
    public int? Take { get; protected set; }

    protected void ApplyCriteria(Expression<Func<T, bool>> criteria) => Criteria = criteria;
    protected void ApplyOrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy) => OrderBy = orderBy;

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
    }

    protected void DisableNoTracking() => AsNoTracking = false;
}