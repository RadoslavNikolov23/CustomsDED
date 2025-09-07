namespace CustomsDED.Data.Repository.Contracts
{
    using CustomsDED.Data.Models;

    public interface IPersonRepository : IBaseAsyncRepository<Person>
    {
        Task<IEnumerable<Person>> GetAllPersonsByDate(DateTime pickedDate);
    }
}
