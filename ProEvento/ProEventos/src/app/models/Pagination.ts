export class Pagination {

  currentPage: number = 0;
  itemsPerPage: number = 0;
  totalItems: number = 0;
  TotalPages: number = 0;
}

export class PaginatedResult<T> {
  result: T | undefined | null;
  pagination: Pagination = new Pagination;
}
