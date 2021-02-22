namespace S4N.SuCorrientazoADomicilio.Dto.Validation
{
    /// <summary>
    /// Data transfer object for validation conditions
    /// </summary>
    public class ValidationConditionDto
    {
        /// <summary>
        /// The message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The severity level (ERROR, INFO, WARNING)
        /// </summary>
        public string Severity { get; set; }
    }
}
