
# Handlebars.Net.Helpers
Several helpers for [Handlebars.Net](https://github.com/rexm/Handlebars.Net).

[![NuGet](https://buildstats.info/nuget/Handlebars.Net.Helpers)](https://www.nuget.org/packages/Handlebars.Net.Helpers)

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
```handlebars
{{String.Append "foobar" "bar"}}
```

#### With an additional custom prefix
If you would like to use the helpers with a custom prefix, you need to register the helpers using this code:
``` c#
HandlebarsHelpers.Register(handlebarsContext, true, "custom");
```

Now you can only access the helpers by using the custom prefix, category prefix and the name like:
```handlebars
{{custom.String.Append "foobar" "bar"}}
```

#### Without a prefix
If you would like to use the helpers without a prefix, so just by name, use this code:
``` c#
HandlebarsHelpers.Register(handlebarsContext, false);
```

Now you can only access the helpers by just using the name like:
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

***
## References
- Thanks to https://github.com/rexm/Handlebars.Net
- Some ideas based on https://github.com/helpers/handlebars-helpers
- Some code based on https://www.30secondsofcode.org/c-sharp/t/string/p/1
- Some documentation based on https://github.com/arinet/HandlebarDocs