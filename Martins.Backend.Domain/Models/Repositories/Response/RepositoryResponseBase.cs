namespace Martins.Backend.Domain.Models.Repositories.Response
{
    public class RepositoryResponseBase<TData>
    {
        public TData? Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
