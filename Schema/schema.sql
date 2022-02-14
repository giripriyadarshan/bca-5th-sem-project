CREATE TABLE [dbo].[admin] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [username] NVARCHAR (100) NOT NULL,
    [password] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_admin] PRIMARY KEY NONCLUSTERED ([ID] ASC)
);

CREATE TABLE [dbo].[semester] (
    [sem] INT NOT NULL,
    CONSTRAINT [semester_pk] PRIMARY KEY NONCLUSTERED ([sem] ASC)
);

CREATE TABLE [dbo].[subject] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [subject_name] VARCHAR (100) NOT NULL,
    [semesterID]   INT           NOT NULL,
    CONSTRAINT [subject_pk] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [subject_semester_sem_fk] FOREIGN KEY ([semesterID]) REFERENCES [dbo].[semester] ([sem])
);

CREATE TABLE [dbo].[year] (
    [ID]          INT IDENTITY (1, 1) NOT NULL,
    [year_number] INT NOT NULL,
    [subjectID]   INT NOT NULL,
    CONSTRAINT [year_pk] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [year_subject_ID_fk] FOREIGN KEY ([subjectID]) REFERENCES [dbo].[subject] ([ID])
);

CREATE TABLE [dbo].[files] (
    [ID]                 INT           IDENTITY (1, 1) NOT NULL,
    [file_name]          VARCHAR (MAX) NOT NULL,
    [no_of_times_opened] INT           NOT NULL,
    [yearID]             INT           NOT NULL,
    CONSTRAINT [files_pk] PRIMARY KEY NONCLUSTERED ([ID] ASC),
    CONSTRAINT [files_year_ID_fk] FOREIGN KEY ([yearID]) REFERENCES [dbo].[year] ([ID])
);

