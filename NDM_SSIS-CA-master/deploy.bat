ECHO Off
ECHO Starting deployment

:: Prompt for folder path defaults
SET NDM_SSIS_SOLUTION_FOLDER=c:\vs15\NDM_SSIS-master
SET /P NDM_SSIS_SOLUTION_FOLDER="NDM_SSIS Solution Folder [%NDM_SSIS_SOLUTION_FOLDER%] ?"

SET NDM_SSIS_DTS_SOLUTION_FOLDER=c:\vs15\NDM_SSIS-CA-master
SET /P NDM_SSIS_DTS_SOLUTION_FOLDER="NDM_SSIS DTS Solution Folder [%NDM_SSIS_DTS_SOLUTION_FOLDER%] ?"

SET DEPLOY_ROOT_FOLDER=.\deployment
SET /P DEPLOY_ROOT_FOLDER="Deployment Folder [%DEPLOY_ROOT_FOLDER%] ?"

:: Set variables SOURCE 
SET SOURCE_PACKAGES=%NDM_SSIS_SOLUTION_FOLDER%\NDM_SSIS\bin\Deployment\*
SET SOURCE_SQLSCRIPTS=%NDM_SSIS_SOLUTION_FOLDER%\SqlScripts\*
SET SOURCE_LAUNCHER=%NDM_SSIS_DTS_SOLUTION_FOLDER%\NDM_SSIS-CA-master\bin\Release\*

:: Set variables DESTINATION 
SET DEPLOY_PCK_FOLDER=%DEPLOY_ROOT_FOLDER%\Packages\
SET DEPLOY_SQL_FOLDER=%DEPLOY_ROOT_FOLDER%\SqlScripts\
SET DEPLOY_LAUNCHER_FOLDER=%DEPLOY_ROOT_FOLDER%

SET DEPLOY__SPLITTING_FOLDER=%DEPLOY_ROOT_FOLDER%\_Splitting
SET DEPLOY__IMPORT_FOLDER=%DEPLOY_ROOT_FOLDER%\_Import
SET DEPLOY__STAGING_FOLDER=%DEPLOY_ROOT_FOLDER%\_Staging
SET DEPLOY__ARCHIVE_FOLDER=%DEPLOY_ROOT_FOLDER%\_Archive
SET DEPLOY__ERROR_FOLDER=%DEPLOY_ROOT_FOLDER%\_Error

:: Start deploying files
IF EXIST %DEPLOY_ROOT_FOLDER% (
	ECHO Deleting old deployment...
	RMDIR %DEPLOY_ROOT_FOLDER% /S /Q
)

ECHO Creating data transfer (empty) directories...
MKDIR %DEPLOY__SPLITTING_FOLDER%
MKDIR %DEPLOY__IMPORT_FOLDER%
MKDIR %DEPLOY__STAGING_FOLDER%
MKDIR %DEPLOY__ARCHIVE_FOLDER%
MKDIR %DEPLOY__ERROR_FOLDER%

MKDIR %DEPLOY_PCK_FOLDER%
MKDIR %DEPLOY_DTS_FOLDER%

ECHO Copying DTS packages and files
xcopy %SOURCE_PACKAGES% %DEPLOY_PCK_FOLDER% > nul
xcopy %SOURCE_SQLSCRIPTS% %DEPLOY_SQL_FOLDER% > nul

ECHO Copying DTS launcher
xcopy %SOURCE_LAUNCHER% %DEPLOY_LAUNCHER_FOLDER% > nul

ECHO *** Remeber to update .dtsConfig files in [%DEPLOY_PCK_FOLDER%] with 
ECHO Well all looks great!

