using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotNote.ViewModel.Helpers
{
    public class DatabaseHelper
    {
        private static string dbFile = Path.Combine(Environment.CurrentDirectory, "dbNotes.db3");
    
        public static bool Insert<T>(T item)
        {
            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>(); // ensure table exists
                int rows = conn.Insert(item);

                if (rows > 0) return true;
            }

            return false;
        }
    
        public static bool Update<T>(T item)
        {
            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>(); // ensure table exists
                int rows = conn.Update(item);

                if (rows > 0) return true;
            }

            return false;
        }
    
        public static List<T> GetAll<T>() where T : new()
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
