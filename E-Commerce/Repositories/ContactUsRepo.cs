using E_Commerce.Data;
using E_Commerce.Models;

namespace E_Commerce.Repositories
{
    public class ContactUsRepo : IContactUsRepo
    {
        private readonly ApplicationDbContext db;

        public ContactUsRepo(ApplicationDbContext db)
        {
            this.db = db;
        }
        public int AddMessage(ContactUs contactus)
        {
            int result = 0;
            db.ContactUs.Add(contactus);
            result = db.SaveChanges();
            return result;
        }

        public int DeleteMessage(int id)
        {
            int result = 0;
            var model = db.ContactUs.Where(cont => cont.Id == id).FirstOrDefault();
            if (model != null)
            {
                db.ContactUs.Remove(model);
                result = db.SaveChanges();
            }
            return result;
        }

        public IEnumerable<ContactUs> GetAllMessages()
        {
           return db.ContactUs.ToList();
        }

        public ContactUs GetMessageById(int id)
        {
            return db.ContactUs.Where(x => x.Id == id).SingleOrDefault();
        }
    }
}
