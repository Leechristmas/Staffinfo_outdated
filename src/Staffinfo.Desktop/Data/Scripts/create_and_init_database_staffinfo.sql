USE STAFFINFO_TESTS
GO
-------------------------------------------------------------------------
---CREATE THE NEW TABLES
-------------------------------------------------------------------------
--ПОЛЬЗОВАТЕЛИ
CREATE TABLE USERS(
	ID INT PRIMARY KEY IDENTITY,
	USER_LOGIN VARCHAR(20) NOT NULL,
	USER_PASSWORD VARCHAR(20) NOT NULL,
	ACCESS_LEVEL INT NOT NULL,
	LAST_NAME VARCHAR(50) NOT NULL,
	FIRST_NAME VARCHAR(50),
	MIDDLE_NAME VARCHAR(50));
GO
--ДОЛЖНОСТИ
CREATE TABLE POST(
		ID INT PRIMARY KEY IDENTITY,
		POST_TITLE VARCHAR(64) NOT NULL,
		SERVICE_ID INT NOT NULL);
GO
--ЗВАНИЯ
CREATE TABLE RANK(
		ID INT PRIMARY KEY IDENTITY,
		RANK_TITLE VARCHAR(64) NOT NULL UNIQUE);
GO
--КЛАССНОСТЬ 
CREATE TABLE CLASINESS (
		ID INT PRIMARY KEY IDENTITY,
		EMPLOYEE_ID	INT NOT NULL,
		ORDER_NUMBER INT NOT NULL,
		CLASINESS_DATE DATE NOT NULL,
		CLASINESS_LEVEL INT NOT NULL,
		DESCRIPTION	VARCHAR(64));
GO
--КОНТРАКТЫ
CREATE TABLE CONTRACT (
		ID	INT PRIMARY KEY IDENTITY,
		EMPLOYEE_ID	INT NOT NULL,
		START_DATE	DATE NOT NULL,
		FINISH_DATE	DATE NOT NULL,
		DESCRIPTION	VARCHAR(64));	
GO
--УЧЕБНЫЕ ЗАВЕДЕНИЯ
CREATE TABLE EDUCATIONAL_INSTITUTION(
		ID INT PRIMARY KEY IDENTITY,
		INST_TITLE VARCHAR(64) NOT NULL UNIQUE, 
		DESCRIPTION VARCHAR(64), 
		INST_TYPE VARCHAR(64) NOT NULL);	
GO
--ПРОЦЕСС ОБУЧЕНИЯ В УЧРЕЖДЕНИИ ОБРАЗОВАНИЯ
CREATE TABLE EDUCATION_TIME (
		ID INT PRIMARY KEY IDENTITY,
		EMPLOYEE_ID	INT NOT NULL,
		START_DATE DATE NOT NULL,
		FINISH_DATE DATE NOT NULL,
		SPECIALITY_ID INT NOT NULL,
		INSTITUTION_ID INT NOT NULL,
		DESCRIPTION VARCHAR(64));
GO
--БЛАГОДАРНОСТИ
CREATE TABLE GRATITUDE (
		ID	INT PRIMARY KEY IDENTITY,
		EMPLOYEE_ID	INT NOT NULL,
		DESCRIPTION	VARCHAR(64),
		GRATITUDE_DATE DATE NOT NULL);
GO
--ОТПУСКА	
CREATE TABLE HOLIDAY_TIME (
		ID	INT PRIMARY KEY IDENTITY,
		EMPLOYEE_ID	INT NOT NULL,
		DESCRIPTION	VARCHAR(64),
		START_HOLIDAY_DATE DATE NOT NULL,
		FINISH_HOLIDAY_DATE DATE NOT NULL);
GO
--БОЛЬНИЧНЫЕ
CREATE TABLE HOSPITAL_TIME(
		ID INT PRIMARY KEY IDENTITY, 
		DISEASED_ID INT NOT NULL, 
		DESCRIPTION VARCHAR(64), 
		START_HOSPITAL_TIME DATE NOT NULL, 
		FINISH_HOSPITAL_TIME DATE NOT NULL);
GO
--ПРОХОЖДЕНИЕ ВОИНСКОЙ СЛУЖБЫ
CREATE TABLE MILITARY_PROCESS (
	ID INT PRIMARY KEY IDENTITY,
	EMPLOYEE_ID INT NOT NULL,
	DESCRIPTION VARCHAR(64),
	START_DATE DATE NOT NULL,
	FINISH_DATE DATE NOT NULL,
	MILITARY_UNIT_ID INT NOT NULL);
