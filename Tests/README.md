# Test Coverage

This solution uses **Coverlet** for code coverage and generates reports via the .NET CLI.

Visual Studio Test Explorer is not used for coverage generation in this setup due to inconsistencies in coverage collection behavior. Instead, coverage is generated via the command line for consistent results across local development and CI.

---

## Prerequisites

Ensure the following are installed:

* .NET SDK 6+ / 7+ / 8+ (depending on the solution)
* ReportGenerator global tool

### Install ReportGenerator

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

Or update it:

```bash
dotnet tool update -g dotnet-reportgenerator-globaltool
```

---

## Running Tests with Coverage

From the **solution root directory**, run:

```bash
dotnet test --settings ./coverlet.runsettings
```

This will:

* Execute all tests in the solution
* Collect code coverage using Coverlet
* Generate `coverage.cobertura.xml` output files (typically under `TestResults/` or project output folders)

---

## Generating an HTML Coverage Report

After tests complete, generate a human-readable HTML report:

```bash
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
```

This will:

* Aggregate all `coverage.cobertura.xml` files in the solution
* Generate an HTML report in the `coveragereport/` directory

---

## Viewing the Report

Open the generated report in a browser:

```
coveragereport/index.html
```

---

## Notes

* Coverage is only generated when running tests via `dotnet test` with the provided runsettings file.
* Running tests through Visual Studio Test Explorer will execute tests but will **not generate coverage reports** in this setup.
* Always run commands from the **solution root** to ensure correct relative paths.

---

## Recommended Workflow

```bash
# 1. Run tests + collect coverage
dotnet test --settings ./coverlet.runsettings

# 2. Generate HTML report
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

# 3. Open report (Windows)
start coveragereport/index.html
```
