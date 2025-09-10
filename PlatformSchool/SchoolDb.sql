USE SchoolDb;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'SchoolDb')
BEGIN
    CREATE DATABASE SchoolDb;
END
GO

USE SchoolDb;
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Department]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Department](
        [DepartmentId] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](100) NOT NULL,
        [Budget] [decimal](18,2) NOT NULL,
        [StartDate] [datetime] NOT NULL,
        [Administrator] [int] NULL,
        [CreationDate] [datetime] NOT NULL,
        [ModifyDate] [datetime] NULL,
        [CreationUser] [int] NOT NULL,
        [UserMod] [int] NULL,
        [UserDeleted] [int] NULL,
        [DeletedDate] [datetime] NULL,
        [Deleted] [bit] NOT NULL DEFAULT(0),
        CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED ([DepartmentId] ASC)
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Course]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Course](
        [CourseId] [int] IDENTITY(1,1) NOT NULL,
        [Title] [nvarchar](100) NOT NULL,
        [Credits] [int] NOT NULL,
        [DepartmentId] [int] NOT NULL,
        [CreationDate] [datetime] NOT NULL,
        [ModifyDate] [datetime] NULL,
        [CreationUser] [int] NOT NULL,
        [UserMod] [int] NULL,
        [UserDeleted] [int] NULL,
        [DeletedDate] [datetime] NULL,
        [Deleted] [bit] NOT NULL DEFAULT(0),
        CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED ([CourseId] ASC)
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Course_Department]') AND parent_object_id = OBJECT_ID(N'[dbo].[Course]'))
BEGIN
    ALTER TABLE [dbo].[Course] ADD CONSTRAINT [FK_Course_Department] 
    FOREIGN KEY([DepartmentId]) REFERENCES [dbo].[Department] ([DepartmentId]);
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_InsertDepartment]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[SP_InsertDepartment]
GO

CREATE PROCEDURE [dbo].[SP_InsertDepartment]
    @Name NVARCHAR(100),
    @Budget DECIMAL(18,2),
    @StartDate DATETIME,
    @Administrator INT = NULL,
    @CreationUser INT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO [Department] (Name, Budget, StartDate, Administrator, CreationDate, CreationUser, Deleted)
    VALUES (@Name, @Budget, @StartDate, @Administrator, GETDATE(), @CreationUser, 0);
    
    SELECT SCOPE_IDENTITY() AS DepartmentId;
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_UpdateDepartment]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[SP_UpdateDepartment]
GO

CREATE PROCEDURE [dbo].[SP_UpdateDepartment]
    @DepartmentId INT,
    @Name NVARCHAR(100),
    @Budget DECIMAL(18,2),
    @StartDate DATETIME,
    @Administrator INT = NULL,
    @UserMod INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE [Department] 
    SET Name = @Name,
        Budget = @Budget,
        StartDate = @StartDate,
        Administrator = @Administrator,
        ModifyDate = GETDATE(),
        UserMod = @UserMod
    WHERE DepartmentId = @DepartmentId AND Deleted = 0;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_DeleteDepartment]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[SP_DeleteDepartment]
GO

CREATE PROCEDURE [dbo].[SP_DeleteDepartment]
    @DepartmentId INT,
    @UserDeleted INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE [Department] 
    SET Deleted = 1,
        DeletedDate = GETDATE(),
        UserDeleted = @UserDeleted
    WHERE DepartmentId = @DepartmentId AND Deleted = 0;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_InsertCourse]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[SP_InsertCourse]
GO

CREATE PROCEDURE [dbo].[SP_InsertCourse]
    @Title NVARCHAR(100),
    @Credits INT,
    @DepartmentId INT,
    @CreationUser INT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO [Course] (Title, Credits, DepartmentId, CreationDate, CreationUser, Deleted)
    VALUES (@Title, @Credits, @DepartmentId, GETDATE(), @CreationUser, 0);
    
    SELECT SCOPE_IDENTITY() AS CourseId;
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GetAllDepartments]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[SP_GetAllDepartments]
GO

CREATE PROCEDURE [dbo].[SP_GetAllDepartments]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT DepartmentId, Name, Budget, StartDate, Administrator, 
           CreationDate, ModifyDate, CreationUser, UserMod
    FROM [Department] 
    WHERE Deleted = 0
    ORDER BY Name;
