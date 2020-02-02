BEGIN TRANSACTION

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS [user];
CREATE TABLE [user] (
  [id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [email] NVARCHAR(100) NOT NULL,
  [language] NVARCHAR(5) NOT NULL DEFAULT 'en-US',
  [theme] NVARCHAR(5) NOT NULL DEFAULT 'light',
  [passwordHash] varbinary(64) NOT NULL,
  [passwordSalt] varbinary(128) NOT NULL,
  [createdTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  [dateFormat] NVARCHAR(20) NOT NULL DEFAULT 'YYYY-MM-DD',
  [timeFormat] NVARCHAR(20) NOT NULL DEFAULT 'h:mm a',
  [lastUpdatedTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  PRIMARY KEY ([id]),
  CONSTRAINT [user_email_UNIQUE] UNIQUE  ([email]),
  CONSTRAINT [user_id_UNIQUE] UNIQUE  ([id])
) ;

--
-- Table structure for table `congregation`
--

DROP TABLE IF EXISTS [congregation];
CREATE TABLE congregation (
  [id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [name] NVARCHAR(40) NOT NULL,
  [number] NVARCHAR(10) DEFAULT NULL,
  [createdTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  [lastUpdatedTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  PRIMARY KEY ([id]),
  CONSTRAINT [congregation_id_UNIQUE] UNIQUE  ([id]),
  CONSTRAINT [congregation_name_UNIQUE] UNIQUE  ([name]),
  CONSTRAINT [congregation_number_UNIQUE] UNIQUE  ([number])
) ;

--
-- Table structure for table `person`
--

DROP TABLE IF EXISTS [person];
CREATE TABLE person (
  [id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [userId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [firstname] NVARCHAR(40) NOT NULL,
  [lastname] NVARCHAR(40) NOT NULL,
  [gender] NVARCHAR(6) NOT NULL DEFAULT 'male',
  [congregationId] UNIQUEIDENTIFIER DEFAULT NULL,
  [createdTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  [lastUpdatedTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  [email] NVARCHAR(100),
  [phone] NVARCHAR(40),
  [languages] NVARCHAR(100),
  [notes] NVARCHAR(1000),
  [assignment] NVARCHAR(15) NOT NULL DEFAULT 'publisher',
  [privilege] NVARCHAR(12) NOT NULL DEFAULT 'publisher',
  [language] NVARCHAR(5) NOT NULL DEFAULT 'en-US',
  PRIMARY KEY ([id]),
  CONSTRAINT [person_id_UNIQUE] UNIQUE  ([id])
 ,
  CONSTRAINT [fk_person_congregation] FOREIGN KEY ([congregationId]) REFERENCES congregation ([id]) ON DELETE CASCADE,
  CONSTRAINT [fk_person_user] FOREIGN KEY ([userId]) REFERENCES [user] ([id]) ON DELETE CASCADE ON UPDATE NO ACTION
) ;

--
-- Table structure for table `project`
--

DROP TABLE IF EXISTS [project];
CREATE TABLE project (
  [id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [name] NVARCHAR(40) NOT NULL,
  [email] NVARCHAR(100) NOT NULL,
  [createdTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  [lastUpdatedTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  [allowRequests] BIT NOT NULL DEFAULT 1,
  PRIMARY KEY ([id]),
  CONSTRAINT [project_id_UNIQUE] UNIQUE  ([id]),
  CONSTRAINT [name_UNIQUE] UNIQUE  ([name])
) ;

--
-- Table structure for table `category`
--

DROP TABLE IF EXISTS [category];
CREATE TABLE category (
  [id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [projectId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [name] NVARCHAR(40) NOT NULL,
  [createdTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  [lastUpdatedTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  PRIMARY KEY ([id]),
  CONSTRAINT [category_id_UNIQUE] UNIQUE  ([id])
 ,
  CONSTRAINT [fk_category_project] FOREIGN KEY ([projectId]) REFERENCES project ([id]) ON DELETE CASCADE ON UPDATE NO ACTION
) ;

--
-- Table structure for table `shift`
--

DROP TABLE IF EXISTS [shift];
CREATE TABLE shift (
  [id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [categoryId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [createdTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  [lastUpdatedTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  [date] int NOT NULL,
  [time] int NOT NULL,
  [duration] int NOT NULL,
  [status] NVARCHAR(9) NOT NULL DEFAULT 'draft',
  [mode] NVARCHAR(12) NOT NULL DEFAULT 'open',
  [calledOffReason] NVARCHAR(200),
  PRIMARY KEY ([id]),
  CONSTRAINT [shift_id_UNIQUE] UNIQUE  ([id])
 ,
  CONSTRAINT [fk_shift_category] FOREIGN KEY ([categoryId]) REFERENCES category ([id]) ON DELETE CASCADE ON UPDATE NO ACTION
) ;

--
-- Table structure for table `team`
--

DROP TABLE IF EXISTS [team];
CREATE TABLE team (
  [id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [projectId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [name] NVARCHAR(40) NOT NULL,
  [description] NVARCHAR(1000),
  [helpLink] NVARCHAR(1000),
  [createdTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  [lastUpdatedTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  PRIMARY KEY ([id]),
  CONSTRAINT [team_id_UNIQUE] UNIQUE  ([id])
 ,
  CONSTRAINT [fk_task_project] FOREIGN KEY ([projectId]) REFERENCES project ([id]) ON DELETE CASCADE ON UPDATE NO ACTION
) ;

--
-- Table structure for table `application`
--

DROP TABLE IF EXISTS [application];
CREATE TABLE application (
  [id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [shiftId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [personId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [availableAfter] BIT NOT NULL DEFAULT 1,
  [notes] NVARCHAR(200),
  [createdTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  [lastUpdatedTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  PRIMARY KEY ([id]),
  CONSTRAINT [application_applicant_UNIQUE] UNIQUE  ([shiftId],[personId])
 ,
  CONSTRAINT [fk_application_person] FOREIGN KEY ([personId]) REFERENCES person ([id]) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT [fk_application_shift] FOREIGN KEY ([shiftId]) REFERENCES shift ([id]) ON DELETE CASCADE ON UPDATE NO ACTION
) ;

--
-- Table structure for table `topic`
--

DROP TABLE IF EXISTS [topic];
CREATE TABLE topic (
  [id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [projectId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [name] NVARCHAR(40) NOT NULL,
  [createdTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  [lastUpdatedTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  PRIMARY KEY ([id]),
  CONSTRAINT [topic_id_UNIQUE] UNIQUE  ([id])
 ,
  CONSTRAINT [fk_topic_project] FOREIGN KEY ([projectId]) REFERENCES project ([id]) ON DELETE CASCADE ON UPDATE NO ACTION
) ;

--
-- Table structure for table `article`
--

DROP TABLE IF EXISTS [article];
CREATE TABLE article (
  [id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [topicId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [title] NVARCHAR(100) NOT NULL,
  [content] NVARCHAR(MAX) NOT NULL,
  [createdTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  [lastUpdatedTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  PRIMARY KEY ([id]),
  CONSTRAINT [article_id_UNIQUE] UNIQUE  ([id])
 ,
  CONSTRAINT [fk_article_topic] FOREIGN KEY ([topicId]) REFERENCES topic ([id])
) ;

--
-- Table structure for table `attendee`
--

DROP TABLE IF EXISTS [attendee];
CREATE TABLE attendee (
  [id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [shiftId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [personId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [teamId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [applicationId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [isCaptain] BIT NOT NULL DEFAULT 0,
  [createdTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  [lastUpdatedTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  PRIMARY KEY ([id]),
  CONSTRAINT [attendee_attendee_UNIQUE] UNIQUE  ([shiftId],[personId],[teamId]),
  CONSTRAINT [attendee_application_UNIQUE] UNIQUE  ([applicationId])
 ,
  CONSTRAINT [fk_attendee_application] FOREIGN KEY ([applicationId]) REFERENCES application ([id]) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT [fk_attendee_person] FOREIGN KEY ([personId]) REFERENCES person ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
  -- TODO: CONSTRAINT [fk_attendee_person] FOREIGN KEY ([personId]) REFERENCES person ([id]) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT [fk_attendee_shift] FOREIGN KEY ([shiftId]) REFERENCES shift ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
  -- TODO: CONSTRAINT [fk_attendee_shift] FOREIGN KEY ([shiftId]) REFERENCES shift ([id]) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT [fk_attendee_team] FOREIGN KEY ([teamId]) REFERENCES team ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION
  -- TODO: CONSTRAINT [fk_attendee_team] FOREIGN KEY ([teamId]) REFERENCES team ([id]) ON DELETE CASCADE ON UPDATE NO ACTION
) ;

--
-- Table structure for table `role`
--

DROP TABLE IF EXISTS [role];
CREATE TABLE role (
  [id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [projectId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [name] NVARCHAR(40) NOT NULL,
  [knowledgeBaseRead] BIT NOT NULL DEFAULT 1,
  [knowledgeBaseWrite] BIT NOT NULL DEFAULT 1,
  [participantsRead] BIT NOT NULL DEFAULT 1,
  [participantsWrite] BIT NOT NULL DEFAULT 1,
  [settingsRead] BIT NOT NULL DEFAULT 1,
  [settingsWrite] BIT NOT NULL DEFAULT 1,
  [createdTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  [rolesRead] BIT NOT NULL DEFAULT 1,
  [rolesWrite] BIT NOT NULL DEFAULT 1,
  [editable] BIT NOT NULL DEFAULT 1,
  [lastUpdatedTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  [calendarRead] BIT NOT NULL DEFAULT 1,
  [calendarWrite] BIT NOT NULL DEFAULT 1,
  PRIMARY KEY ([id]),
  CONSTRAINT [role_id_UNIQUE] UNIQUE  ([id])
 ,
  CONSTRAINT [fk_role_project] FOREIGN KEY ([projectId]) REFERENCES project ([id])
) ;

--
-- Table structure for table `eligibility`
--

DROP TABLE IF EXISTS [eligibility];
CREATE TABLE eligibility (
  [id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [roleId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [categoryId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [shiftsRead] BIT NOT NULL DEFAULT 1,
  [shiftsWrite] BIT NOT NULL DEFAULT 1,
  [isTeamCaptain] BIT NOT NULL DEFAULT 1,
  [isSubstituteCaptain] BIT NOT NULL DEFAULT 1,
  [createdTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  [lastUpdatedTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  PRIMARY KEY ([id]),
  CONSTRAINT [eligibility_eligibility_UNIQUE] UNIQUE  ([roleId],[categoryId])
 ,
  CONSTRAINT [fk_eligibility_category] FOREIGN KEY ([categoryId]) REFERENCES category ([id]) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT [fk_eligibility_role] FOREIGN KEY ([roleId]) REFERENCES role ([id]) ON DELETE CASCADE ON UPDATE NO ACTION
) ;

--
-- Table structure for table `participation`
--

DROP TABLE IF EXISTS [participation];
CREATE TABLE participation (
  [id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [personId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [projectId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  [roleId] UNIQUEIDENTIFIER DEFAULT NULL,
  [status] NVARCHAR(9) NOT NULL DEFAULT 'requested',
  [createdTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  [lastUpdatedTime] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
  PRIMARY KEY ([id]),
  CONSTRAINT [participation_id_UNIQUE] UNIQUE  ([id]),
  CONSTRAINT [participation_participant_UNIQUE] UNIQUE  ([personId],[projectId])
 ,
  CONSTRAINT [fk_participation_person] FOREIGN KEY ([personId]) REFERENCES person ([id]) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT [fk_participation_project] FOREIGN KEY ([projectId]) REFERENCES project ([id]) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT [fk_participation_role] FOREIGN KEY ([roleId]) REFERENCES role ([id])
) ;

COMMIT TRANSACTION
