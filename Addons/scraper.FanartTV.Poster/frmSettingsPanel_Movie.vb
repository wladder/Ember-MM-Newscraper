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
' ################################################################################

Imports EmberAPI

Public Class frmSettingsPanel_Movie

#Region "Events"

    Public Event NeedsRestart()
    Public Event SettingsChanged()
    Public Event StateChanged(ByVal State As Boolean, ByVal DiffOrder As Integer)

#End Region 'Events  

#Region "Methods"

    Public Sub New()
        InitializeComponent()
        Setup()
    End Sub

    Private Sub btnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDown.Click
        RaiseEvent StateChanged(chkEnabled.Checked, 1)
    End Sub

    Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp.Click
        RaiseEvent StateChanged(chkEnabled.Checked, -1)
    End Sub

    Private Sub btnUnlockAPI_Click(sender As Object, e As EventArgs) Handles btnUnlockAPI.Click
        If btnUnlockAPI.Text = Master.eLang.GetString(1188, "Use my own API key") Then
            btnUnlockAPI.Text = Master.eLang.GetString(443, "Use embedded API Key")
            lblEMMAPI.Visible = False
            txtApiKey.Enabled = True
        Else
            btnUnlockAPI.Text = Master.eLang.GetString(1188, "Use my own API key")
            lblEMMAPI.Visible = True
            txtApiKey.Enabled = False
            txtApiKey.Text = String.Empty
        End If
    End Sub

    Private Sub cbEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnabled.CheckedChanged
        RaiseEvent StateChanged(chkEnabled.Checked, 0)
    End Sub

    Private Sub chkScrapeBanner_CheckedChanged(sender As Object, e As EventArgs) Handles chkScrapeBanner.CheckedChanged
        RaiseEvent SettingsChanged()
    End Sub

    Private Sub chkScrapeClearArt_CheckedChanged(sender As Object, e As EventArgs) Handles chkScrapeClearArt.CheckedChanged
        RaiseEvent SettingsChanged()
    End Sub

    Private Sub chkScrapeClearLogo_CheckedChanged(sender As Object, e As EventArgs) Handles chkScrapeClearLogo.CheckedChanged
        RaiseEvent SettingsChanged()
    End Sub

    Private Sub chkScrapeDiscArt_CheckedChanged(sender As Object, e As EventArgs) Handles chkScrapeDiscArt.CheckedChanged
        RaiseEvent SettingsChanged()
    End Sub

    Private Sub chkScrapeFanart_CheckedChanged(sender As Object, e As EventArgs) Handles chkScrapeFanart.CheckedChanged
        RaiseEvent SettingsChanged()
    End Sub

    Private Sub chkScrapeLandscape_CheckedChanged(sender As Object, e As EventArgs) Handles chkScrapeLandscape.CheckedChanged
        RaiseEvent SettingsChanged()
    End Sub

    Private Sub chkScrapePoster_CheckedChanged(sender As Object, e As EventArgs) Handles chkScrapePoster.CheckedChanged
        RaiseEvent SettingsChanged()
    End Sub

    Public Sub OrderChanged(ByVal OrderState As Containers.SettingsPanel.OrderState)
        btnDown.Enabled = OrderState.Position < OrderState.TotalCount - 1
        btnUp.Enabled = OrderState.Position > 0
    End Sub

    Private Sub pbApiKeyInfo_Click(sender As System.Object, e As System.EventArgs) Handles pbApiKeyInfo.Click
        Functions.Launch(My.Resources.urlAPIKey)
    End Sub

    Sub Setup()
        btnUnlockAPI.Text = Master.eLang.GetString(1188, "Use my own API key")
        chkEnabled.Text = Master.eLang.GetString(774, "Enabled")
        chkScrapeBanner.Text = Master.eLang.GetString(838, "Banner")
        chkScrapeClearArt.Text = Master.eLang.GetString(1096, "ClearArt")
        chkScrapeClearLogo.Text = Master.eLang.GetString(1097, "ClearLogo")
        chkScrapeDiscArt.Text = Master.eLang.GetString(1098, "DiscArt")
        chkScrapeFanart.Text = Master.eLang.GetString(149, "Fanart")
        chkScrapeLandscape.Text = Master.eLang.GetString(1035, "Landscape")
        chkScrapePoster.Text = Master.eLang.GetString(148, "Poster")
        gbScraperImagesOpts.Text = Master.eLang.GetString(268, "Images - Scraper specific")
        gbScraperOpts.Text = Master.eLang.GetString(1186, "Scraper Options")
        lblAPIHint.Text = Master.eLang.GetString(1248, "Using a Personal API Key reduces the time you have to wait for new images to show up from 7 days to 48 hours.")
        lblAPIKey.Text = String.Concat(Master.eLang.GetString(789, "Fanart.tv Personal API Key"), ":")
        lblEMMAPI.Text = Master.eLang.GetString(1189, "Ember Media Manager Embedded API Key")
        lblInfoBottom.Text = String.Format(Master.eLang.GetString(790, "These settings are specific to this module.{0}Please refer to the global settings for more options."), Environment.NewLine)
        lblScraperOrder.Text = Master.eLang.GetString(168, "Scrape Order")
    End Sub

    Private Sub txtApiKey_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtApiKey.TextChanged
        RaiseEvent SettingsChanged()
    End Sub

#End Region 'Methods

End Class