END
GO

-- Stored Procedures para Cursos
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerCursos]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[ObtenerCursos]
GO

CREATE PROCEDURE [dbo].[ObtenerCursos]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        c.CourseId,
        c.Title,
        c.Credits,
        c.DepartmentId,
        d.Name AS DepartmentName,
        c.CreationDate,
        FORMAT(c.CreationDate, 'dd/MM/yyyy HH:mm') AS CreationDateDisplay
    FROM [Course] c
    INNER JOIN [Department] d ON c.DepartmentId = d.DepartmentId
    WHERE c.Deleted = 0
    ORDER BY c.Title;
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerCursoPorId]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[ObtenerCursoPorId]
GO

CREATE PROCEDURE [dbo].[ObtenerCursoPorId]
    @p_CourseId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        c.CourseId,
        c.Title,
        c.Credits,
        c.DepartmentId,
        d.Name AS DepartmentName,
        c.CreationDate,
        FORMAT(c.CreationDate, 'dd/MM/yyyy HH:mm') AS CreationDateDisplay
    FROM [Course] c
    INNER JOIN [Department] d ON c.DepartmentId = d.DepartmentId
    WHERE c.CourseId = @p_CourseId AND c.Deleted = 0;
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerCursosPorDepartamento]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[ObtenerCursosPorDepartamento]
GO

CREATE PROCEDURE [dbo].[ObtenerCursosPorDepartamento]
    @p_DepartmentId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        c.CourseId,
        c.Title,
        c.Credits,
        c.DepartmentId,
        d.Name AS DepartmentName,
        c.CreationDate,
        FORMAT(c.CreationDate, 'dd/MM/yyyy HH:mm') AS CreationDateDisplay
    FROM [Course] c
    INNER JOIN [Department] d ON c.DepartmentId = d.DepartmentId
    WHERE c.DepartmentId = @p_DepartmentId AND c.Deleted = 0
    ORDER BY c.Title;
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AgregarCurso]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[AgregarCurso]
GO

CREATE PROCEDURE [dbo].[AgregarCurso]
    @p_Title NVARCHAR(100),
    @p_Credits INT,
    @p_DepartmentId INT,
    @p_CreateUser INT,
    @p_result NVARCHAR(1000) OUTPUT,
    @p_CourseId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        INSERT INTO [Course] (Title, Credits, DepartmentId, CreationDate, CreationUser, Deleted)
        VALUES (@p_Title, @p_Credits, @p_DepartmentId, GETDATE(), @p_CreateUser, 0);
        
        SET @p_CourseId = SCOPE_IDENTITY();
        SET @p_result = 'Curso creado exitosamente';
    END TRY
    BEGIN CATCH
        SET @p_result = 'Error: ' + ERROR_MESSAGE();
        SET @p_CourseId = 0;
    END CATCH
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActualizarCurso]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[ActualizarCurso]
GO

CREATE PROCEDURE [dbo].[ActualizarCurso]
    @CourseId INT,
    @Title NVARCHAR(100),
    @Credits INT,
    @DepartmentId INT,
    @UserMod INT,
    @p_result NVARCHAR(1000) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        UPDATE [Course] 
        SET Title = @Title,
            Credits = @Credits,
            DepartmentId = @DepartmentId,
            ModifyDate = GETDATE(),
            UserMod = @UserMod
        WHERE CourseId = @CourseId AND Deleted = 0;
        
        IF @@ROWCOUNT > 0
            SET @p_result = 'Curso actualizado exitosamente';
        ELSE
            SET @p_result = 'No se encontró el curso para actualizar';
    END TRY
    BEGIN CATCH
        SET @p_result = 'Error: ' + ERROR_MESSAGE();
    END CATCH
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EliminarCurso]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[EliminarCurso]
GO

CREATE PROCEDURE [dbo].[EliminarCurso]
    @p_CourseId INT,
    @p_result NVARCHAR(1000) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        UPDATE [Course] 
        SET Deleted = 1,
            DeletedDate = GETDATE()
        WHERE CourseId = @p_CourseId AND Deleted = 0;
        
        IF @@ROWCOUNT > 0
            SET @p_result = 'Curso eliminado exitosamente';
        ELSE
            SET @p_result = 'No se encontró el curso para eliminar';
    END TRY
    BEGIN CATCH
        SET @p_result = 'Error: ' + ERROR_MESSAGE();
    END CATCH
