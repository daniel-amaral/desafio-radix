using System.Collections.Generic;

namespace DesafioRadix.Models
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalItens { get; set; }
        public int PageNumber { get; set; }
        public int DataSize { get; set; }
        public int TotalPages { get; set; }

        public PaginatedResult(IEnumerable<T> data, int totalItens, int pageNumber, int dataSize, int totalPages)
        {
            this.Data = data;
            this.TotalItens = totalItens;
            this.PageNumber = pageNumber;
            this.DataSize = dataSize;
            this.TotalPages = totalPages;
        }
    }
}
