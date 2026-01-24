using Ardalis.Specification;

namespace GlowGirlsBackend.Interfaces;

public interface IRepository<T> : IRepositoryBase<T>
    where T : class;
