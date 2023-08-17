using Globomantics.Domain;

namespace Globomantics.Infrastructure.Data.Repositories;

public class TodoInMemoryRepository<T> : IRepository<T> where T : Todo
{
	public Task AddAsync(T item)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<T>> AllAsync()
	{
		throw new NotImplementedException();
	}

	public Task<T> FindByAsync(string value)
	{
		throw new NotImplementedException();
	}

	public Task<T> GetAsync(Guid id)
	{
		throw new NotImplementedException();
	}

	public Task SaveChangesAsync()
	{
		throw new NotImplementedException();
	}
}

