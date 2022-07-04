
using System.Collections.Generic;

namespace ChurchManagementSystem.Web.Extentions
{
    public class Error
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class Response<T>
    {
        public T Data { get; set; }
        public bool HasErrors { get; set; }
        public List<Error> Errors { get; set; }
    }

    public class PagedResponse<T>
    {
        public List<T> Items { get; set; }
        public int TotalItemCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
    }

    public class Response
    {
        public bool HasErrors { get; set; }
        public List<Error> Errors { get; set; }
    }
}