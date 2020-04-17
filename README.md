
# Handlebars.Net.Helpers
Several helpers for [Handlebars.Net](https://github.com/rexm/Handlebars.Net).

[![NuGet](https://buildstats.info/nuget/Handlebars.Net.Helpers)](https://www.nuget.org/packages/Handlebars.Net.Helpers)

***

## Math

#### Add

#### Abs

#### Avg

#### Ceil

#### Divide

#### Math.E

#### Floor

#### Max

#### Min

#### Minus

#### Modulo

#### Multiply

#### Math.PI

#### Plus

#### Pow

#### Round

#### Sign

#### Sqrt

#### Subtract

#### Sum

#### Times

***
## Regex

#### IsMatch
``` handlebars
{{#IsMatch \"Hello\" \"Hello\"}}
yes
{{else}}
no
{{/IsMatch}}
<!-- results in: 'yes' -->
```

***
## String

#### Append
``` handlebars
// given that "item.x" is "foo"
{{Append item.x ".html"}}
// results in: "foo.html"
```

#### Capitalize

#### Ellipsis

#### IsString
``` handlebars
{{isString "foo"}}
<!-- results in: 'true' -->
```

#### Prepend

#### Reverse

#### Replace

#### StartsWith
``` handlebars
{{#startsWith "Goodbye" "Hello, world!"}}
  no
{{else}}
  yes
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


