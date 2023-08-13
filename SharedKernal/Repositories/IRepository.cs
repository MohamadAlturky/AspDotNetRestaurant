using SharedKernal.Entities;

namespace SharedKernal.Repositories;
public interface IRepository<T> where T : IAggregateRoot
{
	IEnumerable<T> GetAll();
	IEnumerable<T> GetPage(int pageSize, int pageNumber);
	T? GetById(long id);


	void Add(T Entity);
	void Update(T Entity);
	void Delete(T Entity);
}
