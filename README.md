<div id="top"></div>

[![Github][github-shield]][action-url]
[![codecov][codecov-shield]][codecov-url]
[![release][release-shield]][release-url]
[![pre-release][pre-release-shield]][release-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]



<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/idarb-oss/Options.FluentValidation">
    <!--<img src="images/logo.png" alt="Logo" width="80" height="80">-->
  </a>

<h3 align="center">Options.FluentValidation</h3>

  <p align="center">
    Extensions for <a href="https://www.nuget.org/packages/Microsoft.Extensions.Options/">Microsoft.Extensions.Options</a> by using <a href="https://www.nuget.org/packages/FluentValidation">FluentValidation</a> to validate your options at runtime.
    <br />
    <a href="https://alinea.idar-oss.com/"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <!--<a href="https://github.com/idarb-oss/Options.FluentValidation">View Demo</a>-->
    <a href="https://github.com/idarb-oss/Options.FluentValidation/issues">Report Bug</a>
    ·
    <a href="https://github.com/idarb-oss/Options.FluentValidation/issues">Request Feature</a>
  </p>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
      <ul>
        <li><a href="#how-to-use">How to use</a></li>
      </ul>
    </li>
    <li><a href="#license">License</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

Options.FluentValidation adds extension methods on top of Microsoft.Extensions.Options to be able to use FluentValidation for validating your options.

```csharp
services.AddOptions<TestOption>()
    .Bind(config.GetSection("Options"))
    .ValidateFluently();
```

<p align="right">(<a href="#top">back to top</a>)</p>



### Built With

* [.NET](https://dotnet.microsoft.com/)
* [FluentValidation](https://docs.fluentvalidation.net/en/latest/)

<p align="right">(<a href="#top">back to top</a>)</p>


### How to use

Start by creation your option record:

```csharp
namespace Options;

public record AppOptions
{
    public const string SectionName = "App";

    public string Name { get; init; }

    public int WorkerCount { get; init; }
}
```

Create an FluentValidation class:

```csharp
using FluentValidation;

namespace Options;

public class AppOptionsValidation : AbstractValidator<AppOptions>
{
    public AppOptionsValidation()
    {
        RuleFor(o => o.Name)
            .NotEmpty()
            .NotNull();

        RuleFor(o => o.WorkerCount)
            .GreaterThan(0)
            .LessThan(20);
    }
}
```

Then add the option to your `IServiceCollection` by following:

```csharp
using Options.FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Options;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration config)
    {
        services.AddOptions<AppOptions>()
            .Bind(config.GetRequiredSection(AppOptions.SectionName))
            .ValidateFluently()
            .ValidateOnStart();  // Add Microsoft.Extensions.Hosting package

        return services;
    }
}
```

Then you can add the configuration to your `json` file or any other means of setting up your configurations.

```json
{
    "App": {
        "Name": "My fancy app",
        "WorkerCount": 15
    }
}
```


<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.

<p align="right">(<a href="#top">back to top</a>)</p>


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[stars-url]: https://github.com/idarb-oss/Options.FluentValidation/stargazers

[issues-shield]: https://img.shields.io/github/issues/idarb-oss/Options.FluentValidation.svg?style=for-the-badge
[issues-url]: https://github.com/idarb-oss/Options.FluentValidation/issues

[license-shield]: https://img.shields.io/github/license/idarb-oss/Options.FluentValidation.svg?style=for-the-badge
[license-url]: https://github.com/idarb-oss/Options.FluentValidation/blob/master/LICENSE

[codecov-shield]: https://img.shields.io/codecov/c/github/idarb-oss/Options.FluentValidation/main?style=for-the-badge&token=1TU3O38DYG
[codecov-url]: https://codecov.io/gh/idarb-oss/Options.FluentValidation

[github-shield]: https://img.shields.io/github/actions/workflow/status/idarb-oss/Options.FluentValidation/dotnet.yaml?style=for-the-badge
[action-url]: https://github.com/idarb-oss/Options.FluentValidation/actions/workflows/dotnet.yaml

[release-shield]: https://img.shields.io/github/v/release/idarb-oss/Options.FluentValidation?include_prereleases&style=for-the-badge
[pre-release-shield]: https://img.shields.io/github/v/release/idarb-oss/Options.FluentValidation?include_prereleases&label=pre%20release&style=for-the-badge
[release-url]: https://github.com/idarb-oss/Options.FluentValidation/releases

[product-screenshot]: images/screenshot.png
