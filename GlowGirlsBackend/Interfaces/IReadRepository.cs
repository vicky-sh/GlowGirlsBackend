using Ardalis.Specification;

namespace GlowGirlsBackend.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T>
    where T : class;