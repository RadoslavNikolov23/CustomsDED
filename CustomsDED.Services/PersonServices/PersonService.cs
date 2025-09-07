namespace CustomsDED.Services.PersonServices
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CustomsDED.Data.Models;
    using CustomsDED.Data.Repository.Contracts;
    using CustomsDED.DTOs.PersonDTOs;
    using CustomsDED.Services.PersonServices.Contract;

    public class PersonService : IPersonService
    {
        private readonly IPersonRepository personRepo;

        public PersonService(IPersonRepository personRepo)
        {
            this.personRepo = personRepo;
        }

        public async Task<bool> AddPersonAsync(PersonAddDTO personDTO)
        {
            bool isAdded = false;

            if (personDTO != null)
            {
                Person person = new Person()
                {
                    FirstName = personDTO.FirstName,
                    MiddleName = personDTO.MiddleName,
                    LastName = personDTO.LastName,
                    DateOfBirth = personDTO.DateOfBirth,
                    Nationality = personDTO.Nationality,
                    PersonalNumber = personDTO.PersonalNumber,
                    SexType = personDTO.SexType,
                    DocumentNumber = personDTO.DocumentNumber,
                    DocumentType = personDTO.DocumentType,
                    ExpirationDate = personDTO.ExpirationDate,
                    IssuingCountry = personDTO.IssuingCountry,
                    AdditionInfo = personDTO.AdditionInfo,
                    DateOfInspection = DateTime.UtcNow,
                };

                isAdded = await personRepo.AddAsync(person);

            }

            return isAdded;
        }

        public async Task<ICollection<PersonGetTextDTO>> GetPersonsByTextAsync(string textInput)
        {
            IEnumerable<Person> personEntitiesList =
                                    await personRepo.GetAllPersonsByTextInput(textInput.ToLower());

            ICollection<PersonGetTextDTO> personGetTextDTOsList = new List<PersonGetTextDTO>();

            foreach (Person person in personEntitiesList)
            {
                personGetTextDTOsList.Add(new PersonGetTextDTO()
                {
                    FirstName = person.FirstName,
                    MiddleName = person.MiddleName,
                    LastName = person.LastName,
                    DateOfBirth = person.DateOfBirth,
                    Nationality = person.Nationality,
                    PersonalNumber = person.PersonalNumber,
                    SexType = person.SexType,
                    DocumentNumber = person.DocumentNumber,
                    DocumentType = person.DocumentType,
                    ExpirationDate = person.ExpirationDate,
                    IssuingCountry = person.IssuingCountry,
                    AdditionInfo = person.AdditionInfo,
                    DateOfInspection = person.DateOfInspection,

                });
            }

            return personGetTextDTOsList;
        }

        public async Task<ICollection<PersonGetDateDTO>> GetPersonsByDateAsync(DateTime pickedDate)
        {
            IEnumerable<Person> personEntitiesList =
                                    await personRepo.GetAllPersonsByDate(pickedDate);

            ICollection<PersonGetDateDTO> personGetDateDTOsList = new List<PersonGetDateDTO>();

            foreach (Person person in personEntitiesList)
            {
                personGetDateDTOsList.Add(new PersonGetDateDTO()
                {
                    FirstName = person.FirstName,
                    MiddleName = person.MiddleName,
                    LastName = person.LastName,
                    DateOfBirth = person.DateOfBirth,
                    DateOfInspection = person.DateOfInspection,

                });
            }

            return personGetDateDTOsList;
        }

    }
}