GO
--ПРИСВОЕНИЕ ДОЛЖНОСТИ
CREATE TABLE POST_ASSIGNMENT(
	ID INT PRIMARY KEY IDENTITY,
	EMPLOYEE_ID INT NOT NULL,
	DESCRIPTION	VARCHAR(240),
	ASSIGNMENT_DATE	DATE NOT NULL,
	PREV_POST_ID INT,
	NEW_POST_ID	INT NOT NULL,
	ORDER_NUMBER INT NOT NULL);
GO
--ПРИСВОЕНИЕ ЗВАНИЙ
CREATE TABLE RANK_ASSIGNMENT(
	ID INT PRIMARY KEY IDENTITY,
	EMPLOYEE_ID	INT NOT NULL,
	DESCRIPTION	VARCHAR(64),
	ASSIGNMENT_DATE DATE NOT NULL,
	PREV_RANK_ID INT,
	NEW_RANK_ID	INT NOT NULL,
	ORDER_NUMBER INT NOT NULL);
GO
--ТИП РОДСТВА
CREATE TABLE RELATIVE_TYPE(
		ID INT PRIMARY KEY IDENTITY,
		RELATIVE_TYPE VARCHAR(64) UNIQUE);
GO
--РОДСТВЕННИКИ СЛУЖАЩИХ
CREATE TABLE RELATIVE(
	ID INT PRIMARY KEY IDENTITY,
	EMPLOYEE_ID INT NOT NULL,
	RELATION_TYPE VARCHAR(30),
	FIRST_NAME VARCHAR(64) NOT NULL,
	MIDDLE_NAME VARCHAR(64) NOT NULL,
	LAST_NAME VARCHAR(64) NOT NULL,
	BORN_DATE DATE NOT NULL,
	DESCRIPTION VARCHAR(240));
GO
--ВЫГОВОРЫ
CREATE TABLE REPRIMAND(
	ID INT PRIMARY KEY IDENTITY,
	EMPLOYEE_ID INT NOT NULL,
	SUM_OF_REPRIMAND FLOAT NOT NULL,
	REPRIMAND_DATE DATE NOT NULL,
	DESCRIPTION VARCHAR(240));
GO
--АТТЕСТАЦИЯ СЛУЖАЩЕГО
CREATE TABLE SERTIFICATION(
	ID INT PRIMARY KEY IDENTITY,
	EMPLOYEE_ID INT NOT NULL,
	SERTIFICATION_DATE DATE NOT NULL,
	DESCRIPTION VARCHAR(64));
GO
--СЛУЖБЫ
CREATE TABLE SERVICE(
	ID INT PRIMARY KEY IDENTITY, 
	SERVICE_TITLE VARCHAR(120) NOT NULL UNIQUE);
GO
--СПЕЦИАЛЬНОСТИ(ОБРАЗОВАНИЕ)
CREATE TABLE SPECIALITY(
	ID INT PRIMARY KEY IDENTITY,
	SPECIALITY VARCHAR(64) NOT NULL, 
	DESCRIPTION VARCHAR(120));
GO
--СЛУЖАЩИЙ
CREATE TABLE EMPLOYEE(
	ID INT PRIMARY KEY IDENTITY,
	EMPLOYEE_FIRSTNAME VARCHAR(64) NOT NULL,
	EMPLOYEE_MIDDLENAME	VARCHAR(64) NOT NULL,
	EMPLOYEE_LASTNAME VARCHAR(64) NOT NULL,
	PERSONAL_KEY VARCHAR(16) NOT NULL,
	POST_ID	INT NOT NULL,
	RANK_ID	INT NOT NULL,
	BORN_DATE DATE NOT NULL,
	JOB_START_DATE DATE NOT NULL,
	ADDRESS	VARCHAR(120),
	PASPORT_ID	INT NOT NULL,
	MOBILE_PHONE_NUMBER VARCHAR(13),
	HOME_PHONE_NUMBER VARCHAR(10),
	IS_PENSIONER BIT DEFAULT 0,
	PHOTO IMAGE);
GO
--НАРУШЕНИЯ
CREATE TABLE VIOLATION(
	ID INT PRIMARY KEY IDENTITY, 
	VIOLATOR_ID INT NOT NULL, 
	DESCRIPTION VARCHAR(240), 
	VIOLATION_DATE DATE NOT NULL);
GO
--ВОЕННЫЕ ЧАСТИ
CREATE TABLE MILITARY_UNIT(
	ID INT PRIMARY KEY IDENTITY,
	MILITARY_NAME VARCHAR(120),
	DESCRIPTION VARCHAR(240));
GO
--ПАСПОРТНЫЕ ДАННЫЕ
CREATE TABLE PASPORT(
	ID INT PRIMARY KEY IDENTITY,
	--ORGANIZATION_UNIT_ID INT,
	ORGANIZATION_UNIT VARCHAR(120),
	NUMBER VARCHAR(20),
	SERIES VARCHAR(5));
