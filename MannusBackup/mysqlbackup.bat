@ECHO off
cls
set DBchoice=%1%
set User=%2%
set Password=%3%
set pathchoice=%4%
set Host=%5%

@REM Remove double quotes from the path
@REM SET pathchoice=%pathchoice:"=%
@REM SET pathchoice=%pathchoice:"=%

mysqldump --opt --databases %DBchoice% --host %Host% --user %User% --password=%Password%  > %pathchoice%