END
GO

IF NOT EXISTS (SELECT * FROM [Department] WHERE Name = 'Desarrollo de Software')
BEGIN
    EXEC [SP_InsertDepartment] 
        @Name = 'Desarrollo de Software',
        @Budget = 150000.00,
        @StartDate = '2024-01-01',
        @Administrator = 1,
        @CreationUser = 1;
END

IF NOT EXISTS (SELECT * FROM [Course] WHERE Title = 'Programacion 2')
BEGIN
    DECLARE @CourseId INT;
    EXEC @CourseId = [SP_InsertCourse] 
        @Title = 'Programacion 2',
        @Credits = 4,
        @DepartmentId = 1,
        @CreationUser = 1;
END

IF NOT EXISTS (SELECT * FROM [Course] WHERE Title = 'Electiva de Programacion')
BEGIN
    DECLARE @CourseId2 INT;
    EXEC @CourseId2 = [SP_InsertCourse] 
        @Title = 'Electiva de Programacion',
        @Credits = 3,
        @DepartmentId = 1,
        @CreationUser = 1;
END

-- datos de prueba
IF NOT EXISTS (SELECT * FROM [Department] WHERE Name = 'Ciencias de la Computación')
BEGIN
    EXEC [SP_InsertDepartment] 
        @Name = 'Ciencias de la Computación',
        @Budget = 350000.00,
        @StartDate = '2020-01-01',
        @Administrator = 2,
        @CreationUser = 1;
END

IF NOT EXISTS (SELECT * FROM [Department] WHERE Name = 'Matemáticas')
BEGIN
    EXEC [SP_InsertDepartment] 
        @Name = 'Matemáticas',
        @Budget = 250000.00,
        @StartDate = '2020-01-01',
        @Administrator = 3,
        @CreationUser = 1;
END

IF NOT EXISTS (SELECT * FROM [Department] WHERE Name = 'Física')
BEGIN
    EXEC [SP_InsertDepartment] 
        @Name = 'Física',
        @Budget = 200000.00,
        @StartDate = '2020-01-01',
        @Administrator = 4,
        @CreationUser = 1;
END

IF NOT EXISTS (SELECT * FROM [Department] WHERE Name = 'Ingeniería')
BEGIN
    EXEC [SP_InsertDepartment] 
        @Name = 'Ingeniería',
        @Budget = 400000.00,
        @StartDate = '2020-01-01',
        @Administrator = 5,
        @CreationUser = 1;
END

--  mas cursos de prueba
IF NOT EXISTS (SELECT * FROM [Course] WHERE Title = 'Estructuras de Datos')
BEGIN
    DECLARE @CourseId3 INT;
    EXEC @CourseId3 = [SP_InsertCourse] 
        @Title = 'Estructuras de Datos',
        @Credits = 4,
        @DepartmentId = 2,
        @CreationUser = 1;
END

IF NOT EXISTS (SELECT * FROM [Course] WHERE Title = 'Algoritmos')
BEGIN
    DECLARE @CourseId4 INT;
    EXEC @CourseId4 = [SP_InsertCourse] 
        @Title = 'Algoritmos',
        @Credits = 4,
        @DepartmentId = 2,
        @CreationUser = 1;
END

IF NOT EXISTS (SELECT * FROM [Course] WHERE Title = 'Cálculo I')
BEGIN
    DECLARE @CourseId5 INT;
    EXEC @CourseId5 = [SP_InsertCourse] 
        @Title = 'Cálculo I',
        @Credits = 4,
        @DepartmentId = 3,
        @CreationUser = 1;
END

IF NOT EXISTS (SELECT * FROM [Course] WHERE Title = 'Cálculo II')
BEGIN
    DECLARE @CourseId6 INT;
    EXEC @CourseId6 = [SP_InsertCourse] 
        @Title = 'Cálculo II',
        @Credits = 4,
        @DepartmentId = 3,
        @CreationUser = 1;
END

IF NOT EXISTS (SELECT * FROM [Course] WHERE Title = 'Física I')
BEGIN
    DECLARE @CourseId7 INT;
    EXEC @CourseId7 = [SP_InsertCourse] 
        @Title = 'Física I',
        @Credits = 4,
        @DepartmentId = 4,
        @CreationUser = 1;
END