GO
-------------------------------------------------------------------------
---CONSTRAINTS
-------------------------------------------------------------------------
ALTER TABLE PASPORT
	ADD
		CONSTRAINT	UQ_PASPORT_UNIQUE
					UNIQUE (NUMBER, SERIES);
GO
ALTER TABLE CLASINESS
	ADD 
		CONSTRAINT	FK_CLASINESS_EMPLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE ON DELETE CASCADE;
GO
ALTER TABLE CONTRACT
	ADD 
		CONSTRAINT	FK_CONTRACT_EMPLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE ON DELETE CASCADE;
GO
ALTER TABLE EDUCATION_TIME
	ADD 
		CONSTRAINT	FK_EDUCATION_TIME_EMPLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE ON DELETE CASCADE,
		CONSTRAINT	FK_EDUCATION_TIME_EDUCATIONAL_INSTITUTION
					FOREIGN KEY (INSTITUTION_ID) REFERENCES EDUCATIONAL_INSTITUTION ON DELETE CASCADE,
		CONSTRAINT	FK_EDUCATION_TIME_SPECIALITY
					FOREIGN KEY (SPECIALITY_ID) REFERENCES SPECIALITY ON DELETE CASCADE;
GO
ALTER TABLE EMPLOYEE
	ADD
		CONSTRAINT	FK_EMPLOYEE_POST
					FOREIGN KEY	(POST_ID) REFERENCES POST ON DELETE CASCADE,
		CONSTRAINT	FK_EMPLOYEE_RANK
					FOREIGN KEY	(RANK_ID) REFERENCES RANK ON DELETE CASCADE,
		CONSTRAINT	FK_EMPLOYEE_PASPORT
					FOREIGN KEY (PASPORT_ID) REFERENCES PASPORT ON DELETE CASCADE;
GO
ALTER TABLE GRATITUDE
	ADD
		CONSTRAINT	FK_GRATITUDE_EMPLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE ON DELETE CASCADE;
GO
ALTER TABLE HOLIDAY_TIME
	ADD
		CONSTRAINT	FK_HOLIDAY_TIME_EMPLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE ON DELETE CASCADE;
GO
ALTER TABLE HOSPITAL_TIME
	ADD
		CONSTRAINT	FK_HOSPITAL_TIME_EMPLOYEE
					FOREIGN KEY (DISEASED_ID) REFERENCES EMPLOYEE ON DELETE CASCADE;
GO
ALTER TABLE MILITARY_PROCESS
	ADD
		CONSTRAINT	FK_MILITARY_PROCESS_EMPLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE ON DELETE CASCADE,
		CONSTRAINT	FK_MILITARY_PROCESS_MILITARY_UNIT
					FOREIGN KEY (MILITARY_UNIT_ID) REFERENCES MILITARY_UNIT ON DELETE CASCADE;
GO
ALTER TABLE POST
	ADD
		CONSTRAINT	FK_POST_SERVICE
					FOREIGN KEY (SERVICE_ID) REFERENCES SERVICE ON DELETE CASCADE,
		CONSTRAINT	UQ_UNIQUE_CONSTRAINT
					UNIQUE(POST_TITLE,SERVICE_ID);
GO
ALTER TABLE POST_ASSIGNMENT
	ADD
		CONSTRAINT	FK_POST_ASSIGNMENT_TB_EMLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE ON DELETE CASCADE,
		CONSTRAINT	FK_POST_ASSIGNMENT_POST_PREV
					FOREIGN KEY (PREV_POST_ID) REFERENCES POST,
		CONSTRAINT	FK_POST_ASSIGNMENT_POST_NEW
					FOREIGN KEY (NEW_POST_ID) REFERENCES POST;
GO
ALTER TABLE RANK_ASSIGNMENT
	ADD
		CONSTRAINT	FK_RANK_ASSIGNMENT_TB_EMLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE ON DELETE CASCADE,
		CONSTRAINT	FK_RANK_ASSIGNMENT_RANK_PREV
					FOREIGN KEY (PREV_RANK_ID) REFERENCES RANK,
		CONSTRAINT	FK_RANK_ASSIGNMENT_RANK_NEW
					FOREIGN KEY (NEW_RANK_ID) REFERENCES RANK;
GO
ALTER TABLE RELATIVE
	ADD
		CONSTRAINT	FK_RELATIVE_EMPLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE ON DELETE CASCADE,
GO
ALTER TABLE REPRIMAND
	ADD
		CONSTRAINT	FK_REPRIMAND_EMPLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE ON DELETE CASCADE;
