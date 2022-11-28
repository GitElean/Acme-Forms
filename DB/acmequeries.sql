


CREATE database DB_Acme

USE DB_Acme


CREATE TABLE usuario(
	ID_user int primary key identity(1,1),
	email varchar (50),
	pass varchar(70)
)


CREATE PROC register_SingUpUser(
@email varchar(50),
@pass varchar(70),
@signed bit output,
@message varchar(200) output
)
AS
BEGIN
	if(not exists(SELECT * FROM usuario WHERE email = @email))
	BEGIN
		INSERT INTO usuario(email, pass) VALUES (@email, @pass)
		SET @signed = 1
		set @message = 'El usuario ha sido registro de manera satisfactoria'
	END
	else
	BEGIN
		SET @signed = 0
		SET @message = 'El correo ya posee una cuenta, intente con uno distinto o inicia sesión'
	end
end

CREATE PROC validateUser(
@email varchar (50),
@pass varchar (100)
)
AS
BEGIN
	if(exists(SELECT * FROM usuario where email = @email and pass = @pass))
		SELECT ID_user FROM usuario where email = @email and pass = @pass
	else
		SELECT '0'
END

DECLARE @signed bit, @message varchar(200)

EXEC register_SingUpUser 'example@gmail.com', 'defaultpassword',@signed output, @message output 

SELECT @signed
SELECT @message

EXEC validateUser 'exampl1e@gmail.com','defaultpassword'

