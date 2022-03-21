using Lexicon_LMS_G1.Entities.Entities;

namespace Lexicon_LMS_G1.Entities.ViewModels
{
    public class PaginatedList<T> : List<T>
    {
        public int Id { get; }
        public string Name { get; } = "Name";
        public string Description { get; } = "Description";
        public string StartTime { get; } = "Start Date";


        public int CurrentPageIndex { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public bool HasPrevious => CurrentPageIndex > 1;
        public bool HasNext => CurrentPageIndex < TotalPages;

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            CurrentPageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public static async Task<PaginatedList<T>> CreateAsync(List<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return new PaginatedList<T>(items.ToList(), count, pageIndex, pageSize);
        }
    }
}
