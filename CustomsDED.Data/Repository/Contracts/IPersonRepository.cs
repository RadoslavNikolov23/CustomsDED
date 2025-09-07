namespace CustomsDED.Data.Repository.Contracts
{
    using CustomsDED.Data.Models;
    using System.Threading.Tasks;

    public interface IPersonRepository : IBaseAsyncRepository<Person>
    {
        Task<IEnumerable<Person>> GetAllPersonsByTextInput(string textInput);

        Task<IEnumerable<Person>> GetAllPersonsByDate(DateTime pickedDate);
    }
}
