using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Models
{
    public class PageList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PageList()
        {
        }

        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPage = (int)Math.Ceiling(count / (double)PageSize);
            AddRange(items);
        }

        public static async Task<PageList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();

            /*source começa no indice 0, por isso se faz o pageNumber - 1 para posicionar no indice correto referente a pagina para o usuario 
              exemplo: usuario clica na pagina 2, na matriz o indice seria 1;
              é feito o calculo da pagina atual * o numero de itens por pagina e o resultado será o total de itens a ser pulado no skip
              e o Take pega por fim, o total de itens por pagina, apartir dos itens que foral pulados no Skip
            */
            var items = await source.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageList<T>(items, count, pageNumber, pageSize);
        }
    }
}
