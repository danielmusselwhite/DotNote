using DotNote.Model;

namespace DotNote.ViewModel.Helpers.DatabaseHelpers
{
    public interface IDatabaseHelper
    {
        public Task<bool> Insert<T>(T item);

        public Task<bool> Update<T>(T item) where T : IHasId, new();

        public Task<bool> Delete<T>(T item) where T : IHasId, new();

        public Task<List<T>> GetAll<T>() where T : IHasId, new();
    }
}
