using DotNote.Model;
using SQLite;
using System.IO;

namespace DotNote.ViewModel.Helpers.DatabaseHelpers
{
    public class SQLiteDbHelper : IDatabaseHelper
    {
        private static string dbFile = Path.Combine(Environment.CurrentDirectory, "dbNotes.db3");
    
        public async Task<bool> Insert<T>(T item)
        {
            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>(); // ensure table exists
                int rows = conn.Insert(item);

                if (rows > 0) return true;
            }

            return false;
        }
    
        public async Task<bool> Update<T>(T item) where T : IHasId, new()
        {
            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>(); // ensure table exists
                int rows = conn.Update(item);

                if (rows > 0) return true;
            }

            return false;
        }

        public async Task<bool> Delete<T>(T item) where T : IHasId, new()
        {
            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>(); // ensure table exists
                int rows = conn.Delete(item);
                if (rows > 0) return true;
            }
            return false;
        }

        public async Task<List<T>> GetAll<T>() where T : IHasId, new()
        {
            List<T> items = default;

            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>(); // ensure table exists
                items = conn.Table<T>().ToList();
            }

            return items;
        }
    }
}
