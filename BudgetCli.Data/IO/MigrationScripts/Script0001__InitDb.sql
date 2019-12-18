CREATE TABLE Account (
    Id              INTEGER NOT NULL PRIMARY KEY,
    'Name'          TEXT NOT NULL,
    CategoryId      INTEGER REFERENCES Account(Id),
    Priority        INTEGER NOT NULL DEFAULT 100,
    AccountKind     INTEGER NOT NULL DEFAULT 0,
    'Description'   TEXT
);

CREATE TABLE AccountState (
    Id              INTEGER NOT NULL PRIMARY KEY,
    'Timestamp'     TIMESTAMP NOT NULL,
    AccountId       INTEGER NOT NULL REFERENCES Account(Id),
    Funds           INTEGER NOT NULL DEFAULT 0,
    IsClosed        BOOLEAN NOT NULL DEFAULT 0
);

CREATE TABLE 'Transaction' (
    Id                      INTEGER NOT NULL PRIMARY KEY,
    'Timestamp'             TIMESTAMP NOT NULL,
    SourceAccountId         INTEGER REFERENCES Account(Id),
    DestinationAccountId    INTEGER REFERENCES Account(Id),
    TransferAmount          INTEGER NOT NULL DEFAULT 0,
    Memo                    TEXT
);