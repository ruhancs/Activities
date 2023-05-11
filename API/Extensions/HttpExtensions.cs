

using System.Text.Json;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(
            this HttpResponse response,
            int currentPage,
            int itemsPerPAge,
            int totalItems,
            int totalPages
        )
        {
            var paginationHeader = new 
            {
                currentPage,
                itemsPerPAge,
                totalItems,
                totalPages
            };
            //tranformar em json Pagination e a chave do header
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}