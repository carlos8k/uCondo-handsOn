using System.ComponentModel.DataAnnotations;

namespace uCondo.HandsOn.Domain.Validation
{
    public interface IValidationDto
    {
        ValidationResult IsValid();
    }
}