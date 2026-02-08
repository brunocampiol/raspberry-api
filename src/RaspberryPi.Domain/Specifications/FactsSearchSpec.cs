using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Models;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Domain.Specifications;

public sealed class FactsSearchSpec : Specification<Fact>
{
    public FactsSearchSpec(FactQuery q)
    {
        var page = q.Page < 1 ? 1 : q.Page;
        var pageSize = q.PageSize is < 1 ? 20 : q.PageSize;
        if (pageSize > 200) pageSize = 200;
        var skip = (page - 1) * pageSize;

        ApplyCriteria(f =>
            (string.IsNullOrWhiteSpace(q.TextContains) || f.Text.ToUpper().Contains(q.TextContains.ToUpper().Trim()))
            &&
            (!q.CreatedFromUtc.HasValue || f.CreatedAt >= q.CreatedFromUtc.Value)
            &&
            (!q.CreatedToUtc.HasValue || f.CreatedAt <= q.CreatedToUtc.Value)
        );

        ApplyOrderBy(src => src.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id));

        ApplyPaging(skip, pageSize);
    }
}