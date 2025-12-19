# 2.5.4 (TBD)
- Fix String.Split to correctly handle multi-character separators (e.g., `"<br />"` now splits on the whole string instead of each character) [bug]
- Add String.First helper to get the first element from an array/collection [enhancement]
- Add String.Last helper to get the last element from an array/collection [enhancement]

# 2.5.3 (13 September 2025)
- [#132](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/132) - Humanizer Truncate should always be used as &quot;Humanizer.Truncate&quot; [bug] contributed by [StefH](https://github.com/StefH)
- [#133](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/133) - Fix SonarCloud [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.5.2 (10 June 2025)
- [#129](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/129) - Update RandomDataGenerator.Net to 1.0.19 [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.5.1 (08 June 2025)
- [#128](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/128) - Support incompatible types in array (merge to common structure) [enhancement] contributed by [StefH](https://github.com/StefH)
- [#104](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/issues/104) - DynamicLinq gets confused on inconsistent JSON objects [bug]

# 2.5.0 (22 April 2025)
- [#112](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/112) - Update System.Linq.Dynamic.Core to version 1.6.0.2 [bug, dependencies] contributed by [StefH](https://github.com/StefH)
- [#123](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/123) - Update GitHub Actions CreateRelease.yml to auto generate release notes [enhancement] contributed by [StefH](https://github.com/StefH)
- [#125](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/125) - Implement default AllowedHandlebarsHelpers [bug] contributed by [StefH](https://github.com/StefH)
- [#127](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/127) - Add Example WebAssembly based on Abies [Test] contributed by [StefH](https://github.com/StefH)

# 2.4.13 (01 March 2025)
- [#122](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/122) - Adding compare helpers to the Object helpers [enhancement] contributed by [fabianosuet](https://github.com/fabianosuet)
- [#121](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/issues/121) - Add generic object compare helpers to the ObjectHelpers [enhancement]

# 2.4.12 (20 February 2025)
- [#115](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/115) - Add New Features to Object and DateTime Helpers contributed by [fabianosuet](https://github.com/fabianosuet)
- [#117](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/117) - Update DateTimeHelper [enhancement] contributed by [StefH](https://github.com/StefH)
- [#118](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/118) - Refactoring DateTime &quot;Add&quot; Helpers and unit tests [enhancement] contributed by [fabianosuet](https://github.com/fabianosuet)
- [#120](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/120) - Fix DateTimeHelper [bug] contributed by [StefH](https://github.com/StefH)
- [#114](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/issues/114) - Add New Features to Object and DateTime Helpers [enhancement]
- [#119](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/issues/119) - DateTime helper registering all helper methods inherited from StringHelper under DateTime category [bug]

# 2.4.10 (25 January 2025)
- [#113](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/113) - Added Enumerable helper Count [enhancement] contributed by [epDugas](https://github.com/epDugas)

# 2.4.9 (21 January 2025)
- [#110](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/110) - Add Category.Custom [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.4.8 (29 December 2024)
- [#109](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/109) - Add PassthroughTextEncoder [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.4.7 (28 November 2024)
- [#106](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/106) - Exclude some code when running in Blazor Webassembly [bug] contributed by [StefH](https://github.com/StefH)
- [#105](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/issues/105) - System.Diagnostic not supported in net8.0-browser [bug]

# 2.4.6 (09 October 2024)
- [#101](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/101) - Upgrade System.Text.RegularExpressions to 4.3.1 to solve CVE [bug] contributed by [StefH](https://github.com/StefH)

# 2.4.5 (12 July 2024)
- [#97](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/97) - Fix loading additional Handlebars.Net.Helpers dll's when running application from commandline using 'dotnet run' [bug] contributed by [StefH](https://github.com/StefH)
- [#96](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/issues/96) - DynamicLinq Helpers Not Registering Correctly in .NET 8.0 WebAPI Project running in Docker [bug]

# 2.4.4 (27 June 2024)
- [#99](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/99) - Add Handlebars.Net.Helpers.Xslt [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.4.3 (12 May 2024)
- [#93](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/93) - Add String.Equals with StringComparison [enhancement] contributed by [StefH](https://github.com/StefH)
- [#95](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/95) - When ThrowOnUnresolvedBindingExpression is False, do not throw when UndefinedBindingResult but use default value [enhancement] contributed by [StefH](https://github.com/StefH)
- [#94](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/issues/94) - UndefinedBindingResult Throws exception when using #String.Equal [bug]

# 2.4.2.1 (18 April 2024)
- [#90](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/90) - Upgrade some NuGets [enhancement] contributed by [StefH](https://github.com/StefH)
- [#91](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/91) - Use version 1.3.12 from System.Linq.Dynamic.Core [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.4.1.5 (12 March 2024)
- [#88](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/88) - Fixed casting problems in ExecuteUtils breaking EnumerableHelpers [bug] contributed by [HenrikHoyer](https://github.com/HenrikHoyer)

# 2.4.1.4 (20 December 2023)
- [#87](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/87) - Update FormatAsString to make the format optional [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.4.1.3 (14 December 2023)
- [#86](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/86) - String.FormatAsString [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.4.1.2 (08 December 2023)
- [#85](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/85) - Workaround for: Random.Generate Type=&quot;Long&quot; [bug] contributed by [StefH](https://github.com/StefH)

# 2.4.1.1 (08 December 2023)
- [#84](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/84) - Workaround for: Random.Generate Type=&quot;Long&quot; [bug] contributed by [StefH](https://github.com/StefH)

# 2.4.1 (26 August 2023)
- [#82](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/82) - DateTime.Format contributed by [StefH](https://github.com/StefH)
- [#81](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/issues/81) - Add DateTime.Format [enhancement]

# 2.4.0 (20 July 2023)
- [#77](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/77) - Fix some unit tests [Test] contributed by [0xced](https://github.com/0xced)
- [#79](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/79) - Add Xeger unit-test [Test] contributed by [StefH](https://github.com/StefH)
- [#80](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/80) - Update XPath SelectNode and SelectNodes  [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.3.16 (27 March 2023)
- [#75](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/75) - Add unit test for String.Titlecase on dynamic [Test] contributed by [StefH](https://github.com/StefH)
- [#76](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/76) - String.Substring helper [enhancement] contributed by [Andras-Csanyi](https://github.com/Andras-Csanyi)
- [#74](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/issues/74) - Adding further String helpers [enhancement]

# 2.3.15 (07 March 2023)
- [#73](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/73) - Update System.Linq.Dynamic.Core [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.3.14 (06 March 2023)
- [#70](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/70) - Add Path.Lookup [enhancement] contributed by [StefH](https://github.com/StefH)
- [#71](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/71) - Update Handlebars.Net.Helpers.DynamicLinq [enhancement] contributed by [StefH](https://github.com/StefH)
- [#72](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/72) - Use Handlebars.Net 2.1.4 [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.3.13 (26 January 2023)
- [#68](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/68) - Make some simple-json code internal + change the namespace [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.3.12 (19 December 2022)
- [#66](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/66) - System.TypeLoadException : Could not load type 'HandlebarsDotNet.Poly&#8230; [bug] contributed by [caoko](https://github.com/caoko)
- [#65](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/issues/65) - System.TypeLoadException : Could not load type 'HandlebarsDotNet.Polyfills.AsyncLocal`1' from assembly 'Handlebars, Version=2.1.2.0, Culture=neutral, PublicKeyToken=22225d0bf33cd661'. [bug]

# 2.3.11 (11 December 2022)
- [#64](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/64) - Add extension method &quot;Evaluate&quot; [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.3.10 (01 December 2022)
- [#63](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/63) - Use latest RandomDataGenerator.Net [bug] contributed by [StefH](https://github.com/StefH)
- [#41](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/issues/41) - Random number generation isn't very random [bug]

# 2.3.9 (25 November 2022)
- [#61](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/61) - Add String.Coalesce [enhancement] contributed by [StefH](https://github.com/StefH)
- [#62](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/62) - allow String equals checks to have null values contributed by [kspearrin](https://github.com/kspearrin)
- [#60](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/issues/60) - Add String.Coalesce [enhancement]

# 2.3.8 (23 November 2022)
- [#57](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/57) - Fix String.Split + update dependencies  [bug] contributed by [StefH](https://github.com/StefH)
- [#58](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/58) - String empty helpers [enhancement] contributed by [kspearrin](https://github.com/kspearrin)
- [#59](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/59) - Added basic boolean helpers [enhancement] contributed by [kspearrin](https://github.com/kspearrin)

# 2.3.7 (05 November 2022)
- [#56](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/56) - Add GetEnvironmentVariable [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.3.6 (03 November 2022)
- [#55](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/55) - PluginLoader does not require file system access [enhancement] contributed by [devkev2403](https://github.com/devkev2403)

# 2.3.5 (26 April 2022)
- [#52](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/52) - Upgrade some NuGet packages [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.3.4 (31 March 2022)
- [#51](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/51) - Upgrade to latest Handlebars.Net [enhancement] contributed by [StefH](https://github.com/StefH)
- [#50](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/issues/50) - Update to the latest Handlebars.Net version [enhancement]

# 2.3.3 (21 February 2022)
- [#49](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/49) - Update System.Linq.Dynamic.Core [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.3.2 (20 February 2022)
- [#48](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/48) - Upgrade System.Linq.Dynamic.Core, Humanizer, RandomDataGenerator.Net and XPath2.Extensions [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.3.1 (05 February 2022)
- [#47](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/47) - Adding Equal and NotEqual for strings. contributed by [tinytownsoftware](https://github.com/tinytownsoftware)

# 2.3.0 (26 January 2022)
- [#45](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/45) - Upgrade Handlebars.Net to latest (2.1.0) [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.2.1 (10 July 2021)
- [#39](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/39) - XPathHelper : Remove 'xml version' from XML document [bug] contributed by [StefH](https://github.com/StefH)

# 2.2.0 (12 June 2021)
- [#38](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/38) - Humanizer [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.1.9 (01 June 2021)
- [#37](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/37) - Fix StackOverflow problem when providing invalid arguments to Math methods [bug] contributed by [StefH](https://github.com/StefH)
- [#36](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/issues/36) - Stack Overflow on Math.Subtract and empty string [bug]

# 2.1.8 (06 May 2021)
- [#33](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/33) - CI Build : Don't run SonarCloud for PR [enhancement] contributed by [StefH](https://github.com/StefH)
- [#35](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/35) - Fix Math.Avg, Math.Divide and Math.Multiply [bug] contributed by [StefH](https://github.com/StefH)
- [#34](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/issues/34) - Math methods with type double [bug]

# 2.1.7 (25 April 2021)
- [#32](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/32) - Fix the String.Join helper for IEnumerable contributed by [0xced](https://github.com/0xced)

# 2.1.6 (23 April 2021)
- [#31](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/31) - Fix `String.Format` to handle formatting of multiple types correctly, not only DateTime [enhancement] contributed by [DamienBraillard](https://github.com/DamienBraillard)

# 2.1.5 (20 April 2021)
- [#30](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/30) - Add compare operators to Math [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.1.4 (27 March 2021)
- [#24](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/24) - Add String PadLeft and PadRight  [enhancement] contributed by [mrbelk](https://github.com/mrbelk)

# 2.1.3 (25 March 2021)
- [#26](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/26) - Add extra unit tests for Regex [enhancement] contributed by [StefH](https://github.com/StefH)
- [#28](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/28) - Remove default value from Regex helper [bug] contributed by [StefH](https://github.com/StefH)
- [#27](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/issues/27) - Regex.IsMatch broken with ignore case [bug]

# 2.1.2 (18 March 2021)
- [#22](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/22) - Do not use StringValueParser when parsing arguments [bug] contributed by [StefH](https://github.com/StefH)
- [#21](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/issues/21) - System.ArgumentException: 'Object of type 'System.Int32' cannot be converted to type 'System.String'.' [bug]

# 2.1.1 (09 February 2021)
- [#20](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/20) - Add  CustomHelperPaths to options [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.1.0 (06 February 2021)
- [#18](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/18) - Add XPath, DynamicLinq, Json, Xeger and Random [enhancement] contributed by [StefH](https://github.com/StefH)
- [#19](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/19) - Use SimpleJson and update packages [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.0.2 (27 January 2021)
- [#17](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/17) - Add DateTimeHelpers [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.0.1 (14 December 2020)
- [#16](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/16) - Remove dependency on Newtonsoft.Json [enhancement] contributed by [StefH](https://github.com/StefH)

# 2.0.0 (13 December 2020)
- [#13](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/13) - Enumerable test : IsEmpty contributed by [StefH](https://github.com/StefH)
- [#15](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/15) - Use HandleBars.Net version 2 contributed by [StefH](https://github.com/StefH)
- [#7](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/issues/7) - Compatibility with Handlebarsjs (and Handlerbars.CSharp)? [bug]

# 1.1.1 (24 October 2020)
- [#12](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/12) - Enumerable.Select [enhancement] contributed by [StefH](https://github.com/StefH)
- [#9](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/issues/9) - Enumerable.Select [enhancement]

# 1.1.0 (16 October 2020)
- [#11](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/11) - Sign assemblies [enhancement] contributed by [StefH](https://github.com/StefH)
- [#10](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/issues/10) - The assembly is not signed [enhancement]

# 1.0.2 (24 June 2020)
- [#8](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/8) - Handlebars Net v.s. CSharp compatiblity fixes [enhancement] contributed by [StefH](https://github.com/StefH)

# 1.0.1 (12 June 2020)
- [#6](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/6) - MoreHelpers [enhancement] contributed by [StefH](https://github.com/StefH)

# 1.0.0 (22 April 2020)
- [#3](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/3) - Set up CI with Azure Pipelines [enhancement] contributed by [StefH](https://github.com/StefH)
- [#4](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/4) - Add prefix to options [enhancement] contributed by [StefH](https://github.com/StefH)
- [#5](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/5) - Add Contains method to StringHelper [enhancement] contributed by [StefH](https://github.com/StefH)

# 0.0.3 (18 April 2020)
- [#2](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/2) - Lower dependency from Handlebars.Net to 1.9.5 [enhancement] contributed by [StefH](https://github.com/StefH)

# 0.0.2 (18 April 2020)
- [#1](https://github.com/Handlebars-Net/Handlebars.Net.Helpers/pull/1) - Implement helpers for String, Math and Regex [enhancement] contributed by [StefH](https://github.com/StefH)

