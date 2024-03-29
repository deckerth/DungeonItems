﻿Imports DungeonItems.Commands
Imports DungeonItems.Model
Imports DungeonItems.Repository
Imports DungeonItems.Views
Imports Windows.Storage
Imports Windows.Storage.Pickers
Imports Windows.Storage.Provider
Imports Windows.UI.Popups

Namespace Global.DungeonItems.ViewModels

    Public Class ItemsViewModel
        Inherits BindableBase

        Private Items As New ObservableCollection(Of ItemViewModel)
        Public Property GroupedItems As New TypeGroups

        Private Shared _current As ItemsViewModel
        Public Shared ReadOnly Property Current As ItemsViewModel
            Get
                If _current Is Nothing Then
                    _current = New ItemsViewModel
                End If
                Return _current
            End Get
        End Property

        Public Property AddMeleeCommand As RelayCommand
        Public Property AddArtilleryCommand As RelayCommand
        Public Property AddArmorCommand As RelayCommand
        Public Property DeleteCommand As RelayCommand
        Public Property HomeCommand As RelayCommand
        Public Property NavigateToPerksCommand As RelayCommand
        Public Property NavigateToEnchantmentsCommand As RelayCommand
        Public Property ExportCommand As RelayCommand
        Public Property ImportCommand As RelayCommand

        Public Property DetailFrame As Frame
        Public Property RootFrame As Frame

        Public Property TypeFilter As New TypeFilterViewModel

        Private _isInEdit As Boolean = False
        Public Property IsInEdit As Boolean
            Get
                Return _isInEdit
            End Get
            Set(value As Boolean)
                If value <> _isInEdit Then
                    SetProperty(Of Boolean)(_isInEdit, value, "IsInEdit")
                    If Selected IsNot Nothing Then
                        Selected.IsInEdit = _isInEdit
                        Selected.Save(Repository)
                    End If
                End If
            End Set
        End Property

        Private _selected As ItemViewModel
        Public Property Selected As ItemViewModel
            Get
                Return _selected
            End Get
            Set(value As ItemViewModel)
                If value Is Nothing AndAlso _selected Is Nothing Then
                    Return
                End If
                If value Is Nothing OrElse Not value.Equals(_selected) Then
                    If _selected IsNot Nothing AndAlso _selected.Modified Then
                        _selected.Save(Repository)
                    End If
                    _selected = value
                    If _selected IsNot Nothing Then
                        _selected.IsInEdit = _isInEdit
                        Navigate()
                    End If
                    OnPropertyChanged("Selected")
                End If
            End Set
        End Property

        Public Sub Save()
            For Each item In Items
                If item.Modified Then
                    item.Save(Repository)
                End If
            Next
        End Sub

        Private Repository As ItemRepository

        Private Sub New()
            Repository = ItemRepository.Current
            Repository.Load()

            For Each i In Repository.Items
                Select Case i.Type
                    Case Item.ItemType.Artillery
                        Me.Items.Add(New ArtilleryViewModel(i))
                    Case Item.ItemType.Melee
                        Me.Items.Add(New MeleeViewModel(i))
                    Case Item.ItemType.Armor
                        Me.Items.Add(New ItemViewModel(i))
                End Select
            Next

            AddHandler TypeFilter.UpdateFilter, AddressOf HandleUpdateFilter
            HandleUpdateFilter()

            AddMeleeCommand = New RelayCommand(AddressOf DoAddMelee)
            AddArtilleryCommand = New RelayCommand(AddressOf DoAddArtillery)
            AddArmorCommand = New RelayCommand(AddressOf DoAddArmor)
            DeleteCommand = New RelayCommand(AddressOf DoDelete)
            HomeCommand = New RelayCommand(AddressOf displayHomePage)
            NavigateToPerksCommand = New RelayCommand(AddressOf NavigateToPerksPage)
            NavigateToEnchantmentsCommand = New RelayCommand(AddressOf NavigateToEnchantmentsPage)
#Disable Warning BC42359 ' Die von dieser Async-Funktion zurückgegebene Aufgabe wird verworfen, und alle darin enthaltenen Ausnahmen werden ignoriert.
            ExportCommand = New RelayCommand(AddressOf OnExportDB)
            ImportCommand = New RelayCommand(AddressOf OnImportDB)
