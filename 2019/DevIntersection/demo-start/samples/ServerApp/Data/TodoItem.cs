using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerApp
{
    public class TodoItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsDone { get; set; }

        public DateTime CreatedAt { get; set; }

        public static IEnumerable<TodoItem> Sort(SortOrder sort, IEnumerable<TodoItem> items)
        {
            return sort == SortOrder.Ascending ?
                items.OrderBy(i => i.CreatedAt) :
                items.OrderByDescending(i => i.CreatedAt);
        }
    }

    public enum SortOrder
    {
        Ascending,
        Descending,
    }
}
