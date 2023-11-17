using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseApplication
{
    public static class DbUtilities
    {
        /// <summary>
        /// Lock per la concorrenza
        /// </summary>
        public static ReaderWriterLockSlim ConcurrentLock { get; } = new ReaderWriterLockSlim();

        /// <summary>
        /// Funzione per la gestione della concorrenza in Sqlite
        /// </summary>
        /// <typeparam name="T">Dbcontext</typeparam>
        /// <param name="action">Azione da eseguire sul db</param>
        /// <param name="db">Istanza del db, se null la crea ex novo</param>
        public static void ConcurrentExecute<T>(Action<T> action , T db = null)
            where T : DbContext, new()
        {
            ConcurrentLock.EnterWriteLock();
            try
            {
                if (db == null)
                {

                    using var dbContext = new T();
                    action?.Invoke(dbContext);
                }
                else
                {
                    action?.Invoke(db);
                }
            }
            finally { ConcurrentLock.ExitWriteLock(); }

        }

        public static IEnumerable<U> ConcurrentQuery<T, U>(Func<T, IQueryable<U>> query)
        where T : DbContext, new()
        {
            var retVal = Enumerable.Empty<U>();
            try
            {
                ConcurrentLock.EnterReadLock();
                using var dbContext = new T();
                retVal = query?.Invoke(dbContext).ToList();
            }
            finally { ConcurrentLock.ExitReadLock(); }

            return retVal;
        }
    }
}
