# Budget-CLI

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/9f271024f55843e98ab704ad51160d6f)](https://app.codacy.com/manual/samuel.m.vasta/Budget-CLI?utm_source=github.com&utm_medium=referral&utm_content=samvasta/Budget-CLI&utm_campaign=Badge_Grade_Settings) [![Build Status](https://travis-ci.org/samvasta/Budget-CLI.svg?branch=master)](https://travis-ci.org/samvasta/Budget-CLI)

A budgeting tool with a command line interface. Initially started to learn .NET Core, TDD, and other random technologies.

> "It's like [YNAB](https://www.youneedabudget.com/) but with fewer features and no GUI."

Alternate tag line:

> "Because a subscription for a budgeting tool shouldn't be part of your budget (___\*cough\* [ynab](https://www.youneedabudget.com/) \*cough\* _<sup><sub><sub>$12/mo! seriously?</sup></sub></sub>_ \*cough\*___)!"

 _<sup><sup>\*This project is obviously not opinionated at all</sup></sup>_

## Usage

`dotnet run -p BudgetCli.ConsoleApp <path-to-file>` or any of the several Visual Studio Code tasks.

The `<path-to-file>` is a path to a budget file (SQLite), typically with extension `.budget`. The file will be created if it does not exist.
