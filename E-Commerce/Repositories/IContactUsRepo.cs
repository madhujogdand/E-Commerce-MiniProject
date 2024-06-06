using E_Commerce.Models;

namespace E_Commerce.Repositories
{
    public interface IContactUsRepo
    {
        IEnumerable<ContactUs> GetAllMessages();
        ContactUs GetMessageById(int id);
        int AddMessage(ContactUs message);
        int DeleteMessage(int id);
        void Save();
    }
}
