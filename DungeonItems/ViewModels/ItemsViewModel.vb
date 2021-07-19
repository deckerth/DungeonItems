Imports DungeonItems.Commands
Imports DungeonItems.Model
Imports DungeonItems.Repository
Imports DungeonItems.Views

Namespace Global.DungeonItems.ViewModels

    Public Class ItemsViewModel
        Inherits BindableBase

        Private Items As New ObservableCollection(Of ItemViewModel)
        Public Property GroupedItems As New TypeGroups

        Public Shared Property Current As ItemsViewModel

        Public Property AddMeleeCommand As RelayCommand
        Public Property AddArtilleryCommand As RelayCommand
        Public Property AddArmorCommand As RelayCommand
        Public Property DeleteCommand As RelayCommand
        Public Property HomeCommand As RelayCommand
        Public Property NavigateToPerksCommand As RelayCommand
        Public Property NavigateToEnchantmentsCommand As RelayCommand

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

        Private Repository As New ItemRepository

        Public Sub New()
            Current = Me
            Repository.Load()

            For Each i In Repository.Items
                Select Case i.Type
                    Case Item.ItemType.Artillery
                        Me.Items.Add(New ArtilleryViewModel(i))
                End Select
            Next

            AddHandler TypeFilter.UpdateFilter, AddressOf HandleUpdateFilter
            HandleUpdateFilter()

            AddMeleeCommand = New RelayCommand(AddressOf doAddMelee)
            AddArtilleryCommand = New RelayCommand(AddressOf doAddArtillery)
            AddArmorCommand = New RelayCommand(AddressOf doAddArmor)
            DeleteCommand = New RelayCommand(AddressOf doDelete)
            HomeCommand = New RelayCommand(AddressOf displayHomePage)
            NavigateToPerksCommand = New RelayCommand(AddressOf NavigateToPerksPage)
            NavigateToEnchantmentsCommand = New RelayCommand(AddressOf NavigateToEnchantmentsPage)

        End Sub

        Private Sub Navigate()
            If DetailFrame IsNot Nothing AndAlso Selected IsNot Nothing Then
                Select Case Selected.Type
                    Case Item.ItemType.Artillery
                        DetailFrame.Navigate(GetType(ArtilleryPage), Selected)
                End Select
            End If
        End Sub

        Private Sub doAddMelee()
            doAdd(Item.ItemType.Melee)
        End Sub

        Private Sub doAddArtillery()
            doAdd(Item.ItemType.Artillery)
        End Sub

        Private Sub doAddArmor()
            doAdd(Item.ItemType.Armor)
        End Sub

        Private Sub doAdd(type As Item.ItemType)
            Dim newItem = ItemFactory.CreateItem(type)
            Dim itemVM = ItemViewModel.Create(newItem)
            itemVM.Modified = True
            Items.Add(itemVM)
            GroupedItems.Add(itemVM)
            Selected = itemVM
            IsInEdit = True
        End Sub

        Private Sub doDelete()
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

    End Class

End Namespace
