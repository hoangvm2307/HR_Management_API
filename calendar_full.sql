drop database swp_project
create database swp_project
use swp_project
drop table HolidayDimension
drop table DateDimension
drop view TheCalendar

DECLARE @StartDate  date = '20230101';

DECLARE @CutoffDate date = DATEADD(DAY, -1, DATEADD(YEAR, 1, @StartDate));

;WITH seq(n) AS 
(
  SELECT 0 UNION ALL SELECT n + 1 FROM seq
  WHERE n < DATEDIFF(DAY, @StartDate, @CutoffDate)
),
d(d) AS 
(
  SELECT DATEADD(DAY, n, @StartDate) FROM seq
),
src AS
(
  SELECT
    TheDate         = CONVERT(date, d),
    TheDay          = DATEPART(DAY,       d),
    TheDayName      = DATENAME(WEEKDAY,   d),
    TheWeek         = DATEPART(WEEK,      d),
	TheDayOfWeek    = DATEPART(WEEKDAY,   d),
    TheMonth        = DATEPART(MONTH,     d),
    TheMonthName    = DATENAME(MONTH,     d),
    TheQuarter      = DATEPART(Quarter,   d),
    TheYear         = DATEPART(YEAR,      d),
	TheFirstOfMonth = DATEFROMPARTS(YEAR(d), MONTH(d), 1)
  FROM d
),
dim AS
(
  SELECT
    TheDate, 
    TheDay,
    TheDayName,
    TheDayOfWeekInMonth = CONVERT(tinyint, ROW_NUMBER() OVER 
                            (PARTITION BY TheFirstOfMonth, TheDayOfWeek ORDER BY TheDate)),
    IsWeekend           = CASE WHEN TheDayOfWeek IN (CASE @@DATEFIRST WHEN 1 THEN 6 WHEN 7 THEN 1 END,7) 
                            THEN 1 ELSE 0 END,
    TheWeek,
	TheDayOfWeek,
    TheMonth,
    TheMonthName,
    TheQuarter,
    TheYear,
	TheFirstOfYear      = DATEFROMPARTS(TheYear, 1,  1),
	TheFirstOfMonth,
    Style101            = CONVERT(char(10), TheDate, 101)
  FROM src
)
SELECT * INTO dbo.DateDimension FROM dim
  ORDER BY TheDate
  OPTION (MAXRECURSION 0);

 --------------- ===== ---------------
  CREATE UNIQUE CLUSTERED INDEX PK_DateDimension ON dbo.DateDimension(TheDate);

 --------------- ===== ---------------
  CREATE TABLE dbo.HolidayDimension
(
  TheDate date NOT NULL,
  HolidayText nvarchar(255) NOT NULL,
  CONSTRAINT FK_DateDimension FOREIGN KEY(TheDate) REFERENCES dbo.DateDimension(TheDate)
);

CREATE CLUSTERED INDEX CIX_HolidayDimension ON dbo.HolidayDimension(TheDate);

ALTER TABLE DateDimension ADD uniqueId INT IDENTITY(1,1) primary key

GO

--------------- ===== ---------------
;WITH x AS 
(
  SELECT
    TheDate,
    TheFirstOfYear,
    TheDayOfWeekInMonth, 
    TheMonth, 
    TheDayName, 
    TheDay,
    TheLastDayOfWeekInMonth = ROW_NUMBER() OVER 
    (
      PARTITION BY TheFirstOfMonth, TheDayOfWeek
      ORDER BY TheDate DESC
    )
  FROM dbo.DateDimension
),
s AS
(
  SELECT TheDate, HolidayText = CASE
  WHEN (TheMonth = 1 AND TheDay = 2)
    THEN 'New Year''s Day' 
	WHEN (TheMonth = 2 AND TheDay = 20)
    THEN 'Lunar New Year' 
	WHEN (TheMonth = 2 AND TheDay = 21)
    THEN 'Lunar New Year' 
	WHEN (TheMonth = 2 AND TheDay = 22)
    THEN 'Lunar New Year' 
	WHEN (TheMonth = 2 AND TheDay = 23)
    THEN 'Lunar New Year' 
	WHEN (TheMonth = 2 AND TheDay = 24)
    THEN 'Lunar New Year' 
	WHEN (TheMonth = 2 AND TheDay = 25)
    THEN 'Lunar New Year' 
	WHEN (TheMonth = 2 AND TheDay = 26)
    THEN 'Lunar New Year' 
  WHEN (TheMonth = 5 AND TheDay = 1)
    THEN 'International Works'' Day'  
	WHEN (TheMonth = 5 AND TheDay = 2)
    THEN 'Compensatory Leave for the Hung Kings'' Temple Festival'  
	WHEN (TheMonth = 5 AND TheDay = 3)
    THEN 'Compensatory Leave for Liberation Day'  
  WHEN (TheMonth = 9 AND TheDay = 1)
    THEN 'Vietnam National Day'
  WHEN (TheMonth = 9 AND TheDay = 4)
    THEN 'Vietnam National Day'
  WHEN (TheMonth = 12 AND TheDay = 31)
    THEN 'New Year''s Day'
 -- gio to Hung Vuong
 -- tet am
  END
  FROM x
  WHERE 
	(TheMonth = 1 AND TheDay = 2)
	OR (TheMonth = 2 AND TheDay = 20)
	OR (TheMonth = 2 AND TheDay = 21)
	OR (TheMonth = 2 AND TheDay = 22)
	OR (TheMonth = 2 AND TheDay = 23)
	OR (TheMonth = 2 AND TheDay = 24)
	OR (TheMonth = 2 AND TheDay = 25)
	OR (TheMonth = 2 AND TheDay = 26)
	OR (TheMonth = 5 AND TheDay = 1)
	OR (TheMonth = 5 AND TheDay = 2)
	OR (TheMonth = 5 AND TheDay = 3)  
	OR (TheMonth = 9 AND TheDay = 1)
	OR (TheMonth = 9 AND TheDay = 4)
	OR (TheMonth = 12 AND TheDay = 31)

)

