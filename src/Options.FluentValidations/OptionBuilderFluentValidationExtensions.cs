using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Options.FluentValidation;

/// <summary>
/// Extension methods for OptionBuilder to add validation by using FluentValidation.
/// </summary>
public static class OptionBuilderFluentValidationExtensions
{
    /// <summary>
    /// Extension method for the <see cref="OptionsBuilder{TOptions}"/> for validating with FluentValidation.
    /// </summary>
    /// <typeparam name="TOptions">Option type to validate</typeparam>
    /// <param name="optionsBuilder">The OptionBuilder the validation should be performed on.</param>
    /// <returns><see cref="OptionsBuilder{TOptions}"/></returns>
    public static OptionsBuilder<TOptions> ValidateFluently<TOptions>(
        this OptionsBuilder<TOptions> optionsBuilder
    )
        where TOptions : class
    {
        optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(
            s =>
                new FluentValidationOptions<TOptions>(
                    optionsBuilder.Name,
                    s.GetRequiredService<IValidator<TOptions>>()
                )
        );
        return optionsBuilder;
    }
}

/// <inheritdoc cref="IValidateOptions{TOptions}"/>
public class FluentValidationOptions<TOptions> : IValidateOptions<TOptions>
    where TOptions : class
{
    private readonly IValidator<TOptions> _validator;

    /// <summary>
    /// Creates an new FluentValidationOptions to run validations with.
    /// </summary>
    /// <param name="name">The name of the name of the validation</param>
    /// <param name="validator">The FluentValidation validator to validate with.</param>
    public FluentValidationOptions(string? name, IValidator<TOptions> validator)
    {
        Name = name;
        _validator = validator;
    }

    /// <summary>
    /// Name of the Option validator
    /// </summary>
    public string? Name { get; }

    /// <inheritdoc cref="IValidateOptions{TOptions}.Validate(string?, TOptions)"/>
    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        if (Name != null && Name != name)
            return ValidateOptionsResult.Skip;

        if (options == null)
            throw new ArgumentNullException(nameof(options));

        var validationResult = _validator.Validate(options);

        if (validationResult.IsValid)
            return ValidateOptionsResult.Success;

        var errors = validationResult.Errors.Select(
            x => $"Options validation failed for {x.PropertyName} with error: {x.ErrorMessage}"
        );

        return ValidateOptionsResult.Fail(errors);
    }
}
