using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Models
{
    public class PagingList<T> : List<T>
    {
        public int PageIndex { get; set; }
        public int PagesCount { get; set; }

        public PagingList(List<T> items, int itemsCount, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PagesCount = itemsCount / pageSize + 1;
            this.AddRange(items);
        }

        public static PagingList<T> GetPageItems(List<T> items, int pageIndex, int pageSize)
        {
            return new PagingList<T>(
                items.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
                items.Count,
                pageIndex,
                pageSize
                );
        }

        public List<int> GetNavigationIndexes(int currentIndex)
        {
            List<int> navIndexes = new List<int>();

            navIndexes.Add(1);

            if (PagesCount == 1)
            {
                return navIndexes;
            }

            for (int i = (currentIndex - 3 > 1) ?
                currentIndex - 3 : 2;
                i < currentIndex;
                i++)
            {
                navIndexes.Add(i);
            }

            if (currentIndex > 1 && currentIndex < PagesCount)
            {
                navIndexes.Add(currentIndex);
            }

            for (int i = currentIndex + 1;
                i < currentIndex + 4 && i < PagesCount;
                i++)
            {
                navIndexes.Add(i);
            }

            navIndexes.Add(PagesCount);

            return navIndexes;
        }
    }
}
