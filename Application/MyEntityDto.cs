namespace Application
{
    public record MyEntityDto
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }
    }
}