INSERT dbo.HolidayDimension(TheDate, HolidayText)
SELECT TheDate, HolidayText FROM s 
/*

UNION ALL 
SELECT DATEADD(DAY, 1, TheDate), 'Black Friday'
  FROM s WHERE HolidayText = 'Thanksgiving Day'
*/

ORDER BY TheDate;
--------------- ===== ---------------
CREATE VIEW dbo.TheCalendar
AS 
  SELECT 
    d.TheDate,
    d.TheDay,
    d.TheDayName,
    d.TheDayOfWeek,
    d.TheDayOfWeekInMonth,
    d.IsWeekend,
    d.TheWeek,
    d.TheMonth,
    d.TheMonthName,
    d.TheFirstOfMonth,
    d.TheQuarter,
    d.TheYear,
    d.TheFirstOfYear,
    d.Style101,
    IsHoliday = CASE WHEN h.TheDate IS NOT NULL THEN 1 ELSE 0 END,
    h.HolidayText,
	isWorking = CASE WHEN IsWeekend = 1 OR h.TheDate IS NOT NULL THEN 0 ELSE 1 END,
    'Percent' = CASE 
                WHEN h.TheDate IS NOT NULL AND IsWeekend = 1 THEN 4.0
                WHEN h.TheDate IS NOT NULL THEN 3.0
                WHEN IsWeekend = 1 THEN 2.0
                ELSE 1.0 
              END
  FROM dbo.DateDimension AS d
  LEFT OUTER JOIN dbo.HolidayDimension AS h
  ON d.TheDate = h.TheDate;

--------------- ===== ---------------


-- Fake Data Type

-- 1, 3, 7 , 9, 11, 14, , 16, 19
-- 1. Role
INSERT INTO Role VALUES ('HRM', 'HR Manager')
INSERT INTO Role VALUES ('HRS', 'HR Staff')
INSERT INTO Role VALUES ('S', 'Staff')

-- 3. Department
INSERT INTO Department(departmentName) VALUES ('Human Resources')
INSERT INTO Department(departmentName) VALUES ('IT')
INSERT INTO Department(departmentName) VALUES ('Accounting and Finance')
INSERT INTO Department(departmentName) VALUES ('Research and Development')
INSERT INTO Department(departmentName) VALUES ('Marketing')

-- 7. Contract Type
INSERT INTO ContractType (name) VALUES (N'Hợp đồng không xác định hạn')
INSERT INTO ContractType (name) VALUES (N'Hợp đồng xác định thời hạn')

-- 9. Allowance Type
INSERT INTO AllowanceType VALUES (N'Phụ cấp trách nhiệm', N'Không quá 10%')
INSERT INTO AllowanceType VALUES (N'Phụ cấp thu hút', N'Dưới 35%')
INSERT INTO AllowanceType VALUES (N'Phụ cấp lưu động', N'Dưới 10%')
INSERT INTO AllowanceType VALUES (N'Phụ cấp chức vụ, chức danh', N'Dưới 15%')
INSERT INTO AllowanceType VALUES (N'Phụ cấp gửi xe và ăn trưa', N'Tùy thỏa thuận')

-- 11. Tax List
INSERT INTO TaxList VALUES (N'Đến 5 triệu VND', 5000000, 0.05)
INSERT INTO TaxList VALUES (N'Trên 5 triệu VND đến 10 triệu VND', 5000000, 0.1)
INSERT INTO TaxList VALUES (N'Trên 10 triệu VND đến 18 triệu VND', 8000000, 0.15)
INSERT INTO TaxList VALUES (N'Trên 18 triệu VND đến 32 triệu VND', 14000000, 0.2)
INSERT INTO TaxList VALUES (N'Trên 32 triệu VND đến 52 triệu VND', 20000000, 0.25)
INSERT INTO TaxList VALUES (N'Trên 52 triệu VND đến 80 triệu VND', 28000000, 0.3)
INSERT INTO TaxList VALUES (N'Trên 80 triệu VND ', 80000000, 0.35)

-- 14. Ticket Type
INSERT INTO TicketType VALUES (N'Đơn xin nghỉ việc')
INSERT INTO TicketType VALUES (N'Yêu cầu cập nhật thông tin cá nhân')
INSERT INTO TicketType VALUES (N'Yêu cầu cập nhật hợp đồng')
INSERT INTO TicketType VALUES (N'Đơn đề cử ứng viên')
INSERT INTO TicketType VALUES (N'Các loại đơn khác')

-- 16. OT Type
INSERT INTO OtType VALUES (N'Làm thêm ngày trong tuần', 2)
INSERT INTO OtType VALUES (N'Làm thêm ngày cuối tuần', 3)
INSERT INTO OtType VALUES (N'Làm thêm ngày lễ', 4)
-- 19. Leave Type
INSERT INTO LeaveType VALUES (N'Nghỉ thai sản', '', 180, 1)
INSERT INTO LeaveType VALUES (N'Nghỉ phép có lương', 'Nghỉ phép năm', 12, 1)
INSERT INTO LeaveType VALUES (N'Nghỉ không lương', 'Nghỉ không lương', 20, 0)