IF NOT EXISTS (SELECT * FROM [Course] WHERE Title = 'Física II')
BEGIN
    DECLARE @CourseId8 INT;
    EXEC @CourseId8 = [SP_InsertCourse] 
        @Title = 'Física II',
        @Credits = 4,
        @DepartmentId = 4,
        @CreationUser = 1;
END

IF NOT EXISTS (SELECT * FROM [Course] WHERE Title = 'Circuitos Eléctricos')
BEGIN
    DECLARE @CourseId9 INT;
    EXEC @CourseId9 = [SP_InsertCourse] 
        @Title = 'Circuitos Eléctricos',
        @Credits = 3,
        @DepartmentId = 5,
        @CreationUser = 1;
END

IF NOT EXISTS (SELECT * FROM [Course] WHERE Title = 'Mecánica de Materiales')
BEGIN
    DECLARE @CourseId10 INT;
    EXEC @CourseId10 = [SP_InsertCourse] 
        @Title = 'Mecánica de Materiales',
        @Credits = 3,
        @DepartmentId = 5,
        @CreationUser = 1;
END

IF NOT EXISTS (SELECT * FROM [Course] WHERE Title = 'Programación Web')
BEGIN
    DECLARE @CourseId11 INT;
    EXEC @CourseId11 = [SP_InsertCourse] 
        @Title = 'Programación Web',
        @Credits = 3,
        @DepartmentId = 2,
        @CreationUser = 1;
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Student]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Student](
        [StudentId] [int] IDENTITY(1,1) NOT NULL,
        [FirstName] [nvarchar](50) NOT NULL,
        [LastName] [nvarchar](50) NOT NULL,
        [Email] [nvarchar](100) NOT NULL,
        [PhoneNumber] [nvarchar](20) NULL,
        [EnrollmentDate] [datetime] NOT NULL,
        [CreationDate] [datetime] NOT NULL,
        [ModifyDate] [datetime] NULL,
        [CreationUser] [int] NOT NULL,
        [UserMod] [int] NULL,
        [UserDeleted] [int] NULL,
        [DeletedDate] [datetime] NULL,
        [Deleted] [bit] NOT NULL DEFAULT(0),
        CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED ([StudentId] ASC)
    );
END
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_InsertStudent]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[SP_InsertStudent]
GO

CREATE PROCEDURE [dbo].[SP_InsertStudent]
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @PhoneNumber NVARCHAR(20) = NULL,
    @EnrollmentDate DATETIME,
    @CreationUser INT
AS
BEGIN
    SET NOCOUNT ON
    
    INSERT INTO [Student] (FirstName, LastName, Email, PhoneNumber, EnrollmentDate, CreationDate, CreationUser, Deleted)
    VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @EnrollmentDate, GETDATE(), @CreationUser, 0)
    
    SELECT SCOPE_IDENTITY() AS StudentId
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_UpdateStudent]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[SP_UpdateStudent]
GO

CREATE PROCEDURE [dbo].[SP_UpdateStudent]
    @StudentId INT,
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @PhoneNumber NVARCHAR(20) = NULL,
    @EnrollmentDate DATETIME,
    @UserMod INT
AS
BEGIN
    SET NOCOUNT ON
    
    UPDATE [Student] 
    SET FirstName = @FirstName,
        LastName = @LastName,
        Email = @Email,
        PhoneNumber = @PhoneNumber,
        EnrollmentDate = @EnrollmentDate,
        ModifyDate = GETDATE(),
        UserMod = @UserMod
    WHERE StudentId = @StudentId AND Deleted = 0
    
    SELECT @@ROWCOUNT AS RowsAffected
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_DeleteStudent]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[SP_DeleteStudent]
GO

CREATE PROCEDURE [dbo].[SP_DeleteStudent]
    @StudentId INT,
    @UserDeleted INT
AS
BEGIN
    SET NOCOUNT ON
    
    UPDATE [Student] 
    SET Deleted = 1,
        DeletedDate = GETDATE(),
        UserDeleted = @UserDeleted
    WHERE StudentId = @StudentId AND Deleted = 0
    
    SELECT @@ROWCOUNT AS RowsAffected
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerEstudiantes]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[ObtenerEstudiantes]
GO

