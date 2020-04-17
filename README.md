
# Handlebars.Net.Helpers
Several helpers for [Handlebars.Net](https://github.com/rexm/Handlebars.Net).

[![NuGet](https://buildstats.info/nuget/Handlebars.Net.Helpers)](https://www.nuget.org/packages/Handlebars.Net.Helpers)

## Usage

#### Get all helpers
``` c#
var handlebarsContext = HandlebarsDotNet.Handlebars.Create();
HandleBarsHelpers.Register(handlebarsContext);
```

#### Get a specific helper
``` c#
var handlebarsContext = HandlebarsDotNet.Handlebars.Create();
HandleBarsHelpers.Register(handlebarsContext, HelperType.String);
```

#### Get multiple helpers
``` c#
var handlebarsContext = HandlebarsDotNet.Handlebars.Create();
HandleBarsHelpers.Register(handlebarsContext, HelperType.Math, HelperType.String);
```

***

The following helpers are available:
- [Constants](https://github.com/StefH/Handlebars.Net.Helpers/wiki/Constants)
- [Enumerable](https://github.com/StefH/Handlebars.Net.Helpers/wiki/Enumerable)
- [Math](https://github.com/StefH/Handlebars.Net.Helpers/wiki/Math)
- [Regex](https://github.com/StefH/Handlebars.Net.Helpers/wiki/Regex)
- [String](https://github.com/StefH/Handlebars.Net.Helpers/wiki/String)
