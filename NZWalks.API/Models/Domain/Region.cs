namespace NZWalks.API.Models.Domain
{
    public class Region : NamedEntity
    {
        // Inherits Id and Name from NamedEntity

        public required string Code { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