CREATE PROCEDURE [dbo].[ObtenerEstudiantes]
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT 
        StudentId,
        FirstName,
        LastName,
        Email,
        PhoneNumber,
        EnrollmentDate,
        CreationDate,
        FORMAT(EnrollmentDate, 'dd/MM/yyyy') AS EnrollmentDateDisplay,
        FORMAT(CreationDate, 'dd/MM/yyyy HH:mm') AS CreationDateDisplay
    FROM [Student] 
    WHERE Deleted = 0
    ORDER BY LastName, FirstName
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerEstudiantePorId]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[ObtenerEstudiantePorId]
GO

CREATE PROCEDURE [dbo].[ObtenerEstudiantePorId]
    @p_StudentId INT
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT 
        StudentId,
        FirstName,
        LastName,
        Email,
        PhoneNumber,
        EnrollmentDate,
        CreationDate,
        FORMAT(EnrollmentDate, 'dd/MM/yyyy') AS EnrollmentDateDisplay,
        FORMAT(CreationDate, 'dd/MM/yyyy HH:mm') AS CreationDateDisplay
    FROM [Student] 
    WHERE StudentId = @p_StudentId AND Deleted = 0
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AgregarEstudiante]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[AgregarEstudiante]
GO

CREATE PROCEDURE [dbo].[AgregarEstudiante]
    @p_FirstName NVARCHAR(50),
    @p_LastName NVARCHAR(50),
    @p_Email NVARCHAR(100),
    @p_PhoneNumber NVARCHAR(20) = NULL,
    @p_EnrollmentDate DATETIME,
    @p_CreateUser INT,
    @p_result NVARCHAR(1000) OUTPUT,
    @p_StudentId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON
    
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM [Student] WHERE Email = @p_Email AND Deleted = 0)
        BEGIN
            SET @p_result = 'Error: Ya existe un estudiante con este email'
            SET @p_StudentId = 0
            RETURN
        END
        
        INSERT INTO [Student] (FirstName, LastName, Email, PhoneNumber, EnrollmentDate, CreationDate, CreationUser, Deleted)
        VALUES (@p_FirstName, @p_LastName, @p_Email, @p_PhoneNumber, @p_EnrollmentDate, GETDATE(), @p_CreateUser, 0)
        
        SET @p_StudentId = SCOPE_IDENTITY()
        SET @p_result = 'Estudiante creado exitosamente'
    END TRY
    BEGIN CATCH
        SET @p_result = 'Error: ' + ERROR_MESSAGE()
        SET @p_StudentId = 0
    END CATCH
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActualizarEstudiante]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[ActualizarEstudiante]
GO

CREATE PROCEDURE [dbo].[ActualizarEstudiante]
    @StudentId INT,
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @PhoneNumber NVARCHAR(20) = NULL,
    @EnrollmentDate DATETIME,
    @UserMod INT,
    @p_result NVARCHAR(1000) OUTPUT
AS
BEGIN
    SET NOCOUNT ON
    
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM [Student] WHERE Email = @Email AND StudentId != @StudentId AND Deleted = 0)
        BEGIN
            SET @p_result = 'Error: Ya existe otro estudiante con este email'
            RETURN
        END
        
        UPDATE [Student] 
        SET FirstName = @FirstName,
            LastName = @LastName,
            Email = @Email,
            PhoneNumber = @PhoneNumber,
            EnrollmentDate = @EnrollmentDate,
            ModifyDate = GETDATE(),
            UserMod = @UserMod
        WHERE StudentId = @StudentId AND Deleted = 0
        
        IF @@ROWCOUNT > 0
            SET @p_result = 'Estudiante actualizado exitosamente'
        ELSE
            SET @p_result = 'No se encontró el estudiante para actualizar'
    END TRY
    BEGIN CATCH
        SET @p_result = 'Error: ' + ERROR_MESSAGE()
    END CATCH
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EliminarEstudiante]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[EliminarEstudiante]
GO

CREATE PROCEDURE [dbo].[EliminarEstudiante]
    @p_StudentId INT,
    @p_result NVARCHAR(1000) OUTPUT
AS
BEGIN
    SET NOCOUNT ON
    
    BEGIN TRY
        UPDATE [Student] 
        SET Deleted = 1,
            DeletedDate = GETDATE()
        WHERE StudentId = @p_StudentId AND Deleted = 0
        
        IF @@ROWCOUNT > 0
            SET @p_result = 'Estudiante eliminado exitosamente'
        ELSE
            SET @p_result = 'No se encontró el estudiante para eliminar'
    END TRY
    BEGIN CATCH
        SET @p_result = 'Error: ' + ERROR_MESSAGE()
    END CATCH
