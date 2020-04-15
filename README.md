
# Handlebars.Net.Helpers
Several helpers for [Handlebars.Net](https://github.com/rexm/Handlebars.Net).

[![NuGet](https://buildstats.info/nuget/Handlebars.Net.Helpers)](https://www.nuget.org/packages/Handlebars.Net.Helpers)

## String

#### Append
``` handlebars
// given that "item.x" is "foo"
{{Append item.x ".html"}}
// results in: "foo.html"
```

#### Capitalize

#### Ellipsis

#### Prepend

#### Reverse

#### Replace

#### StartsWith
``` handlebars
{{#startsWith "Goodbye" "Hello, world!"}}
  Whoops
{{else}}
  Bro, do you even hello world?
{{/startsWith}}
```

#### ToCamelCase

#### ToLower

#### ToPascalCase

#### ToUpper

#### Trim

#### TrimEnd

#### TrimStart

#### Truncate


## Math

#### Abs
``` handlebars
{{Abs -1}}
// results in: 1
```

#### Max

#### Min

#### Sign