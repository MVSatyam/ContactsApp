using ContactsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsApp.Repositories

{
    public interface IContactRepository
    {
        ContactModel ContactDetails(int id);
        List<ContactModel> AllContacts();
        void CreateContact(ContactModel contact);
        void Update(int id, ContactModel contact);
        void DeleteContact(int id);
    }
}
