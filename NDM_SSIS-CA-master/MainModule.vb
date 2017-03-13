Imports System.IO
Imports Microsoft.SqlServer.Dts.Runtime

Module MainModule

  Private Const _folderpath_packages As String = "\Packages\"
  Private Dim _rootpath As String
  Private Dim _packages As Queue(Of String)


  Sub Main()

    SetUp()
    ShowStartBanner() 
    ExecutePackages()
    ShowEndBanner()
            
  End Sub

  Sub SetUp()
    _rootpath = GetCurrentDirectory()
    ' Packages are located in _folderpath_packages
    _packages = New Queue(Of String)()    
    _packages.Enqueue("splitter.dtsx")
    _packages.Enqueue("import_ff_nca_m501.dtsx")
    _packages.Enqueue("import_ff_nca_m221.dtsx")
  End Sub

  Sub ExecutePackages()
    Try
        For Each pkgFilename As String In _packages
            ExecutePackage(_folderpath_packages + pkgFilename)
        Next
    Catch ex As Exception
         ShowErrorBanner(ex)
    End Try

  End Sub


  Function GetCurrentDirectory()
    GetCurrentDirectory = Directory.GetCurrentDirectory()
    Console.WriteLine("Root folder '{0}'", GetCurrentDirectory)
  End Function

  Sub ExecutePackage(pkgLocation As String)
    Dim pkg As Package
    Dim pkgFilename As String
    Dim pkgResults As DTSExecResult
    Dim app As New Application
    Dim eventListener As New EventListener()

    pkgFilename = _rootpath + pkgLocation 
    Console.WriteLine(VbCrLf & "Loading package '{0}'...", pkgFilename) 
    pkg = app.LoadPackage(pkgFilename, eventListener)
    RelocateConfigurations(pkg)
    Console.WriteLine("Executing package '{0}...'", pkgFilename) 
    pkgResults = pkg.Execute(Nothing, Nothing, eventListener, Nothing, Nothing)
  End Sub

    Sub RelocateConfigurations(ByRef pkg As Package)
        Dim configs As Configurations
        Dim oldconfigstring, newconfigstring As String
        configs = pkg.Configurations
        For Each config In configs
            oldconfigstring = config.ConfigurationString
            newconfigstring = _rootpath + _folderpath_packages + FileNameFromPath(oldconfigstring)
            config.ConfigurationString = newconfigstring
            Console.WriteLine("Loading config file '{0}...", newconfigstring) 
        Next
    End Sub


  Function FolderFromPath(ByRef strFullPath As String) As String
    FolderFromPath = Left(strFullPath, InStrRev(strFullPath, "\"))
  End Function

  Function FileNameFromPath(strFullPath As String) As String 
        FileNameFromPath = Right(strFullPath, Len(strFullPath) - InStrRev(strFullPath, "\"))
  End Function

  Sub ShowStartBanner()
    Console.WriteLine("===========================================================================")
  End Sub

  Sub ShowErrorBanner(ex As Exception)
    Console.WriteLine("Error: '{0}'", ex.ToString())
  End Sub

  Sub ShowEndBanner()
    ShowStartBanner()
    Console.WriteLine("See you!") 
  End Sub

End Module

Class EventListener
    Inherits DefaultEvents

  Public Overrides Function OnError(ByVal source As Microsoft.SqlServer.Dts.Runtime.DtsObject, _
    ByVal errorCode As Integer, ByVal subComponent As String, ByVal description As String, _
    ByVal helpFile As String, ByVal helpContext As Integer, _
    ByVal idofInterfaceWithError As String) As Boolean

    ' Add application–specific diagnostics here.
    Console.WriteLine()
    Console.WriteLine("Error in {0}/{1} : {2}", source, subComponent, description)
    Return False

  End Function

End Class