#Enable Warning BC42359 ' Die von dieser Async-Funktion zurückgegebene Aufgabe wird verworfen, und alle darin enthaltenen Ausnahmen werden ignoriert.

        End Sub

        Public Sub Navigate()
            If DetailFrame IsNot Nothing Then
                If Selected IsNot Nothing Then
                    Select Case Selected.Type
                        Case Item.ItemType.Artillery
                            DetailFrame.Navigate(GetType(ArtilleryPage), Selected)
                        Case Item.ItemType.Melee
                            DetailFrame.Navigate(GetType(MeleePage), Selected)
                        Case Item.ItemType.Armor
                            DetailFrame.Navigate(GetType(ArmorPage), Selected)
                    End Select
                Else
                    DetailFrame.Navigate(GetType(BlankPage))
                End If
            End If
        End Sub

        Private Sub DoAddMelee()
            DoAdd(Item.ItemType.Melee)
        End Sub

        Private Sub DoAddArtillery()
            DoAdd(Item.ItemType.Artillery)
        End Sub

        Private Sub DoAddArmor()
            DoAdd(Item.ItemType.Armor)
        End Sub

        Private Sub DoAdd(type As Item.ItemType)
            Dim newItem = ItemFactory.CreateItem(type)
            Dim itemVM = ItemViewModel.Create(newItem)
            itemVM.Modified = True
            Items.Add(itemVM)
            GroupedItems.Add(itemVM)
            Selected = itemVM
            IsInEdit = True
        End Sub

        Private Sub DoDelete()
            If Selected IsNot Nothing Then
                Selected.Delete(Repository)
                Items.Remove(Selected)
                GroupedItems.Delete(Selected)
                DetailFrame.Navigate(GetType(BlankPage))
            End If
        End Sub

        Private Sub displayHomePage()
            If Selected IsNot Nothing Then
                Selected = Nothing
                DetailFrame.Navigate(GetType(BlankPage))
            End If
        End Sub

        Private Sub NavigateToPerksPage()
            RootFrame.Navigate(GetType(PerksEditorPage), PerksViewModel.Current)
        End Sub

        Private Sub NavigateToEnchantmentsPage()
            RootFrame.Navigate(GetType(EnchantmentsEditorPage), EnchantmentsViewModel.Current)
        End Sub

        Private Sub HandleUpdateFilter()
            GroupedItems.ApplyFilter(TypeFilter, Items)
        End Sub

        Public Async Function OnExportDB() As Task

            Dim savepicker As New FileSavePicker With {
            .SuggestedStartLocation = PickerLocationId.DocumentsLibrary
        }

            ' Dropdown of file types the user can save the file as
            savepicker.FileTypeChoices.Add("Excel Workbook", New List(Of String) From {".xlsx"})
            ' Default file name if the user does Not type one in Or select a file to replace
            savepicker.SuggestedFileName = "MyDungeonItems"

            Dim File As StorageFile = Await savepicker.PickSaveFileAsync()
            If File IsNot Nothing Then
                Using stream = Await File.OpenStreamForWriteAsync()
                    stream.SetLength(0)
                    ' Prevent updates to the remote version of the file until we finish making changes And call CompleteUpdatesAsync.
                    CachedFileManager.DeferUpdates(File)
                    Dim service = New ImportExportService
                    service.Export(stream)

                    Dim status As FileUpdateStatus = Await CachedFileManager.CompleteUpdatesAsync(File)
                    If status = FileUpdateStatus.Complete Then
                        Await New MessageDialog("Die Datenbank wurde erfolgreich exportiert.").ShowAsync()
                    Else
                        Await New MessageDialog("Die Datenbank konnte nicht exportiert werden.").ShowAsync()
                    End If
                End Using
            End If
        End Function

        Private ImportDecisionDialogResult As ImportExportService.ImportOptions
        Private Cancelled As Boolean

        Private Async Function ImportDialog() As Task(Of Boolean)

            Dim dialog = New MessageDialog("Möchten Sie die importierten Medien der Sammlung hinzufügen, oder möchten Sie die Sammlung ersetzen?")

            ' Add commands and set their callbacks 
            dialog.Commands.Add(New UICommand("Hinzufügen", Sub(command) ImportDecisionDialogResult = ImportExportService.ImportOptions.Add))
            dialog.Commands.Add(New UICommand("Ersetzen", Sub(command) ImportDecisionDialogResult = ImportExportService.ImportOptions.Replace))
            dialog.Commands.Add(New UICommand("Abbrechen", Sub(command) Cancelled = True))

            Cancelled = False
            Await dialog.ShowAsync()
            Return Not Cancelled
        End Function

        Public Async Function OnImportDB() As Task

            If Await ImportDialog() Then
                Dim openPicker As FileOpenPicker = New FileOpenPicker()
                openPicker.ViewMode = PickerViewMode.Thumbnail
                openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary
                openPicker.FileTypeFilter.Add(".xlsx")
                Dim File As StorageFile = Await openPicker.PickSingleFileAsync()
                Dim Counters As UpdateCounters
                Dim service = New ImportExportService
                If File IsNot Nothing Then
                    'Await Progress.SetIndeterministicAsync()
                    Counters = service.Import(Await File.OpenStreamForReadAsync(), ImportDecisionDialogResult)
                    'Await Progress.HideAsync()
                    Dim dialog = New ImportResultDialog(Counters)
                    Await dialog.ShowAsync()
                End If
            End If

        End Function

    End Class

End Namespace
