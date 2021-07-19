Imports DungeonItems.Commands
Imports DungeonItems.Model
Imports DungeonItems.Repository
Imports DungeonItems.Views

Namespace Global.DungeonItems.ViewModels

    Public Class EnchantmentsViewModel
        Inherits BindableBase

        Private Enchantments As New ObservableCollection(Of EnchantmentViewModel)
        Public Property GroupedEnhancements As New TypeGroups

        Private Shared _current As EnchantmentsViewModel
        Public Shared ReadOnly Property Current As EnchantmentsViewModel
            Get
                If _current Is Nothing Then
                    _current = New EnchantmentsViewModel()
                End If
                Return _current
            End Get
        End Property

        Public Property AddArtilleryEchantmentCommand As RelayCommand
        Public Property AddMeleeEchantmentCommand As RelayCommand
        Public Property AddArmorEchantmentCommand As RelayCommand
        Public Property DeleteCommand As RelayCommand
        Public Property HomeCommand As RelayCommand
        Public Property NavigateToPerksCommand As RelayCommand

        Public Property DetailFrame As Frame
        Public Property RootFrame As Frame

        Public Property TypeFilter As TypeFilterViewModel

        Private Sub SaveSelected()
            If _selected IsNot Nothing AndAlso _selected.Modified Then
                _selected.Save(Repository)
            End If
        End Sub

        Private _selected As EnchantmentViewModel
        Public Property Selected As EnchantmentViewModel
            Get
                Return _selected
            End Get
            Set(value As EnchantmentViewModel)
                If value Is Nothing AndAlso _selected Is Nothing Then
                    Return
                End If
                If value Is Nothing OrElse Not value.Equals(_selected) Then
                    SaveSelected()
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
                    DetailFrame.Navigate(GetType(EnchantmentPage), Selected)
                Else
                    DetailFrame.Navigate(GetType(EmptyPage), "Bitte wählen Sie eine Verzauberung aus um sie anzuzeigen, oder fügen Sie neue Verzauberungen hinzu.")
                End If
            End If
        End Sub

        Public Sub Save()
            For Each item In Enchantments
                If item.Modified Then
                    item.Save(Repository)
                End If
            Next
        End Sub

        Private Repository As New EnchantmentRepository


        Public Sub New()
            _current = Me
            Repository.Load()

            For Each i In Repository.Enchantments
                Enchantments.Add(New EnchantmentViewModel(i))
            Next

            TypeFilter = New TypeFilterViewModel
            AddHandler TypeFilter.UpdateFilter, AddressOf HandleUpdateFilter
            HandleUpdateFilter()

            AddArtilleryEchantmentCommand = New RelayCommand(AddressOf AddArtilleryEnchantment)
            AddMeleeEchantmentCommand = New RelayCommand(AddressOf AddMeleeEnchantment)
            AddArmorEchantmentCommand = New RelayCommand(AddressOf AddArmorEnchantment)
            DeleteCommand = New RelayCommand(AddressOf DeleteEnchantment)
            HomeCommand = New RelayCommand(AddressOf Home)
            NavigateToPerksCommand = New RelayCommand(AddressOf NavigateToPerksPage)
        End Sub

        Private Sub AddEnchantment(type As Item.ItemType)
            Dim newItem = New Enchantment(type)
            Dim itemVM = New EnchantmentViewModel(newItem)
            itemVM.Modified = True
            Enchantments.Add(itemVM)
            GroupedEnhancements.Add(itemVM)
            Selected = itemVM
        End Sub

        Private Sub AddArtilleryEnchantment()
            AddEnchantment(Item.ItemType.Artillery)
        End Sub

        Private Sub AddMeleeEnchantment()
            AddEnchantment(Item.ItemType.Melee)
        End Sub

        Private Sub AddArmorEnchantment()
            AddEnchantment(Item.ItemType.Armor)
        End Sub

        Private Sub DeleteEnchantment()
            If Selected IsNot Nothing Then
                Selected.Delete(Repository)
                Enchantments.Remove(Selected)
                GroupedEnhancements.Delete(Selected)
                Navigate()
            End If
        End Sub

        Private Sub Home()
            SaveSelected()
            RootFrame.Navigate(GetType(MainPage), ItemsViewModel.Current)
        End Sub

        Private Sub NavigateToPerksPage()
            SaveSelected()
            RootFrame.Navigate(GetType(PerksEditorPage), PerksViewModel.Current)
        End Sub

        Private Sub HandleUpdateFilter()
            GroupedEnhancements.ApplyFilter(TypeFilter, Enchantments)
        End Sub

    End Class

End Namespace
