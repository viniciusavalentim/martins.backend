namespace Martins.Backend.Domain.Commands.Material
{
    public class CreateMaterialCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
