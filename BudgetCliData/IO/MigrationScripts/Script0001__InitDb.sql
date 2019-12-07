CREATE TABLE Account (
    Id              INTEGER NOT NULL PRIMARY KEY,
    'Name'          TEXT NOT NULL,
    CategoryId      INTEGER REFERENCES Account(Id),
    InitialFunds    INTEGER NOT NULL DEFAULT 0,
    Priority        INTEGER NOT NULL DEFAULT 100,
    AccountKind     INTEGER NOT NULL DEFAULT 0,
    'Description'   TEXT
);

CREATE TABLE 'Transaction' (
    Id                      INTEGER NOT NULL PRIMARY KEY,
    'Timestamp'             TIMESTAMP NOT NULL,
    SourceAccountId         INTEGER REFERENCES Account(Id),
    DestinationAccountId    INTEGER REFERENCES Account(Id),
    TransferAmount          INTEGER NOT NULL DEFAULT 0,
    Memo                    TEXT
);

CREATE TABLE CommandAction (
    Id                  INTEGER NOT NULL PRIMARY KEY,
    CommandActionKind   INTEGER NOT NULL DEFAULT 0,
    CommandText         TEXT,
    'Timestamp'         TIMESTAMP NOT NULL,
    IsExecuted          BOOLEAN NOT NULL DEFAULT 0
);

CREATE TABLE CommandActionParameter (
    Id              INTEGER NOT NULL PRIMARY KEY,
    CommandActionId INTEGER NOT NULL REFERENCES CommandAction(Id),
    'Data'          TEXT
);