GO
ALTER TABLE SERTIFICATION
	ADD
		CONSTRAINT	FK_SERTIFICATION_EMPLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE ON DELETE CASCADE;
GO
ALTER TABLE VIOLATION
	ADD
		CONSTRAINT	FK_VIOLATION_EMPLOYEE
					FOREIGN KEY (VIOLATOR_ID) REFERENCES EMPLOYEE ON DELETE CASCADE;
GO
INSERT INTO USERS VALUES('ADMIN', 'admin', 1, 'Шевчук', 'Дмитрий', 'Павлович'),
						('READER', 'reader', 0, 'Шевчук', 'Дмитрий', 'Павлович');

GO

INSERT INTO SERVICE
	VALUES('Пожарно-спасательная'),
		('Медицинская'),
		('Взрывотезническая'),
		('Водолазная');

GO

INSERT INTO POST
	VALUES('Водитель', 1),
		('Спасатель',1),
		('Начальник службы',1),
		('Водитель', 2),
		('Спасатель',2),
		('Начальник службы',2),
		('Водитель', 3),
		('Спасатель',3),
		('Начальник службы',3),
		('Водитель', 4),
		('Спасатель',4),
		('Начальник службы',4);

GO

INSERT INTO RANK
	VALUES('Сержант'),('Прапорщик'), ('Лейтенант'),
	('Капитан'),('Майор'),('Подполковник');

GO
-------------------------------------------------------------------------
---PROCEDURES
-------------------------------------------------------------------------
--ВОЗВРАЩАЕТ ВСЕХ ПОЛЬЗОВАТЕЛЕЙ
CREATE PROCEDURE GET_ALL_USERS
AS
	SELECT * FROM USERS;
GO
--ВОЗВРАЩАЕТ ПОЛЬЗОВАТЕЛЯ, ЕСЛИ АВТОРИЗАЦИЯ ПРОШЛА УСПЕШНО
--@LOGIN - ЛОГИН
--@PASSWORD - ПАРОЛЬ
CREATE PROCEDURE CHECK_USER 
	@LOGIN VARCHAR(20), 
	@PASSWORD VARCHAR(20)
AS
	SELECT * FROM USERS WHERE USER_LOGIN = @LOGIN AND USER_PASSWORD = @PASSWORD;
GO
--ДОБАВЛЯЕТ СЛУЖАЩЕГО В БД И ВОЗВРАЩАЕТ ЕГО ID
--@FIRSTNAME - ИМЯ; 
--@MIDDLENAME - ОТЧЕСТВО; 
--@LASTNAME - ФАМИЛИЯ
--@PERSONAL_KEY - ЛИЧНЫЙ НОМЕР; 
--@POST_ID - ID ДОЛЖНОСТИ
--@RANK_ID - ID ЗВАНИЯ; 
--@BORN_DATE - ДАТА РОЖДЕНИЯ
--@JOB_START_DATE - ДАТА НАЧАЛА РАБОТЫ; 
--@ADDRESS - АДРЕС
--@PASPORT_ID - ID ПАСПОРТА; @
--MOBILE_PHONE_NUMBER - МОБИЛЬНЫЙ НОМЕР
--@HOME_PHONE_NUMBER - ДОМАШНИЙ НОМЕР; 
--@IS_PENSIONER - ЯВЛЯЕТСЯ ЛИ ПЕНСИОНЕРОМ
--@PHOTO - ФОТО
CREATE PROCEDURE ADD_EMPLOYEE 
				@FIRSTNAME VARCHAR(64), 
				@MIDDLENAME VARCHAR(64), 
				@LASTNAME VARCHAR(64), 
				@PERSONAL_KEY VARCHAR(16),
				@POST_ID INT, 
				@RANK_ID INT, 
				@BORN_DATE DATE, 
				@JOB_START_DATE DATE, 
				@ADDRESS VARCHAR(120),
				@PASPORT_ID INT, 
				@MOBILE_PHONE_NUMBER VARCHAR(13), 
				@HOME_PHONE_NUMBER VARCHAR(10), 
				@IS_PENSIONER BIT, 
				@PHOTO IMAGE
AS
INSERT INTO EMPLOYEE VALUES(@FIRSTNAME, 
							@MIDDLENAME, 
							@LASTNAME, 
							@PERSONAL_KEY,
							@POST_ID, 
							@RANK_ID, 
							@BORN_DATE, 
							@JOB_START_DATE, 
							@ADDRESS,
							@PASPORT_ID, 
							@MOBILE_PHONE_NUMBER, 
							@HOME_PHONE_NUMBER, 
							@IS_PENSIONER, 
							@PHOTO);
