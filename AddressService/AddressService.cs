using DatabaseApplication;

namespace AddressService
{
    public class AddressService
    {
        public int AddAddress(Address address)
        {
            int retVal = -1;
            DbUtilities.ConcurrentExecute((DbApplication db) =>
            {
                db.Addresses.Add(address);
                db.SaveChanges();
                retVal = address.Id;
            });

            return retVal;
        }

        public void RemoveAddress(int id)
        {
            DbUtilities.ConcurrentExecute((DbApplication db) =>
            {
                var address = db.Addresses.Find(id);
                if (address != null)
                {
                    db.Addresses.Remove(address);
                    db.SaveChanges();
                }
            });
        }

        public Address GetAddressById(int id)
        {
            var res = DbUtilities.ConcurrentQuery((DbApplication db) =>
            {
                return db.Addresses.Where(u => id == u.Id);
            });
            return res.FirstOrDefault();
        }

        public IEnumerable<Address> GetAllAddresses()
        {
            var res = DbUtilities.ConcurrentQuery((DbApplication db) =>
            {
                return db.Addresses;
            });

            return res.ToList();
        }

        public void ClearAddresses()
        {
            using var db = new DbApplication();
            db.Addresses.RemoveRange(db.Addresses.ToList());
            db.SaveChanges();
        }
    }
}