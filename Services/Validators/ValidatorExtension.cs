namespace Services.Validators
{
    using FluentValidation;
    using System;

    public static class ValidatorExtension
    {
        public static IRuleBuilderOptions<T, TProperty> Required<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return ruleBuilder.NotNull().NotEmpty().WithMessage("{PropertyName} is required.");
        }

        public static IRuleBuilderOptions<T, string> IsAlphaNumeric<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            /*
             * [^a-zA-Z\d\s:]
                \d - numeric class
                \s - whitespace
                a-zA-Z - matches all the letters
                ^ - negates them all - so you get - non numeric chars, non spaces and non colons
             * */
            return ruleBuilder.Matches("^[a-zA-Z0-9 ]*$").WithMessage("{PropertyName} should only contain alpha numeric characters.");
        }

        public static IRuleBuilderOptions<T, string> IsAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            // "^[a-zA-Z0-9_-,;\\s]*$"
            return ruleBuilder.Matches("^[a-zA-Z0-9 ,-;_\\s]*$").WithMessage(@"These (!""#$%&^:~*()\@><}{][-_+) special characters are not permitted as part of {PropertyName}.");
        }

        public static IRuleBuilderOptions<T, string> IsZipCode<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches("^[0-9\\-]*$").WithMessage(@"{PropertyName} is not a valid Zip Code.");
        }

        public static IRuleBuilderOptions<T, string> IsSSN<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches("^[0-9]*$").Must(x => string.IsNullOrEmpty(x) ? true : x.Length == 9).WithMessage(@"{PropertyName} is not a valid SSN.");
        }

        public static IRuleBuilderOptions<T, string> IsGuid<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(x => !string.IsNullOrEmpty(x) ? Guid.TryParse(x, out _) : true).WithMessage("{PropertyName} is not in correct format. Should be a Guid.");
        }

        public static IRuleBuilderOptions<T, string> HasMaximumLength<T>(this IRuleBuilder<T, string> ruleBuilder, int maximumLength = 50)
        {
            return ruleBuilder.MaximumLength(maximumLength).WithMessage("{PropertyName} characters length should not exceed {MaxLength}.");
        }

        public static IRuleBuilderOptions<T, string> HasManimumLength<T>(this IRuleBuilder<T, string> ruleBuilder, int minimumLength = 3)
        {
            return ruleBuilder.MinimumLength(minimumLength).WithMessage("{PropertyName} should have minimum {MinLength} characters.");
        }

        public static IRuleBuilderOptions<T, DateTime> HasNotManimumValue<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder.Must(x => x != DateTime.MinValue).WithMessage("{PropertyName} does not have valid Date.");
        }

        public static IRuleBuilderOptions<T, DateTime?> HasNotManimumValue<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
        {
            return ruleBuilder.Must(x => x.HasValue ? x.Value != DateTime.MinValue : true).WithMessage("{PropertyName} does not have valid Date.");
        }
    }
}