SELECT MAX(ID) FROM EMPLOYEE;
GO
--ДОБАВЛЯЕТ КЛАССНОСТЬ И ВОЗВРАЩАЕТ ЕГО ID
--@EMPLOYEE_ID - ID СЛУЖАЩЕГО
--@ORDER_NUMBER - НОМЕР ПРИКАЗА
--@CLASINESS_DATE - ДАТА ПОДТВЕРЖДЕНИЯ КЛАССНОСТИ
--@CLASINESS_LEVEL - УРОВЕНЬ КЛАССНОСТИ
--@DESCRIPTION - ДОП. ИНФОРМАЦИЯ
CREATE PROCEDURE ADD_CLASINESS 
				@EMPLOYEE_ID INT,
				@ORDER_NUMBER INT,
				@CLASINESS_DATE DATE,
				@CLASINESS_LEVEL INT,
				@DESCRIPTION VARCHAR(64)
AS
INSERT INTO CLASINESS VALUES(@EMPLOYEE_ID, 
							 @ORDER_NUMBER, 
							 @CLASINESS_DATE, 
							 @CLASINESS_LEVEL, 
							 @DESCRIPTION);
SELECT MAX(ID) FROM CLASINESS;
GO
--ДОБАВЛЯЕТ КОНТРАКТ И ВОЗВРАЩАЕТ ЕГО ID
--@EMPLOYEE_ID - ID СЛУЖАЩЕГО
--@START_DATE - ДАТА ПОДПИСАНИЯ КОНТРАКТА
--@FINISH_DATE - ДАТА ОКОНЧАНИЯ КОНТРАКТА
--@DESCRIPTION - ДОПОЛНИТЕЛЬНО
CREATE PROCEDURE ADD_CONTRACT
				@EMPLOYEE_ID INT,
				@START_DATE DATE,
				@FINISH_DATE DATE,
				@DESCRIPTION VARCHAR(64)
AS
INSERT INTO CONTRACT VALUES(@EMPLOYEE_ID, 
							@START_DATE, 
							@FINISH_DATE, 
							@DESCRIPTION);
SELECT MAX(ID) FROM CONTRACT;
GO
--ДОБАВЛЯЕТ ПРОЦЕСС ОБУЧЕНИЯ И ВОЗВРАЩАЕТ ЕГО ID
--@EMPLOYEE_ID - ID СЛУЖАЩЕГО
--@START_DATE - ДАТА НАЧАЛА ОБУЧЕНИЯ
--@FINISH_DATE - ДАТА ОКОНЧАНИЯ ОБУЧЕНИЯ
--@SPECIALITY_ID - ID СПЕЦИАЛЬНОСТИ
--@INSTITUTION_ID - ID УЧЕБНОГО ЗАВЕДЕНИЯ
--@DESCRIPTION - ДОП. ИНФОРМАЦИЯ
CREATE PROCEDURE ADD_EDUCATION_TIME
				@EMPLOYEE_ID INT,
				@START_DATE DATE,
				@FINISH_DATE DATE,
				@SPECIALITY_ID INT,
				@INSTITUTION_ID INT,
				@DESCRIPTION VARCHAR(64)
AS
INSERT INTO EDUCATION_TIME VALUES(@EMPLOYEE_ID,
								  @START_DATE,
								  @FINISH_DATE,
								  @SPECIALITY_ID,
								  @INSTITUTION_ID,
								  @DESCRIPTION);
SELECT MAX(ID) FROM EDUCATION_TIME;
GO
--ДОБАВЛЯЕТ УЧЕБНОЕ ЗАВЕДЕНИЕ И ВОЗВРАЩАЕТ ЕГО ID
--@INST_TITLE - НАЗВАНИЕ
--@DESCRIPTION - ДОП. ИНФОРМАЦИЯ
--@INST_TYPE - ТИП УЧЕБНОГО ЗАВЕДЕНИЯ
CREATE PROCEDURE ADD_EDUCATIONAL_INSTITUTION
				@INST_TITLE VARCHAR(64),
				@DESCRIPTION VARCHAR(64),
				@INST_TYPE VARCHAR(64)
AS
INSERT INTO EDUCATIONAL_INSTITUTION VALUES(@INST_TITLE,
										   @DESCRIPTION,
										   @INST_TYPE);
SELECT MAX(ID) FROM EDUCATIONAL_INSTITUTION;
GO
--ДОБАВЛЯЕТ БЛАГОДАРНОСТЬ И ВОЗВРАЩАЕТ ЕЕ ID
--@EMPLOYEE_ID - ID СЛУЖАЩЕГО
--@DESCRIPTION - ДОП. ИНФОРМАЦИЯ
--@GRATITUDE_DATE - ДАТА ВЫНЕСЕНИЯ БЛАГОДАРНОСТИ
CREATE PROCEDURE ADD_GRATITUDE
				@EMPLOYEE_ID INT,
				@DESCRIPTION VARCHAR(64),
				@GRATITUDE_DATE DATE
