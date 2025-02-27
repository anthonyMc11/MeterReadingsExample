
namespace MeterReadings.Repositories
{
    /// <summary>
    /// due to time constraints, I have chosen to use a seeded inmemory list
    /// </summary>
    public class AccountsListRepository : IRepository<Account>
    {
        private readonly List<Account> _db = [];

        public void Add(Account entity)
        {
            _db.Add(entity);
        }

        public void Delete(Account entity)
        {
            _db.Remove(entity);
        }

        public IEnumerable<Account> GetAll()
        {
            return _db; 
        }

        public Account? GetById(int id)
        {
            return _db.FirstOrDefault(x => x.Id == id);
        }

        public void Update(Account entity)
        {
            throw new NotImplementedException();
        }
    }
}
