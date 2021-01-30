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
| Handlebars.Net.Helpers |[![NuGet](https://buildstats.info/nuget/Handlebars.Net.Helpers)](https://www.nuget.org/packages/Handlebars.Net.Helpers) | [![MyGet](https://buildstats.info/myget/handlebars_net_helpers/Handlebars.Net.Helpers?includePreReleases=true)](https://www.myget.org/feed/handlebars_net_helpers/package/nuget/Handlebars.Net.Helpers)|
| Handlebars.Net.Helpers.Json |[![NuGet](https://buildstats.info/nuget/Handlebars.Net.Helpers.Json)](https://www.nuget.org/packages/Handlebars.Net.Helpers.Json) | [![MyGet](https://buildstats.info/myget/handlebars_net_helpers/Handlebars.Net.Helpers.Json?includePreReleases=true)](https://www.myget.org/feed/handlebars_net_helpers/package/nuget/Handlebars.Net.Helpers.Json)|
| Handlebars.Net.Helpers.DynamicLinq |[![NuGet](https://buildstats.info/nuget/Handlebars.Net.Helpers.DynamicLinq)](https://www.nuget.org/packages/Handlebars.Net.Helpers.DynamicLinq) | [![MyGet](https://buildstats.info/myget/handlebars_net_helpers/Handlebars.Net.Helpers.DynamicLinq?includePreReleases=true)](https://www.myget.org/feed/handlebars_net_helpers/package/nuget/Handlebars.Net.Helpers.DynamicLinq)|
| Handlebars.Net.Helpers.XPath |[![NuGet](https://buildstats.info/nuget/Handlebars.Net.Helpers.XPath)](https://www.nuget.org/packages/Handlebars.Net.Helpers.XPath) | [![MyGet](https://buildstats.info/myget/handlebars_net_helpers/Handlebars.Net.Helpers.XPath?includePreReleases=true)](https://www.myget.org/feed/handlebars_net_helpers/package/nuget/Handlebars.Net.Helpers.XPath)|
| Handlebars.Net.Helpers.Xeger |[![NuGet](https://buildstats.info/nuget/Handlebars.Net.Helpers.Xeger)](https://www.nuget.org/packages/Handlebars.Net.Helpers.Xeger) | [![MyGet](https://buildstats.info/myget/handlebars_net_helpers/Handlebars.Net.Helpers.Xeger?includePreReleases=true)](https://www.myget.org/feed/handlebars_net_helpers/package/nuget/Handlebars.Net.Helpers.Xeger)|
| Handlebars.Net.Helpers.Random |[![NuGet](https://buildstats.info/nuget/Handlebars.Net.Helpers.Random)](https://www.nuget.org/packages/Handlebars.Net.Helpers.Random) | [![MyGet](https://buildstats.info/myget/handlebars_net_helpers/Handlebars.Net.Helpers.Random?includePreReleases=true)](https://www.myget.org/feed/handlebars_net_helpers/package/nuget/Handlebars.Net.Helpers.Random)|

## Framework support
- .NET Framework 4.5.1 and 4.5.2
- .NET Standard 1.3, 2.0 and 2.1

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

The following helpers are available:
- [Constants](https://github.com/StefH/Handlebars.Net.Helpers/wiki/Constants)
- [Enumerable](https://github.com/StefH/Handlebars.Net.Helpers/wiki/Enumerable)
- [Math](https://github.com/StefH/Handlebars.Net.Helpers/wiki/Math)
- [Regex](https://github.com/StefH/Handlebars.Net.Helpers/wiki/Regex)
- [String](https://github.com/StefH/Handlebars.Net.Helpers/wiki/String)
- [Url](https://github.com/StefH/Handlebars.Net.Helpers/wiki/Url)
- [DateTime](https://github.com/StefH/Handlebars.Net.Helpers/wiki/DateTime)

***
## References
- Thanks to https://github.com/rexm/Handlebars.Net
- Some ideas based on https://github.com/helpers/handlebars-helpers
- Some code based on https://www.30secondsofcode.org/c-sharp/t/string/p/1
- Some documentation based on https://github.com/arinet/HandlebarDocs
- SimpleJson copied from (https://github.com/facebook-csharp-sdk/simple-json)