AS
INSERT INTO GRATITUDE VALUES(@EMPLOYEE_ID,
							 @DESCRIPTION,
							 @GRATITUDE_DATE);
SELECT MAX(ID) FROM GRATITUDE;
GO
--ДОБАВЛЯЕТ ОТПУСК И ВОЗВРАЩАЕТ ЕГО ID
--@EMPLOYEE_ID - ID СЛУЖАЩЕГО
--@DESCRIPTION - ДОП. ИНФОРМАЦИЯ
--@START_HOLIDAY_DATE - ДАТА НАЧАЛА ОТПУСКА
--@FINISH_HOLIDAY_DATE - ДАТА ОКОНЧАНИЯ ОТПУСКА
CREATE PROCEDURE ADD_HOLIDAY_TIME
				@EMPLOYEE_ID INT,
				@DESCRIPTION VARCHAR(64),
				@START_HOLIDAY_DATE DATE,
				@FINISH_HOLIDAY_DATE DATE
AS
INSERT INTO HOLIDAY_TIME VALUES(@EMPLOYEE_ID,
								@DESCRIPTION,
								@START_HOLIDAY_DATE,
								@FINISH_HOLIDAY_DATE);
SELECT MAX(ID) FROM HOLIDAY_TIME;
GO
--ДОБАВЛЯЕТ БОЛЬНИЧНЫЙ И ВОЗВРАЩАЕТ ЕГО ID
--@DISEASED_ID - ID СЛУЖАЩЕГО
--@DESCRIPTION - ДОП. ИНФОРМАЦИЯ
--@START_HOSPITAL_TIME - ДАТА ОТКРЫТИЯ БОЛЬНИЧНОГО
--@FINISH_HOSPITAL_TIME - ДАТА ЗАКРЫТИЯ БОЛЬНИЧНОГО
CREATE PROCEDURE ADD_HOSPITAL_TIME
				@DISEASED_ID INT,
				@DESCRIPTION VARCHAR(64),
				@START_HOSPITAL_TIME DATE,
				@FINISH_HOSPITAL_TIME DATE
AS
INSERT INTO HOSPITAL_TIME VALUES(@DISEASED_ID,
								 @DESCRIPTION,
								 @START_HOSPITAL_TIME,
								 @FINISH_HOSPITAL_TIME);
SELECT MAX(ID) FROM HOSPITAL_TIME;
GO
--ДОБАВЛЯЕТ ПРОХОЖДЕНИЕ СЛУЖБЫ И ВОЗВРАЩАЕТ ЕГО ID
--@EMPLOYEE_ID - ID СЛУЖАЩЕГО
--@DESCRIPTION - ДОП. ИНФОРМЦАЦИЯ
--@START_DATE - ДАТА НАЧАЛА НЕСЕНИЯ СЛУЖБЫ
--@FINISH_DATE - ДАТА ОКОНЧАНИЯ СЛУЖБЫ
--@MILITARY_UNIT_ID - ID ВОИНСКОЙ ЧАСТИ
CREATE PROCEDURE ADD_MILITARY_PROCESS
				@EMPLOYEE_ID INT,
				@DESCRIPTION VARCHAR(64),
				@START_DATE DATE,
				@FINISH_DATE DATE,
				@MILITARY_UNIT_ID INT
AS
INSERT INTO MILITARY_PROCESS VALUES(@EMPLOYEE_ID,
								    @DESCRIPTION,
								    @START_DATE,
								    @FINISH_DATE,
								    @MILITARY_UNIT_ID);
SELECT MAX(ID) FROM MILITARY_PROCESS;
GO
--ДОБАВЛЯЕТ ВОИНСКУЮ ЧАСТЬ И ВОЗВРАЩАЕТ ЕЕ ID
--@MILITARY_NAME - НАЗВАНИЕ/НОМЕР ЧАСТИ
--@DESCRIPTION - ДОП. ИНФОРМАЦИЯ
CREATE PROCEDURE ADD_MILITARY_UNIT
				@MILITARY_NAME VARCHAR(120),
				@DESCRIPTION VARCHAR(240)
AS
INSERT INTO MILITARY_UNIT VALUES(@MILITARY_NAME,
								 @DESCRIPTION);
