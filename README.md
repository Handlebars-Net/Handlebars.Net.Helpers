# Handlebars.Net.Helpers
Several helpers which can be used for [Handlebars.Net](https://github.com/rexm/Handlebars.Net)

## Project
| | |
| --- | --- |
| **Build Azure** | [![Build Status Azure](https://dev.azure.com/stef/Handlebars.Net.Helpers/_apis/build/status/CI?branchName=master)](https://dev.azure.com/stef/Handlebars.Net.Helpers/_build/latest?definitionId=36&branchName=master) |
| **Sonar Quality** | [![Sonar Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=Handlebars.Net.Helpers&metric=alert_status)](https://sonarcloud.io/project/issues?id=Handlebars.Net.Helpers) [![Sonar Bugs](https://sonarcloud.io/api/project_badges/measure?project=Handlebars.Net.Helpers&metric=bugs)](https://sonarcloud.io/project/issues?id=Handlebars.Net.Helpers&resolved=false&types=BUG) [![Sonar Code Smells](https://sonarcloud.io/api/project_badges/measure?project=Handlebars.Net.Helpers&metric=code_smells)](https://sonarcloud.io/project/issues?id=Handlebars.Net.Helpers&resolved=false&types=CODE_SMELL)|
| **Coverage** | [![Sonar Coverage](https://sonarcloud.io/api/project_badges/measure?project=Handlebars.Net.Helpers&metric=coverage)](https://sonarcloud.io/component_measures?id=Handlebars.Net.Helpers&metric=coverage) [![codecov](https://codecov.io/gh/StefH/Handlebars.Net.Helpers/branch/master/graph/badge.svg)](https://codecov.io/gh/StefH/Handlebars.Net.Helpers) |

## Packages

| Package | Nuget | MyGet [:information_source:](https://github.com/StefH/Handlebars.Net.Helpers/wiki/MyGet) |
| --- | --- | --- |
| Handlebars.Net.Helpers |[![NuGet](https://img.shields.io/nuget/v/Handlebars.Net.Helpers)](https://www.nuget.org/packages/Handlebars.Net.Helpers) | [![MyGet](https://img.shields.io/myget/handlebars_net_helpers/vpre/Handlebars.Net.Helpers?label=MyGet)](https://www.myget.org/feed/handlebars_net_helpers/package/nuget/Handlebars.Net.Helpers)|
| Handlebars.Net.Helpers.DynamicLinq |[![NuGet](https://img.shields.io/nuget/v/Handlebars.Net.Helpers.DynamicLinq)](https://www.nuget.org/packages/Handlebars.Net.Helpers.DynamicLinq) | [![MyGet](https://img.shields.io/myget/handlebars_net_helpers/vpre/Handlebars.Net.Helpers.DynamicLinq?label=MyGet)](https://www.myget.org/feed/handlebars_net_helpers/package/nuget/Handlebars.Net.Helpers.DynamicLinq)|
| Handlebars.Net.Helpers.Humanizer |[![NuGet](https://img.shields.io/nuget/v/Handlebars.Net.Helpers.Humanizer)](https://www.nuget.org/packages/Handlebars.Net.Helpers.Humanizer) | [![MyGet](https://img.shields.io/myget/handlebars_net_helpers/vpre/Handlebars.Net.Helpers.Humanizer?label=MyGet)](https://www.myget.org/feed/handlebars_net_helpers/package/nuget/Handlebars.Net.Helpers.Humanizer)|
| Handlebars.Net.Helpers.Json |[![NuGet](https://img.shields.io/nuget/v/Handlebars.Net.Helpers.Json)](https://www.nuget.org/packages/Handlebars.Net.Helpers.Json) | [![MyGet](https://img.shields.io/myget/handlebars_net_helpers/vpre/Handlebars.Net.Helpers.Json?label=MyGet)](https://www.myget.org/feed/handlebars_net_helpers/package/nuget/Handlebars.Net.Helpers.Json)|
| Handlebars.Net.Helpers.Random |[![NuGet](https://img.shields.io/nuget/v/Handlebars.Net.Helpers.Random)](https://www.nuget.org/packages/Handlebars.Net.Helpers.Random) | [![MyGet](https://img.shields.io/myget/handlebars_net_helpers/vpre/Handlebars.Net.Helpers.Random?label=MyGet)](https://www.myget.org/feed/handlebars_net_helpers/package/nuget/Handlebars.Net.Helpers.Random)|
| Handlebars.Net.Helpers.Xeger |[![NuGet](https://img.shields.io/nuget/v/Handlebars.Net.Helpers.Xeger)](https://www.nuget.org/packages/Handlebars.Net.Helpers.Xeger) | [![MyGet](https://img.shields.io/myget/handlebars_net_helpers/vpre/Handlebars.Net.Helpers.Xeger?label=MyGet)](https://www.myget.org/feed/handlebars_net_helpers/package/nuget/Handlebars.Net.Helpers.Xeger)|
| Handlebars.Net.Helpers.XPath |[![NuGet](https://img.shields.io/nuget/v/Handlebars.Net.Helpers.XPath)](https://www.nuget.org/packages/Handlebars.Net.Helpers.XPath) | [![MyGet](https://img.shields.io/myget/handlebars_net_helpers/vpre/Handlebars.Net.Helpers.XPath?label=MyGet)](https://www.myget.org/feed/handlebars_net_helpers/package/nuget/Handlebars.Net.Helpers.XPath)|
| Handlebars.Net.Helpers.Xslt |[![NuGet](https://img.shields.io/nuget/v/Handlebars.Net.Helpers.Xslt)](https://www.nuget.org/packages/Handlebars.Net.Helpers.Xslt) | [![MyGet](https://img.shields.io/myget/handlebars_net_helpers/vpre/Handlebars.Net.Helpers.Xslt?label=MyGet)](https://www.myget.org/feed/handlebars_net_helpers/package/nuget/Handlebars.Net.Helpers.Xslt)|

## Framework support
- .NET Framework 4.5.1 4.5.2 and 4.6
- .NET Standard 1.3, 2.0 and 2.1
- .NET 6.0 and .NET 8.0

## :exclamation: Breaking changes

### 2.5.0
Some breaking changes are introduced in this version:

#### A. EnvironmentHelpers
By default, the category `Environment` is not automatically registered due to potential security issues.
You need to allow this via the HandlebarsHelpersOptions.

#### B. System.Linq.Dynamic.Core
By default, the category `DynamicLinq` is not automatically registered due to [a CVE in System.Linq.Dynamic.Core DynamicLinq](https://github.com/zzzprojects/System.Linq.Dynamic.Core/issues/867).
This means that the NuGet *Handlebars.Net.Helpers.DynamicLinq* will not be loaded and registred automatically anymore. 
You need to allow this via the HandlebarsHelpersOptions.

## Usage

### Register

#### Get all helpers
``` c#
var handlebarsContext = HandlebarsDotNet.Handlebars.Create();
HandlebarsHelpers.Register(handlebarsContext);
```

#### Get a specific helper
``` c#
var handlebarsContext = HandlebarsDotNet.Handlebars.Create();
HandlebarsHelpers.Register(handlebarsContext, Category.String);
```

#### Get multiple helpers
``` c#
var handlebarsContext = HandlebarsDotNet.Handlebars.Create();
HandlebarsHelpers.Register(handlebarsContext, Category.Math, Category.String);
```

### Using

#### With a category prefix (default)
By default you can use the helpers by using a prefix from the category:
``` handlebars
{{[String.Append] "foobar" "bar"}}
```

#### With an additional custom prefix
If you would like to use the helpers with a custom prefix, you need to register the helpers using this code:
``` c#
HandlebarsHelpers.Register(handlebarsContext, options => { Prefix = "custom"; });
```

Now you can only access the helpers by using the custom prefix, category prefix and the name like:
```handlebars
{{[custom.String.Append] "foobar" "bar"}}
```

#### With a custom prefix separator character
By default the dot (`.`) character is used, use the code below to use a different value:

``` c#
HandlebarsHelpers.Register(handlebarsContext, options => { PrefixSeparator = "-"; });
```

Now you can only access the helpers by using the `-` separator like this:
```handlebars
{{[String-Append] "foobar" "bar"}}
```

#### Without a prefix
If you would like to use the helpers without a prefix, so just by name, use this code:
``` c#
HandlebarsHelpers.Register(handlebarsContext, options => { UseCategoryPrefix = false; });
```

Now you can use it like:
``` handlebars
{{String-Append "foobar" "bar"}}
```

Now you can access the helpers by just using the name like:
```handlebars
{{Append "foobar" "bar"}}
```

***

The following default built-in helpers are available:
- [Boolean](https://github.com/StefH/Handlebars.Net.Helpers/wiki/Boolean)
- [DateTime](https://github.com/StefH/Handlebars.Net.Helpers/wiki/DateTime)
- [Dictionary](https://github.com/StefH/Handlebars.Net.Helpers/wiki/Dictionary)
- [Constants](https://github.com/StefH/Handlebars.Net.Helpers/wiki/Constants)
- [Enumerable](https://github.com/StefH/Handlebars.Net.Helpers/wiki/Enumerable)
- [Math](https://github.com/StefH/Handlebars.Net.Helpers/wiki/Math)
- [Regex](https://github.com/StefH/Handlebars.Net.Helpers/wiki/Regex)
- [String](https://github.com/StefH/Handlebars.Net.Helpers/wiki/String)
- [Url](https://github.com/StefH/Handlebars.Net.Helpers/wiki/Url)

And the following additonal helpers are available
- [DynamicLinq](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/wiki/DynamicLinq)
- [Humanizer](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/wiki/Humanizer)
- [Json](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/wiki/Json)
- [Random](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/wiki/Random)
- [Xeger](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/wiki/Xeger)
- [XPath](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/wiki/XPath)
- [Xslt](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/wiki/Xslt)

***
## References
- Thanks to https://github.com/rexm/Handlebars.Net
- Some ideas based on https://github.com/helpers/handlebars-helpers
- Some code based on https://www.30secondsofcode.org/c-sharp/t/string/p/1
- Some documentation based on https://github.com/arinet/HandlebarDocs
- SimpleJson copied from (https://github.com/facebook-csharp-sdk/simple-json)
