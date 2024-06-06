using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repositories
{
    public class ContactUsRepo : IContactUsRepo
    {
        private readonly ApplicationDbContext db;

        public ContactUsRepo(ApplicationDbContext db)
        {
            this.db = db;
        }
        public int AddMessage(ContactUs message)
        {


            int result = 0;
            db.ContactUs.Add(message);
            result = db.SaveChanges();
            return result;

        }

        public int DeleteMessage(int id)
        {

           
            var model = db.ContactUs.Where(contactus => contactus.Id == id).FirstOrDefault();
            if (model != null)
            {
                db.ContactUs.Remove(model);
                return db.SaveChanges();
            }
            return 0;
        }

        public IEnumerable<ContactUs> GetAllMessages()
        {
            return db.ContactUs.ToList(); 
        }

        public ContactUs GetMessageById(int id)
        {
            return db.ContactUs.Find(id);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