SELECT MAX(ID) FROM MILITARY_UNIT;
GO
--ДОБАВЛЯЕТ ПАСПОРТ И ВОЗВРАЩАЕТ ЕГО ID
--@ORGANIZATION_UNIT - ОРГАНИЗАЦИЯ, ВЫДАВАВШАЯ ПАСПОРТ
--@NUMBER - НОМЕР ПАСПОРТА
--@SERIES - СЕРИЯ ПАСПОРТА
CREATE PROCEDURE ADD_PASPORT
				@ORGANIZATION_UNIT VARCHAR(120),
				@NUMBER VARCHAR(20),
				@SERIES VARCHAR(5)
AS
INSERT INTO PASPORT VALUES(@ORGANIZATION_UNIT,
						   @NUMBER,
						   @SERIES);
GO
--ДОБАВЛЯЕТ ПРИСВОЕНИЕ ДОЛЖНОСТИ И ВОЗВРАЩАЕТ ЕГО ID
--@EMPLOYEE_ID - ID СЛУЖАЩЕГО
--@DESCRIPTION - ДОП. ИНФОРМАЦИЯ
--@ASSIGNMENT_DATE - ДАТА ПРИСВОЕНИЯ
--@PREV_POST_ID - ID ПРЕДЫДУЩЕЙ ДОЛЖНОСТИ
--@NEW_POST_ID - ID НОВОЙ ДОЛЖНОСТИ
--@ORDER_NUMBER - НОМЕР ПРИКАЗА
CREATE PROCEDURE ADD_POST_ASSIGNMENT
				@EMPLOYEE_ID INT,
				@DESCRIPTION VARCHAR(240),
				@ASSIGNMENT_DATE DATE,
				@PREV_POST_ID INT,
				@NEW_POST_ID INT,
				@ORDER_NUMBER INT
AS
INSERT INTO POST_ASSIGNMENT VALUES(@EMPLOYEE_ID,
								   @DESCRIPTION,
								   @ASSIGNMENT_DATE,
								   @PREV_POST_ID,
								   @NEW_POST_ID,
								   @ORDER_NUMBER);
SELECT MAX(ID) FROM POST_ASSIGNMENT;
GO
--ДОБАВЛЯЕТ ПРИСВОЕНИЕ ЗВАНИЯ И ВОЗВРАЩАЕТ ЕГО ID
--@EMPLOYEE_ID - ID СЛУЖАЩЕГО
--@DESCRIPTION - ДОП. ИНФОРМАЦИЯ
--@ASSIGNMENT_DATE - ДАТА ПРИСВОЕНИЯ
--@PREV_RANK_ID - ID ПРЕДЫДУЩЕЙ ДОЛЖНОСТИ
--@NEW_RANK_ID - ID НОВОЙ ДОЛЖНОСТИ
--@ORDER_NUMBER - НОМЕР ПРИКАЗА
CREATE PROCEDURE ADD_RANK_ASSIGNMENT
				@EMPLOYEE_ID INT,
				@DESCRIPTION VARCHAR(240),
				@ASSIGNMENT_DATE DATE,
				@PREV_RANK_ID INT,
				@NEW_RANK_ID INT,
				@ORDER_NUMBER INT
AS
INSERT INTO RANK_ASSIGNMENT VALUES(@EMPLOYEE_ID,
								   @DESCRIPTION,
								   @ASSIGNMENT_DATE,
								   @PREV_RANK_ID,
								   @NEW_RANK_ID,
								   @ORDER_NUMBER);
SELECT MAX(ID) FROM RANK_ASSIGNMENT;
GO
--ДОБАВЛЯЕТ РОДСТВЕННИКА И ВОЗВРАЩАЕТ ЕГО ID
--@EMPLOYEE_ID - ID СЛУЖАЩЕГО
--@RELATION_TYPE_ID - ID ТИПА РОДСТВА
--@FIRST_NAME - ИМЯ
--@MIDDLE_NAME - ОТЧЕСТВО
--@LAST_NAME - ФАМИЛИЯ
--@BORN_DATE - ДАТА РОЖДЕНИЯ
--@DESCRIPTION - ДОП. ИНФОРМАЦИЯ
CREATE PROCEDURE ADD_RELATIVE
				@EMPLOYEE_ID INT,
				@RELATION_TYPE_ID INT,
				@FIRST_NAME VARCHAR(64),
				@MIDDLE_NAME VARCHAR(64),
				@LAST_NAME VARCHAR(64),
				@BORN_DATE DATE,
				@DESCRIPTION VARCHAR(240)
AS
INSERT INTO RELATIVE VALUES(@EMPLOYEE_ID,
							@RELATION_TYPE_ID,
							@FIRST_NAME,
							@MIDDLE_NAME,
							@LAST_NAME,
							@BORN_DATE,
							@DESCRIPTION);
