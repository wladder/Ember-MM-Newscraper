﻿' ################################################################################
' #                             EMBER MEDIA MANAGER                              #
' ################################################################################
' ################################################################################
' # This file is part of Ember Media Manager.                                    #
' #                                                                              #
' # Ember Media Manager is free software: you can redistribute it and/or modify  #
' # it under the terms of the GNU General Public License as published by         #
' # the Free Software Foundation, either version 3 of the License, or            #
' # (at your option) any later version.                                          #
' #                                                                              #
' # Ember Media Manager is distributed in the hope that it will be useful,       #
' # but WITHOUT ANY WARRANTY; without even the implied warranty of               #
' # MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                #
' # GNU General Public License for more details.                                 #
' #                                                                              #
' # You should have received a copy of the GNU General Public License            #
' # along with Ember Media Manager.  If not, see <http://www.gnu.org/licenses/>. #
' ###############################################################################

Imports EmberAPI
Imports NLog

Public Class Addon
    Implements Interfaces.IScraperModule_Trailer_Movie


#Region "Fields"

    Shared _Logger As Logger = LogManager.GetCurrentClassLogger()

    Private _AddonSettings As New AddonSettings
    Public Shared _ConfigModifier As New Structures.ScrapeModifiers
    Private _PnlSettingsPanel As frmSettingsPanel

#End Region 'Fields

#Region "Properties"

    Property IsEnabled() As Boolean Implements Interfaces.IScraperModule_Trailer_Movie.IsEnabled

    Property Order As Integer Implements Interfaces.IScraperModule_Trailer_Movie.Order

    Property SettingsPanel As Containers.SettingsPanel = Nothing Implements Interfaces.IScraperModule_Trailer_Movie.SettingsPanel

#End Region 'Properties

#Region "Events"

    Public Event NeedsRestart() Implements Interfaces.IScraperModule_Trailer_Movie.NeedsRestart
    Public Event ScraperEvent(ByVal eType As Enums.ScraperEventType, ByVal Parameter As Object) Implements Interfaces.IScraperModule_Trailer_Movie.ScraperEvent
    Public Event SettingsChanged() Implements Interfaces.IScraperModule_Trailer_Movie.SettingsChanged
    Public Event StateChanged(ByVal SettingsPanelID As String, ByVal State As Boolean, ByVal DiffOrder As Integer) Implements Interfaces.IScraperModule_Trailer_Movie.StateChanged

#End Region 'Events

#Region "Event Methods"

    Private Sub Handle_NeedsRestart()
        RaiseEvent NeedsRestart()
    End Sub

    Private Sub Handle_SettingsChanged()
        RaiseEvent SettingsChanged()
    End Sub

    Private Sub Handle_StateChanged(ByVal State As Boolean, ByVal DiffOrder As Integer)
        IsEnabled = State
        RaiseEvent StateChanged(SettingsPanel.SettingsPanelID, State, DiffOrder)
    End Sub

#End Region 'Event Methods

#Region "Interface Methods"

    Sub Init() Implements Interfaces.IScraperModule_Trailer_Movie.Init
        Settings_Load()
    End Sub

    Public Sub InjectSettingsPanel() Implements Interfaces.IScraperModule_Trailer_Movie.InjectSettingsPanel
        Settings_Load()
        _PnlSettingsPanel = New frmSettingsPanel
        _PnlSettingsPanel.chkEnabled.Checked = IsEnabled

        AddHandler _PnlSettingsPanel.NeedsRestart, AddressOf Handle_NeedsRestart
        AddHandler _PnlSettingsPanel.SettingsChanged, AddressOf Handle_SettingsChanged
        AddHandler _PnlSettingsPanel.StateChanged, AddressOf Handle_StateChanged

        SettingsPanel = New Containers.SettingsPanel With {
            .ImageIndex = If(IsEnabled, 9, 10),
            .Panel = _PnlSettingsPanel.pnlSettings,
            .Title = "VideoBuster.de",
            .Type = Enums.SettingsPanelType.MovieTrailer
        }
    End Sub

    Public Sub OrderChanged(ByVal OrderState As Containers.SettingsPanel.OrderState) Implements Interfaces.IScraperModule_Trailer_Movie.OrderChanged
        _PnlSettingsPanel.orderChanged(OrderState)
    End Sub

    Function Run(ByRef DBMovie As Database.DBElement, ByVal Type As Enums.ModifierType, ByRef TrailerList As List(Of MediaContainers.Trailer)) As Interfaces.ModuleResult Implements Interfaces.IScraperModule_Trailer_Movie.Run
        _Logger.Trace("[VideobusterDE_Trailer] [Scraper_Movie] [Start]")

        Settings_Load()

        Dim strTitle As String = String.Empty
        If Not String.IsNullOrEmpty(DBMovie.Movie.Title) Then
            strTitle = DBMovie.Movie.Title
        ElseIf Not String.IsNullOrEmpty(DBMovie.Movie.OriginalTitle) Then
            strTitle = DBMovie.Movie.OriginalTitle
        End If

        If Not String.IsNullOrEmpty(strTitle) Then
            TrailerList = Scraper.GetTrailers(strTitle)
        End If

        _Logger.Trace("[VideobusterDE_Trailer] [Scraper_Movie] [Done]")
        Return New Interfaces.ModuleResult With {.breakChain = False}
    End Function

    Public Sub SaveSetup(ByVal DoDispose As Boolean) Implements Interfaces.IScraperModule_Trailer_Movie.SaveSetup
        Settings_Save()
        If DoDispose Then
            RemoveHandler _PnlSettingsPanel.NeedsRestart, AddressOf Handle_NeedsRestart
            RemoveHandler _PnlSettingsPanel.SettingsChanged, AddressOf Handle_SettingsChanged
            RemoveHandler _PnlSettingsPanel.StateChanged, AddressOf Handle_StateChanged
            _PnlSettingsPanel.Dispose()
        End If
    End Sub

#End Region 'Interface Methods

#Region "Methods"

    Sub Settings_Load()
        _ConfigModifier.MainTrailer = AdvancedSettings.GetBooleanSetting("DoTrailer", True, , Enums.ContentType.Movie)
    End Sub

    Sub Settings_Save()
        Using settings = New AdvancedSettings()
            settings.SetBooleanSetting("DoTrailer", _ConfigModifier.MainTrailer, , , Enums.ContentType.Movie)
        End Using
    End Sub

#End Region 'Methods

#Region "Nested Types"

    Structure AddonSettings

#Region "Fields"

        Dim PrefLanguage As String

#End Region 'Fields

    End Structure

#End Region 'Nested Types

End Class