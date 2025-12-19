using Ardalis.Specification.EntityFrameworkCore;
using GlowGirlsBackend.Interfaces;
using GlowGirlsData.Database;

namespace GlowGirlsBackend.Repository;

public class ReadRepository<T>(GlowGirlsDbContext dbContext)
    : RepositoryBase<T>(dbContext),
        IReadRepository<T>
    where T : class;