using Microsoft.AspNetCore.Http;
using ProEventos.Api.Models;
using System.Text.Json;

namespace ProEventos.Api.Extensions
{
    public static class Pagination
    {
        public static void AddPagination(this HttpResponse response, int CurrentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var pagination = new PaginationHeaders(CurrentPage, itemsPerPage, totalItems, totalPages);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            response.Headers.Add("Pagination", JsonSerializer.Serialize(pagination, options));

            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
