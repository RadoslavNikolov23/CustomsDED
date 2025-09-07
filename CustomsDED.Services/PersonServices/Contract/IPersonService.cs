namespace CustomsDED.Services.PersonServices.Contract
{
    using CustomsDED.DTOs.PersonDTOs;

    public interface IPersonService
    {
        Task<bool> AddPersonAsync(PersonAddDTO personDTO);

        Task<ICollection<PersonGetTextDTO>> GetPersonsByTextAsync(string textInput);

        Task<ICollection<PersonGetDateDTO>> GetPersonsByDateAsync(DateTime pickedDate);
    }
}
