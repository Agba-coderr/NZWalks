using NZWalks.API.Models.Enums;

namespace NZWalks.API.Models.Domain
{
    public class Walk : NamedEntity
    {
        // Inherits Id and Name from NamedEntity

        public required string Description { get; set; }

        public double LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }

        public Guid RegionId { get; set; }

        // who created this walk
        public string CreatedByUserId { get; set; } = string.Empty;

        // Navigation Properties
        public required DifficultyType DifficultyType { get; set; }

        public required Region Region { get; set; }
    }
}