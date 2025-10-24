namespace Martins.Backend.Domain.Commands.Product.Update
{
    public class UpdateProductCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
