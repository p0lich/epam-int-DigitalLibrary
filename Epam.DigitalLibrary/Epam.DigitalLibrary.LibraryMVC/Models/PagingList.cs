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

        public bool PreviousPage
        {
            get
            {
                return PageIndex > 1;
            }
        }

        public bool NextPage
        {
            get
            {
                return PageIndex > PagesCount;
            }
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
    }
}
