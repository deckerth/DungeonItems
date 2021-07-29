Imports DungeonItems.Commands
Imports DungeonItems.Model
Imports DungeonItems.Repository
Imports DungeonItems.Views

Namespace Global.DungeonItems.ViewModels

    Public Class PerksViewModel
        Inherits BindableBase

        Public Property Perks As New ObservableCollection(Of PerkViewModel)
        Public Property DescriptionTextBox As TextBox

        Private Shared _current As PerksViewModel
        Public Shared ReadOnly Property Current As PerksViewModel
            Get
                If _current Is Nothing Then
                    _current = New PerksViewModel()
                End If
                Return _current
            End Get
        End Property

        Public Property AddPerkCommand As RelayCommand
        Public Property DeleteCommand As RelayCommand
        Public Property HomeCommand As RelayCommand
        Public Property NavigateToEchantmentsCommand As RelayCommand

        Public Property DetailFrame As Frame
        Public Property RootFrame As Frame

        Private Sub SaveSelected(item As PerkViewModel)
            If item IsNot Nothing AndAlso item.Modified Then
                item.Save(Repository)
            End If
        End Sub

        Private _selected As PerkViewModel
        Public Property Selected As PerkViewModel
            Get
                Return _selected
            End Get
            Set(value As PerkViewModel)
                If value Is Nothing AndAlso _selected Is Nothing Then
                    Return
                End If
                If value Is Nothing OrElse Not value.Equals(_selected) Then
                    SaveSelected(_selected)
                    _selected = value
                    If _selected IsNot Nothing Then
                        Navigate()
                    End If
                    OnPropertyChanged("Selected")
                End If
            End Set
        End Property

        Public Sub Navigate()
            If DetailFrame IsNot Nothing Then
                If Selected IsNot Nothing Then
                    DetailFrame.Navigate(GetType(PerkPage), Selected)
                Else
                    DetailFrame.Navigate(GetType(EmptyPage), "Bitte wählen Sie eine Eigenschaft aus um sie anzuzeigen, oder fügen Sie neue Eigenschaften hinzu.")
                End If
            End If
        End Sub

        Public Sub Save()
            For Each item In Perks
                If item.Modified Then
                    item.Save(Repository)
                End If
            Next
        End Sub

        Private Repository = PerkRepository.Current

        Public Sub New()
            _current = Me
            Repository.Load()

            For Each i In Repository.Perks
                Perks.Add(New PerkViewModel(i))
            Next

            AddPerkCommand = New RelayCommand(AddressOf AddPerk)
            DeleteCommand = New RelayCommand(AddressOf DeletePerk)
            HomeCommand = New RelayCommand(AddressOf Home)
            NavigateToEchantmentsCommand = New RelayCommand(AddressOf NavigateToEchantments)
        End Sub

        Private Sub AddPerk()
            Dim newItem = New Perk()
            Dim itemVM = New PerkViewModel(newItem)
            itemVM.Modified = True
            Perks.Add(itemVM)
            Selected = itemVM
        End Sub

        Private Sub DeletePerk()
            If Selected IsNot Nothing Then
                Selected.Delete(Repository)
                Perks.Remove(Selected)
                DetailFrame.Navigate(GetType(EmptyPage), "Bitte wählen Sie eine Eigenschaft aus um sie anzuzeigen, oder fügen Sie neue Eigenschaften hinzu.")
            End If
        End Sub

        Private Sub Home()
            SaveSelected(_selected)
            RootFrame.Navigate(GetType(MainPage), ItemsViewModel.Current)
        End Sub

        Private Sub NavigateToEchantments()
            SaveSelected(_selected)
            RootFrame.Navigate(GetType(EnchantmentsEditorPage), EnchantmentsViewModel.Current)
        End Sub

    End Class

End Namespace
