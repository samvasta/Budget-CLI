
# Tests And Code Coverage

This solution uses [XUnit](https://xunit.net/) as the testing framework, [Coverlet](https://github.com/tonerdo/coverlet) for coverage analysis, and [ReportGenerator](https://github.com/danielpalme/ReportGenerator) to generate coverage reports.

Projects should have a corresponding tests project, where applicable. Test projects should have the naming convention `BudgetCli.<NAME>.Tests`.

## Tests

Tests can be run in several ways:

- `dotnet test`
- The [.NET Core Test Explorer](https://marketplace.visualstudio.com/items?itemName=formulahendry.dotnet-test-explorer) Visual Studio Code plugin
- The `test` or `test-cover` Visual Studio Code task

## Code Coverage

Code coverage data can be generated with the [.NET Core Test Explorer](https://marketplace.visualstudio.com/items?itemName=formulahendry.dotnet-test-explorer) Visual Studio Code plugin or the `test-cover` task (see `tasks.json`).

### Coverage Report Generator

To generate human readable coverage reports:

1. Install report generator as a global tool in VS Code with

    `dotnet tool install --global dotnet-reportgenerator-globaltool`

2. From solution root run:

    `reportgenerator "-reports:BudgetCli.*.Tests/lcov.info" "-targetdir:coveragereport" -reporttypes:Html "-filefilters:-*\Budget-CLI\BudgetCli.*.Tests\*;-*Generated*;-*.nuget*"`