END
GO

IF NOT EXISTS (SELECT * FROM [Student] WHERE Email = 'juan.perez@estudiante.com')
BEGIN
    EXEC [SP_InsertStudent] 
        @FirstName = 'Juan',
        @LastName = 'Pérez',
        @Email = 'juan.perez@estudiante.com',
        @PhoneNumber = '809-555-0101',
        @EnrollmentDate = '2024-01-15',
        @CreationUser = 1
END

IF NOT EXISTS (SELECT * FROM [Student] WHERE Email = 'maria.garcia@estudiante.com')
BEGIN
    EXEC [SP_InsertStudent] 
        @FirstName = 'María',
        @LastName = 'García',
        @Email = 'maria.garcia@estudiante.com',
        @PhoneNumber = '809-555-0102',
        @EnrollmentDate = '2024-01-20',
        @CreationUser = 1
END

IF NOT EXISTS (SELECT * FROM [Student] WHERE Email = 'carlos.lopez@estudiante.com')
BEGIN
    EXEC [SP_InsertStudent] 
        @FirstName = 'Carlos',
        @LastName = 'López',
        @Email = 'carlos.lopez@estudiante.com',
        @PhoneNumber = '809-555-0103',
        @EnrollmentDate = '2024-02-01',
        @CreationUser = 1
END

IF NOT EXISTS (SELECT * FROM [Student] WHERE Email = 'ana.martinez@estudiante.com')
BEGIN
    EXEC [SP_InsertStudent] 
        @FirstName = 'Ana',
        @LastName = 'Martínez',
        @Email = 'ana.martinez@estudiante.com',
        @PhoneNumber = '809-555-0104',
        @EnrollmentDate = '2024-02-10',
        @CreationUser = 1
END

IF NOT EXISTS (SELECT * FROM [Student] WHERE Email = 'pedro.rodriguez@estudiante.com')
BEGIN
    EXEC [SP_InsertStudent] 
        @FirstName = 'Pedro',
        @LastName = 'Rodriguez',
        @Email = 'pedro.rodriguez@estudiante.com',
        @PhoneNumber = '809-555-0105',
        @EnrollmentDate = '2024-02-15',
        @CreationUser = 1
END

IF NOT EXISTS (SELECT * FROM [Student] WHERE Email = 'lucia.fernandez@estudiante.com')
BEGIN
    EXEC [SP_InsertStudent] 
        @FirstName = 'Lucia',
        @LastName = 'Fernandez',
        @Email = 'lucia.fernandez@estudiante.com',
        @PhoneNumber = '809-555-0106',
        @EnrollmentDate = '2024-02-20',
        @CreationUser = 1
END

IF NOT EXISTS (SELECT * FROM [Student] WHERE Email = 'diego.sanchez@estudiante.com')
BEGIN
    EXEC [SP_InsertStudent] 
        @FirstName = 'Diego',
        @LastName = 'Sanchez',
        @Email = 'diego.sanchez@estudiante.com',
        @PhoneNumber = '809-555-0107',
        @EnrollmentDate = '2024-03-01',
        @CreationUser = 1
END

IF NOT EXISTS (SELECT * FROM [Student] WHERE Email = 'sofia.torres@estudiante.com')
BEGIN
    EXEC [SP_InsertStudent] 
        @FirstName = 'Sofia',
        @LastName = 'Torres',
        @Email = 'sofia.torres@estudiante.com',
        @PhoneNumber = '809-555-0108',
        @EnrollmentDate = '2024-03-05',
        @CreationUser = 1
END

IF NOT EXISTS (SELECT * FROM [Student] WHERE Email = 'miguel.ramirez@estudiante.com')
BEGIN
    EXEC [SP_InsertStudent] 
        @FirstName = 'Miguel',
        @LastName = 'Ramirez',
        @Email = 'miguel.ramirez@estudiante.com',
        @PhoneNumber = '809-555-0109',
        @EnrollmentDate = '2024-03-10',
        @CreationUser = 1
END

