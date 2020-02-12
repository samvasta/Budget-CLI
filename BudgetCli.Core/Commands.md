# Commands

## General

* `h` or `help`
* `v` or `version`

## Parameter Types & Expressions

### String

Quotation marks are optional except for strings which contain spaces.

### Integer

* Accepts decimals, but rounds to nearest whole number
* Ignores currency symbols
* Accepts parenthesis for negative numbers

Examples:

    1234            =>       1234
    1234.56          =>       1235
    1234.32         =>       1234

    $1234.56        =>       1235
    
    (1234)          =>       1234
    (1234)          =>      -1234
    ($1234)         =>      -1234
    
    -1234.56        =>      -1235
    -$1234.56       =>      -1235

#### Integer Expressions

Examples:

    12+                      12 and higher (inclusive)
    12-                      12 and lower (inclusive)
    12 : 56                  between 12 and 56 (inclusive)

### Decimal

* Rounds to 2 decimal places.
* Ignores currency symbols
* Accepts parenthesis for negative numbers

Examples:

    1234            =>       1234.00
    1234.5          =>       1234.50
    1234.56         =>       1234.56
    1234.567        =>       1234.57

    $1234.56        =>       1234.56
    
    (1234.56)       =>      -1234.56
    ($1234.56)      =>      -1234.56
    
    -1234.56        =>      -1234.56
    -$1234.56       =>      -1234.56

#### Decimal Expressions

Examples:

    12.34+                      12.34 and higher
    12.34-                      12.34 and lower
    12.34 : 56.78               between 12.34 and 56.78, inclusive

### Date

Examples:

    dd/mm/yyyy
    dd.mm.yyyy
    dd/mm/yy
    dd.mm.yy
    dd/mm               [Year assumed to be present year]
    dd.mm
    last ddd            [ddd = day name string]
    last mmm            [mmm = month name string]
    [INT] days ago
    [INT] weeks ago
    [INT] months ago

#### Date Expressions

    [DATE]              'to' is unbounded
    : [DATE]           'from' is unbounded
    [DATE] : [DATE]    Bounded on both sides

## Accounts

* `cat [a, account] [STRING]` list details of an account
  * `-d --date` list details of account on a specific date
* `ls [a, account, accounts]` list accounts as tree
  * `-c --category [STRING]` filter by category name
  * `-d --description` filter by description
  * `-f --funds [DECIMAL EXPR]` filter by available funds
  * `-n --name [STRING]`
  * `-p --priority [INTEGER EXPR]` filter by priority
  * `-t --type`
    * `source` or `sink`
* `mv [STRING] [STRING]` Move account to be a child of another account
* `new [a, account] [STRING]` create a new account with given name
  * `-c --category [STRING]`
  * `-d --description`
  * `-f --funds [DECIMAL]`
  * `-p --priority [INTEGER]`
  * `-t --type`
    * `source` or `sink`
* `rm [STRING]` Remove an account
  * `-r --recursive` to remove child accounts
* `set [a, account] [STRING]` Sets values of the account
  * `-c --category [STRING]`
  * `-d --description`
  * `-n --name [STRING]`
  * `-p --priority [INTEGER]`
  * `-t --type`
    * `source` or `sink`

## Transactions

* `cat [t, trans, transaction] [INTEGER]` display details of a transaction
* `ls [t, trans, transactions]` list transactions
  * `-a --account [STRING]` Filter by account
  * `-d --date [DATE EXPR]` Filter by date
  * `-f --funds [DECIMAL EXPR]` Filter by amount
  * `-i --id [INTEGER EXPR]` Filter by id
  * `-t --type` Filter by type (`inflow` or `outflow` or `internal`)
* `new [t, trans, transaction]` transaction
  * `-f --funds  [DECIMAL]` amount to move
  * `-d --dest [STRING]` account to put funds into
    * Omit for outflow
  * `-s --source [STRING]` account to take funds from
    * Omit for inflow
* `rm [t, trans, transaction] [INTEGER]` remove transaction

## Reporting

* `ls [h, history]` list history of activity in reverse chronological order
  * `-d --date [DATE EXPR]`
  * `-c --count [INTEGER]` max count of results to return (default 10)
