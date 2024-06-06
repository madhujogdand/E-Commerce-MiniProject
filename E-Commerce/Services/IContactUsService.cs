using E_Commerce.Models;

namespace E_Commerce.Services
{
    public interface IContactUsService
    {

        IEnumerable<ContactUs> GetAllMessages();
        ContactUs GetMessageById(int id);
        int AddMessage(ContactUs message);
        int DeleteMessage(int id);
        void Save();
    }
}
