using Lexicon_LMS_G1.Entities.Entities;

namespace Lexicon_LMS_G1.Entities.Paging
{
    public class PaginatedList<T> : List<T>
    {
        public int CurrentPageIndex { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public bool HasPrevious => CurrentPageIndex > 1;
        public bool HasNext => CurrentPageIndex < TotalPages;
        public Course Course { get; set; }
        public int ActivitiesCount { get; set; }

        public bool ShowHistory { get; set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize, int aCount, Course course = null)
        {
            CurrentPageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Course = course;
            ActivitiesCount = aCount;

            AddRange(items);
        }

        public static async Task<PaginatedList<T>> CreateAsync(List<T> source, int pageIndex, int pageSize, int aCount, Course course = null)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return new PaginatedList<T>(items.ToList(), count, pageIndex, pageSize, aCount, course);
        }
    }
}
