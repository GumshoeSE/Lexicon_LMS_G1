using System.ComponentModel.DataAnnotations;

namespace Lexicon_LMS_G1.Entities.Dtos
{
    public record ActivityUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
