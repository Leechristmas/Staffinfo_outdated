ALTER TABLE CLASINESS
	DROP
		CONSTRAINT FK_CLASINESS_EMPLOYEE;

GO
/*
ALTER TABLE PASPORT
	DROP
		CONSTRAINT FK_PASPORT_PASPORT_ORGANIZATION_UNIT;

GO
*/
ALTER TABLE CONTRACT
	DROP
		CONSTRAINT	FK_CONTRACT_EMPLOYEE;

GO

ALTER TABLE EDUCATION_TIME
	DROP
		CONSTRAINT	FK_EDUCATION_TIME_EDUCATIONAL_INSTITUTION,
		CONSTRAINT	FK_EDUCATION_TIME_EMPLOYEE,
		CONSTRAINT	FK_EDUCATION_TIME_SPECIALITY;

GO

ALTER TABLE EMPLOYEE
	DROP
		CONSTRAINT	FK_EMPLOYEE_POST,
		CONSTRAINT	FK_EMPLOYEE_RANK,
		CONSTRAINT	FK_EMPLOYEE_PASPORT;

GO

ALTER TABLE GRATITUDE
	DROP
		CONSTRAINT	FK_GRATITUDE_EMPLOYEE;

GO

ALTER TABLE HOLIDAY_TIME
	DROP
		CONSTRAINT	FK_HOLIDAY_TIME_EMPLOYEE;

GO

ALTER TABLE HOSPITAL_TIME
	DROP
		CONSTRAINT	FK_HOSPITAL_TIME_EMPLOYEE;

GO

ALTER TABLE MILITARY_PROCESS
	DROP
		CONSTRAINT	FK_MILITARY_PROCESS_EMPLOYEE,
		CONSTRAINT	FK_MILITARY_PROCESS_MILITARY_UNIT;

GO

ALTER TABLE POST
	DROP 
		CONSTRAINT	FK_POST_SERVICE;

GO

ALTER TABLE POST_ASSIGNMENT
	DROP
		CONSTRAINT	FK_POST_ASSIGNMENT_TB_EMLOYEE,
		CONSTRAINT	FK_POST_ASSIGNMENT_POST_NEW,
		CONSTRAINT	FK_POST_ASSIGNMENT_POST_PREV;

GO

ALTER TABLE RANK_ASSIGNMENT
	DROP	
		CONSTRAINT	FK_RANK_ASSIGNMENT_TB_EMLOYEE,
		CONSTRAINT	FK_RANK_ASSIGNMENT_RANK_NEW,
		CONSTRAINT	FK_RANK_ASSIGNMENT_RANK_PREV;

GO

ALTER TABLE RELATIVE
	DROP
		CONSTRAINT	FK_RELATIVE_EMPLOYEE,
		CONSTRAINT	FK_RELATIVE_TB_RELATION_TYPE;

GO

ALTER TABLE REPRIMAND
	DROP
		CONSTRAINT	FK_REPRIMAND_EMPLOYEE;

GO

ALTER TABLE SERTIFICATION
	DROP
		CONSTRAINT	FK_SERTIFICATION_EMPLOYEE;

GO

ALTER TABLE VIOLATION
	DROP
		CONSTRAINT	FK_VIOLATION_EMPLOYEE;

DROP TABLE POST;
DROP TABLE RANK;
DROP TABLE CLASINESS;
DROP TABLE CONTRACT;
DROP TABLE EDUCATIONAL_INSTITUTION; 
DROP TABLE EDUCATION_TIME;
DROP TABLE GRATITUDE;
DROP TABLE HOLIDAY_TIME;
DROP TABLE HOSPITAL_TIME;
DROP TABLE MILITARY_UNIT;
DROP TABLE MILITARY_PROCESS;
DROP TABLE POST_ASSIGNMENT;
DROP TABLE RANK_ASSIGNMENT;
DROP TABLE RELATIVE_TYPE;
DROP TABLE RELATIVE;
DROP TABLE SERTIFICATION;
DROP TABLE SERVICE;
DROP TABLE REPRIMAND;
DROP TABLE SPECIALITY;
DROP TABLE EMPLOYEE;
DROP TABLE VIOLATION;
--DROP TABLE PASPORT_ORGANIZATION_UNIT;
DROP TABLE PASPORT;
DROP TABLE USERS;

DROP PROCEDURE CHECK_USER;

GO

