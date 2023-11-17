using DatabaseApplication;

namespace UserService
{
    public class UserService
    {
        public UserService()
        {
        }

        public void ClearUsers()
        {
            using var db = new DbApplication();
            db.Users.RemoveRange(db.Users.ToList());
            db.SaveChanges();
        }

        public int AddUser(User user)
        {
            int retVal = -1;
            DbUtilities.ConcurrentExecute((DbApplication db) =>
            {
                db.Users.Add(user);
                db.SaveChanges();
                retVal = user.Id;
            });

            return retVal;
        }

        public void RemoveUser(int userId)
        {
            DbUtilities.ConcurrentExecute((DbApplication db) =>
            {
                var dbUser = db.Users.Find(userId);
                if (dbUser != null)
                {
                    db.Users.Remove(dbUser);
                    db.SaveChanges();
                }
            });
        }

        public int UpdateUser(User user)
        {
            var retVal = -1;
            DbUtilities.ConcurrentExecute((DbApplication db) =>
            {
                var dbUser = db.Users.SingleOrDefault(u => user.Id == u.Id);
                if (dbUser != null)
                {
                    dbUser.FirstName = user.FirstName;
                    dbUser.LastName = user.LastName;

                    db.SaveChanges();
                    retVal = dbUser.Id;
                }

            });
            return retVal;
        }

        public User GetUserById(int userId)
        {
            var res = DbUtilities.ConcurrentQuery((DbApplication db) =>
            {
                return db.Users.Where(u => userId == u.Id);
            });
            return res.FirstOrDefault();
        }

        public IEnumerable<User> GetAllUsers()
        {
            var res = DbUtilities.ConcurrentQuery((DbApplication db) =>
            {
                return db.Users;
            });

            return res.ToList();
        }
    }
}