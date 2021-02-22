namespace S4N.SuCorrientazoADomicilio.Dto.Validation
{
    /// <summary>
    /// Data transfer object for validations
    /// </summary>
    public class ValidationResultDto
    {
        /// <summary>
        /// Validation errors / warnings that apply to the validated item
        /// </summary>

        public ValidationConditionDto Conditions { get; set; } = new ValidationConditionDto();
    }
}