CREATE PROCEDURE CHECK_USER 
	@LOGIN VARCHAR(20), @PASSWORD VARCHAR(20)
AS
	SELECT * FROM USERS WHERE USER_LOGIN = @LOGIN AND USER_PASSWORD = @PASSWORD;

GO

CREATE TABLE USERS(
	ID INT PRIMARY KEY IDENTITY,
	USER_LOGIN VARCHAR(20) NOT NULL,
	USER_PASSWORD VARCHAR(20) NOT NULL,
	ACCESS_LEVEL INT NOT NULL)

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
		INST_TITLE VARCHAR(64) NOT NULL, 
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
	MILITARY_UNIT_ID INT NOT NULL
);

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
	RELATION_TYPE_ID INT NOT NULL,
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
/*
--ОРГАНИЗАЦИИ (ПАСПОРТНЫЕ СТОЛЫ)
CREATE TABLE PASPORT_ORGANIZATION_UNIT(
	ID INT PRIMARY KEY IDENTITY,
	ORGANIZATION_NAME VARCHAR(120),
	ADDRESS VARCHAR(120))

GO
*/
--ПАСПОРТНЫЕ ДАННЫЕ
CREATE TABLE PASPORT(
	ID INT PRIMARY KEY IDENTITY,
	--ORGANIZATION_UNIT_ID INT,
	ORGANIZATION_UNIT VARCHAR(120),
	NUMBER VARCHAR(20),
	SERIES VARCHAR(5))

GO

ALTER TABLE PASPORT
	ADD
		CONSTRAINT	FK_PASPORT_UNIQUE
					UNIQUE (NUMBER, SERIES);
		--CONSTRAINT	FK_PASPORT_PASPORT_ORGANIZATION_UNIT
		--			FOREIGN KEY (ORGANIZATION_UNIT_ID) REFERENCES PASPORT_ORGANIZATION_UNIT;

GO

ALTER TABLE CLASINESS
	ADD 
		CONSTRAINT	FK_CLASINESS_EMPLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE;

GO

ALTER TABLE CONTRACT
	ADD 
		CONSTRAINT	FK_CONTRACT_EMPLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE;

GO

ALTER TABLE EDUCATION_TIME
	ADD 
		CONSTRAINT	FK_EDUCATION_TIME_EMPLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE,
		CONSTRAINT	FK_EDUCATION_TIME_EDUCATIONAL_INSTITUTION
					FOREIGN KEY (INSTITUTION_ID) REFERENCES EDUCATIONAL_INSTITUTION,
		CONSTRAINT	FK_EDUCATION_TIME_SPECIALITY
					FOREIGN KEY (SPECIALITY_ID) REFERENCES SPECIALITY;

GO

ALTER TABLE EMPLOYEE
	ADD
		CONSTRAINT	FK_EMPLOYEE_POST
					FOREIGN KEY	(POST_ID) REFERENCES POST,
		CONSTRAINT	FK_EMPLOYEE_RANK
					FOREIGN KEY	(RANK_ID) REFERENCES RANK,
		CONSTRAINT	FK_EMPLOYEE_PASPORT
					FOREIGN KEY (PASPORT_ID) REFERENCES PASPORT;

GO

ALTER TABLE GRATITUDE
	ADD
		CONSTRAINT	FK_GRATITUDE_EMPLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE;

GO

ALTER TABLE HOLIDAY_TIME
	ADD
		CONSTRAINT	FK_HOLIDAY_TIME_EMPLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE;

GO

ALTER TABLE HOSPITAL_TIME
	ADD
		CONSTRAINT	FK_HOSPITAL_TIME_EMPLOYEE
					FOREIGN KEY (DISEASED_ID) REFERENCES EMPLOYEE;

GO

ALTER TABLE MILITARY_PROCESS
	ADD
		CONSTRAINT	FK_MILITARY_PROCESS_EMPLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE,
		CONSTRAINT	FK_MILITARY_PROCESS_MILITARY_UNIT
					FOREIGN KEY (MILITARY_UNIT_ID) REFERENCES MILITARY_UNIT;

GO

ALTER TABLE POST
	ADD
		CONSTRAINT	FK_POST_SERVICE
					FOREIGN KEY (SERVICE_ID) REFERENCES SERVICE,
		CONSTRAINT	UNIQUE_CONSTRAINT
					UNIQUE(POST_TITLE,SERVICE_ID);

GO

