# Handlebars.Net.Helpers
Several helpers which can be used for:
- [Handlebars.Net](https://github.com/rexm/Handlebars.Net)
- [Handlebars.CSharp](https://github.com/zjklee/Handlebars.CSharp) (Note that this library follows [handlebarsjs](https://handlebarsjs.com/) more strictly.)

## Project
| | |
| --- | --- |
| **Build Azure** | [![Build Status Azure](https://dev.azure.com/stef/Handlebars.Net.Helpers/_apis/build/status/CI?branchName=master)](https://dev.azure.com/stef/Handlebars.Net.Helpers/_build/latest?definitionId=36&branchName=master) |
| **Sonar Quality** | [![Sonar Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=Handlebars.Net.Helpers&metric=alert_status)](https://sonarcloud.io/project/issues?id=Handlebars.Net.Helpers) [![Sonar Bugs](https://sonarcloud.io/api/project_badges/measure?project=Handlebars.Net.Helpers&metric=bugs)](https://sonarcloud.io/project/issues?id=Handlebars.Net.Helpers&resolved=false&types=BUG) [![Sonar Code Smells](https://sonarcloud.io/api/project_badges/measure?project=Handlebars.Net.Helpers&metric=code_smells)](https://sonarcloud.io/project/issues?id=Handlebars.Net.Helpers&resolved=false&types=CODE_SMELL)|
| **Coverage** | [![Sonar Coverage](https://sonarcloud.io/api/project_badges/measure?project=Handlebars.Net.Helpers&metric=coverage)](https://sonarcloud.io/component_measures?id=Handlebars.Net.Helpers&metric=coverage) [![codecov](https://codecov.io/gh/StefH/Handlebars.Net.Helpers/branch/master/graph/badge.svg)](https://codecov.io/gh/StefH/Handlebars.Net.Helpers) |
| **Packages** | |
| &nbsp;&nbsp;Handlebars.Net.Helpers | [![NuGet](https://buildstats.info/nuget/Handlebars.Net.Helpers)](https://www.nuget.org/packages/Handlebars.Net.Helpers) |
| &nbsp;&nbsp;Handlebars.CSharp.Helpers | [![NuGet](https://buildstats.info/nuget/Handlebars.CSharp.Helpers)](https://www.nuget.org/packages/Handlebars.CSharp.Helpers)|
| &nbsp;&nbsp;MyGet previews | [![MyGet](https://buildstats.info/myget/handlebars_net_helpers/Handlebars.Net.Helpers)](https://www.myget.org/feed/handlebars_net_helpers/package/nuget/Handlebars.Net.Helpers) [![MyGet](https://buildstats.info/myget/handlebars_net_helpers/Handlebars.Net.Helpers)](https://www.myget.org/feed/handlebars_net_helpers/package/nuget/Handlebars.CSharp.Helpers) [Info](https://github.com/StefH/Handlebars.Net.Helpers/wiki/MyGet)|

## Framework support
- .NET Framework 4.5.1 and 4.5.2 `*`
- .NET Standard 1.3, 2.0 and 2.1

`*` : not all functionality works on .NET 4.5.1 due to an older version from Handlebars.Net

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

#### Without a prefix
If you would like to use the helpers without a prefix, so just by name, use this code:
``` c#
HandlebarsHelpers.Register(handlebarsContext, options => { UseCategoryPrefix = false; });
```

Now you can use it like:
``` handlebars
{{[String-Append] "foobar" "bar"}}
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

***
## References
- Thanks to https://github.com/rexm/Handlebars.Net
- Some ideas based on https://github.com/helpers/handlebars-helpers
- Some code based on https://www.30secondsofcode.org/c-sharp/t/string/p/1
- Some documentation based on https://github.com/arinet/HandlebarDocs