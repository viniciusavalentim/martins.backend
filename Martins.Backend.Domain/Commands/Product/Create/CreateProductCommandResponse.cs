namespace Martins.Backend.Domain.Commands.Product.Create
{
    public class CreateProductCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
