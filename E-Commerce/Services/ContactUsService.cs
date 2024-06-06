using E_Commerce.Models;
using E_Commerce.Repositories;

namespace E_Commerce.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly IContactUsRepo repo;

        public ContactUsService(IContactUsRepo repo)
        {
            this.repo = repo;
        }
        public int AddMessage(ContactUs message)
        {
         return  repo.AddMessage(message);
        }

        public int DeleteMessage(int id)
        {
          return  repo.DeleteMessage(id);
        }

        public IEnumerable<ContactUs> GetAllMessages()
        {
        return repo.GetAllMessages();
        }

        public ContactUs GetMessageById(int id)
        {
          return repo.GetMessageById(id);
        }

        public void Save()
        {
         repo.Save();
        }
    }
}