SELECT MAX(ID) FROM RELATIVE;
GO
--ДОБАВЛЯЕТ ВЫГОВОР И ВОЗВРАЩАЕТ ЕГО ID
--@EMPLOYEE_ID - ID СЛУЖАЩЕГО
--@SUM_OF_REPRIMAND - СУММА ВЫГОВОРА
--@REPRIMAND_DATE - ДАТА ВЫНЕСЕНИЯ ВЫГОВОРА
--@DESCRIPTION - ДОП. ИНФОРМАЦИЯ
CREATE PROCEDURE ADD_REPRIMAND
				@EMPLOYEE_ID INT,
				@SUM_OF_REPRIMAND FLOAT,
				@REPRIMAND_DATE DATE,
				@DESCRIPTION VARCHAR(240)
AS
INSERT INTO REPRIMAND VALUES(@EMPLOYEE_ID,
							 @SUM_OF_REPRIMAND,
							 @REPRIMAND_DATE,
							 @DESCRIPTION);
SELECT MAX(ID) FROM REPRIMAND;
GO
--ДОБАВЛЯЕТ АТТЕСТАЦИЮ И ВОЗВРАЩАЕТ ЕЁ ID
--@EMPLOYEE_ID - ID СЛУЖАЩЕГО
--@SERTIFICATION_DATE - ДАТА ВЫНЕСЕНИЯ ВЫГОВОРА
--@DESCRIPTION - ДОП. ИНФОРМАЦИЯ
CREATE PROCEDURE ADD_SERTIFICATION
				@EMPLOYEE_ID INT,
				@SERTIFICATION_DATE DATE,
				@DESCRIPTION VARCHAR(240)
AS
INSERT INTO SERTIFICATION VALUES(@EMPLOYEE_ID,
								 @SERTIFICATION_DATE,
								 @DESCRIPTION);
SELECT MAX(ID) FROM SERTIFICATION;
GO
--ДОБАВЛЯЕТ ВЫГОВОР И ВОЗВРАЩАЕТ ЕГО ID
--@VIOLATOR_ID - ID СЛУЖАЩЕГО
--@DESCRIPTION - ДОП. ИНФОРМАЦИЯ
--@VIOLATION_DATE - ДАТА НАРУШЕНИЯ
CREATE PROCEDURE ADD_VIOLATION
				@VIOLATOR_ID INT,
				@DESCRIPTION VARCHAR(240),
				@VIOLATION_DATE DATE
AS
INSERT INTO VIOLATION VALUES(@VIOLATOR_ID,
							 @DESCRIPTION,
							 @VIOLATION_DATE);
SELECT MAX(ID) FROM VIOLATION;
GO
--ДОБАВЛЯЕТ СПЕЦИАЛЬНОСТЬ И ВОЗВРАЩАЕТ ЕЕ ID
--@SPECIALITY - НАЗВАНИЕ СПЕЦИАЛЬНОСТИ
--@DESCRIPTION - ДОП. ИНФОРМАЦИЯ
CREATE PROCEDURE ADD_SPECIALITY
				@SPECIALITY VARCHAR(64),
				@DESCRIPTION VARCHAR(120)
AS
INSERT INTO SPECIALITY VALUES(@SPECIALITY,
							  @DESCRIPTION);
SELECT MAX(ID) FROM SPECIALITY;
GO
--ДОБАВЛЯЕТ ПОЛЬЗОВАТЕЛЯ И ВОЗВРАЩАЕТ ЕГО ID
--@USER_LOGIN - ЛОГИН
--@USER_PASSWORD - ПАРОЛЬ
--@ACCESS_LEVEL - УРОВЕНЬ ДОСТУПА
--@LAST_NAME - ФАМИЛИЯ
--@FIRST_NAME - ИМЯ
--@MIDDLE_NAME - ОТЧЕСТВО
CREATE PROCEDURE ADD_USERS
				@USER_LOGIN VARCHAR(20),
				@USER_PASSWORD VARCHAR(20),
				@ACCESS_LEVEL INT,
				@LAST_NAME VARCHAR(50),
				@FIRST_NAME VARCHAR(50),
				@MIDDLE_NAME VARCHAR(50)
AS
INSERT INTO USERS VALUES(@USER_LOGIN,
						 @USER_PASSWORD,
						 @ACCESS_LEVEL,
						 @LAST_NAME,
						 @FIRST_NAME,
						 @MIDDLE_NAME);
SELECT MAX(ID) FROM USERS;

GO
-------------------------------------------------------------------------
---TRIGGERS
-------------------------------------------------------------------------

-------------------------------------------------------------------------
---EOF
-------------------------------------------------------------------------