ALTER TABLE POST_ASSIGNMENT
	ADD
		CONSTRAINT	FK_POST_ASSIGNMENT_TB_EMLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE,
		CONSTRAINT	FK_POST_ASSIGNMENT_POST_PREV
					FOREIGN KEY (PREV_POST_ID) REFERENCES POST,
		CONSTRAINT	FK_POST_ASSIGNMENT_POST_NEW
					FOREIGN KEY (NEW_POST_ID) REFERENCES POST;

GO

ALTER TABLE RANK_ASSIGNMENT
	ADD
		CONSTRAINT	FK_RANK_ASSIGNMENT_TB_EMLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE,
		CONSTRAINT	FK_RANK_ASSIGNMENT_RANK_PREV
					FOREIGN KEY (PREV_RANK_ID) REFERENCES RANK,
		CONSTRAINT	FK_RANK_ASSIGNMENT_RANK_NEW
					FOREIGN KEY (NEW_RANK_ID) REFERENCES RANK;

GO

ALTER TABLE RELATIVE
	ADD
		CONSTRAINT	FK_RELATIVE_EMPLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE,
		CONSTRAINT	FK_RELATIVE_TB_RELATION_TYPE
					FOREIGN KEY	(RELATION_TYPE_ID) REFERENCES RELATIVE_TYPE;

GO

ALTER TABLE REPRIMAND
	ADD
		CONSTRAINT	FK_REPRIMAND_EMPLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE;

GO

ALTER TABLE SERTIFICATION
	ADD
		CONSTRAINT	FK_SERTIFICATION_EMPLOYEE
					FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEE;

GO

ALTER TABLE VIOLATION
	ADD
		CONSTRAINT	FK_VIOLATION_EMPLOYEE
					FOREIGN KEY (VIOLATOR_ID) REFERENCES EMPLOYEE;


----------------------------------------------------
GO

INSERT INTO USERS VALUES('ADMIN', 'admin', 1),
						('READER', 'reader', 0);

GO

INSERT INTO EDUCATIONAL_INSTITUTION
	VALUES('ГГУ им. Ф. Скорнины', '', 'ВУЗ'),
		('ГГTУ им. Сухого', 'Технический ВУЗ', 'ВУЗ');

GO

INSERT INTO MILITARY_UNIT
	VALUES('Военная часть 5505','г. Гомель'),
		('Военная часть 3216','г. Минск');

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

INSERT INTO RELATIVE_TYPE
	VALUES('Супруг(а)'),
	('Сын'),('Дочь'),('Мать'),('Отец');

GO

INSERT INTO SPECIALITY
	VALUES('Спасатель', ''),
	('Инженер-механик',''),
	('Инструктор', '');

GO

INSERT INTO PASPORT_ORGANIZATION_UNIT
	VALUES('Гомельский РОВД', 'г. ГОМЕЛЬ'),
	('Областной РОВД Гомельской области', 'г. ГОМЕЛЬ');

GO

INSERT INTO PASPORT
	VALUES('Гомельский РОВД', 226023, 'HB'),
	('Областной РОВД Гомельской области', 226012, 'HB'),
	('Гомельский РОВД', 222123, 'HB'),
	('Областной РОВД Гомельской области', 232017, 'HB');
GO

INSERT INTO EMPLOYEE
	VALUES('Алексей', 'Иванович', 'Петров',11423,1,2,'1972-01-01', '2000-02-04', 'Смолевичи#Интернациональная#123#32', 1, 'типа номер','номерок', 0, NULL),
	('Виктор', 'Сергеевич', 'Скворцов', 10233, 2,3, '1973-04-07', '2002-10-07', 'Витебск#Ленина#234#2342', 2, 'типа номер','номерок', 0, NULL),
	('Петр', 'Александрович', 'Головач', 10443, 1,1, '1979-02-11', '2004-04-02', 'Торжок#Ленина#232#12', 3, 'типа номер','номерок', 0, NULL),
	('Иван', 'Федорович', 'Кедров', 13228, 3,3, '1976-01-12', '2002-05-01', 'Гомель#Советская#21#12', 4, 'типа номер','номерок', 0, NULL);

GO

INSERT INTO CLASINESS
	VALUES(3,2,'2000-03-02',2,'типа классность');

GO

INSERT INTO CONTRACT
	VALUES(1,'2000-01-06', '2002-02-01', 'decsription');

GO

INSERT INTO EDUCATION_TIME
	VALUES(2, '1990-01-06', '1995-02-01', 1, 1, 'education_time_description');