IF NOT EXISTS (SELECT * FROM [Student] WHERE Email = 'valentina.morales@estudiante.com')
BEGIN
    EXEC [SP_InsertStudent] 
        @FirstName = 'Valentina',
        @LastName = 'Morales',
        @Email = 'valentina.morales@estudiante.com',
        @PhoneNumber = '809-555-0110',
        @EnrollmentDate = '2024-03-15',
        @CreationUser = 1
END

IF NOT EXISTS (SELECT * FROM [Student] WHERE Email = 'alejandro.jimenez@estudiante.com')
BEGIN
    EXEC [SP_InsertStudent] 
        @FirstName = 'Alejandro',
        @LastName = 'Jimenez',
        @Email = 'alejandro.jimenez@estudiante.com',
        @PhoneNumber = '809-555-0111',
        @EnrollmentDate = '2024-03-20',
        @CreationUser = 1
END

IF NOT EXISTS (SELECT * FROM [Student] WHERE Email = 'camila.herrera@estudiante.com')
BEGIN
    EXEC [SP_InsertStudent] 
        @FirstName = 'Camila',
        @LastName = 'Herrera',
        @Email = 'camila.herrera@estudiante.com',
        @PhoneNumber = '809-555-0112',
        @EnrollmentDate = '2024-03-25',
        @CreationUser = 1
END

IF NOT EXISTS (SELECT * FROM [Student] WHERE Email = 'sebastian.castro@estudiante.com')
BEGIN
    EXEC [SP_InsertStudent] 
        @FirstName = 'Sebastian',
        @LastName = 'Castro',
        @Email = 'sebastian.castro@estudiante.com',
        @PhoneNumber = '809-555-0113',
        @EnrollmentDate = '2024-04-01',
        @CreationUser = 1
END

IF NOT EXISTS (SELECT * FROM [Student] WHERE Email = 'isabella.ruiz@estudiante.com')
BEGIN
    EXEC [SP_InsertStudent] 
        @FirstName = 'Isabella',
        @LastName = 'Ruiz',
        @Email = 'isabella.ruiz@estudiante.com',
        @PhoneNumber = '809-555-0114',
        @EnrollmentDate = '2024-04-05',
        @CreationUser = 1
END

IF NOT EXISTS (SELECT * FROM [Student] WHERE Email = 'mateo.vargas@estudiante.com')
BEGIN
    EXEC [SP_InsertStudent] 
        @FirstName = 'Mateo',
        @LastName = 'Vargas',
        @Email = 'mateo.vargas@estudiante.com',
        @PhoneNumber = '809-555-0115',
        @EnrollmentDate = '2024-04-10',
        @CreationUser = 1
END

IF NOT EXISTS (SELECT * FROM [Student] WHERE Email = 'emma.gutierrez@estudiante.com')
BEGIN
    EXEC [SP_InsertStudent] 
        @FirstName = 'Emma',
        @LastName = 'Gutierrez',
        @Email = 'emma.gutierrez@estudiante.com',
        @PhoneNumber = '809-555-0116',
        @EnrollmentDate = '2024-04-15',
        @CreationUser = 1
END

IF NOT EXISTS (SELECT * FROM [Student] WHERE Email = 'santiago.mendoza@estudiante.com')
BEGIN
    EXEC [SP_InsertStudent] 
        @FirstName = 'Santiago',
        @LastName = 'Mendoza',
        @Email = 'santiago.mendoza@estudiante.com',
        @PhoneNumber = '809-555-0117',
        @EnrollmentDate = '2024-04-20',
        @CreationUser = 1
END

IF NOT EXISTS (SELECT * FROM [Student] WHERE Email = 'olivia.aguilar@estudiante.com')
BEGIN
    EXEC [SP_InsertStudent] 
        @FirstName = 'Olivia',
        @LastName = 'Aguilar',
        @Email = 'olivia.aguilar@estudiante.com',
        @PhoneNumber = '809-555-0118',
        @EnrollmentDate = '2024-04-25',
        @CreationUser = 1
END

IF NOT EXISTS (SELECT * FROM [Student] WHERE Email = 'david.silva@estudiante.com')
BEGIN
    EXEC [SP_InsertStudent] 
        @FirstName = 'David',
        @LastName = 'Silva',
        @Email = 'david.silva@estudiante.com',
        @PhoneNumber = '809-555-0119',
        @EnrollmentDate = '2024-05-01',
        @CreationUser = 1
END

GO
