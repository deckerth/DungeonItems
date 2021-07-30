Imports DungeonItems.Commands
Imports DungeonItems.Model
Imports DungeonItems.Repository
Imports DungeonItems.Views
Imports Microsoft.Toolkit.Uwp.UI
Imports Windows.Storage

Namespace Global.DungeonItems.ViewModels

    Public Class ItemViewModel
        Inherits ItemTypeBase

        Public Property Model As Item

        Public Property AllPerks As AdvancedCollectionView
        Public Property DeletePerkCommand As RelayCommand

        Public Property AllEnchantments As AdvancedCollectionView
        Public Property DeleteEnchantmentCommand As RelayCommand

        Public Property AllRunes As AdvancedCollectionView
        Public Property DeleteRuneCommand As RelayCommand

        Private _selectedPerk As Perk = Nothing
        Public Property SelectedPerk As Perk
            Get
                Return _selectedPerk
            End Get
            Set(value As Perk)
                If value Is Nothing <> (_selectedPerk Is Nothing) Then
                    SetProperty(Of Perk)(_selectedPerk, value, "SelectedPerk")
                ElseIf value IsNot Nothing AndAlso Not value.Equals(_selectedPerk) Then
                    SetProperty(Of Perk)(_selectedPerk, value, "SelectedPerk")
                End If
            End Set
        End Property

        Private _selectedEnchantment As Enchantment = Nothing
        Public Property SelectedEnchantment As Enchantment
            Get
                Return _selectedEnchantment
            End Get
            Set(value As Enchantment)
                If value Is Nothing <> (_selectedEnchantment Is Nothing) Then
                    SetProperty(Of Enchantment)(_selectedEnchantment, value, "SelectedEnchantment")
                ElseIf value IsNot Nothing AndAlso Not value.Equals(_selectedEnchantment) Then
                    SetProperty(Of Enchantment)(_selectedEnchantment, value, "SelectedEnchantment")
                End If
            End Set
        End Property

        Private _selectedRune As Rune = Nothing
        Public Property SelectedRune As Rune
            Get
                Return _selectedRune
            End Get
            Set(value As Rune)
                If value Is Nothing <> (_selectedRune Is Nothing) Then
                    SetProperty(Of Rune)(_selectedRune, value, "SelectedRune")
                ElseIf value IsNot Nothing AndAlso Not value.Equals(_selectedRune) Then
                    SetProperty(Of Rune)(_selectedRune, value, "SelectedRune")
                End If
            End Set
        End Property

        Public Property Modified As Boolean = False

        Private _isInEdit As Boolean = False
        Public Property IsInEdit As Boolean
            Get
                Return _isInEdit
            End Get
            Set(value As Boolean)
                If value <> _isInEdit Then
                    SetProperty(Of Boolean)(_isInEdit, value, "IsInEdit")
                End If
            End Set
        End Property

        Public Property ChangeImageCommand As RelayCommand

        Public Property DisplayImage As Image
        Public Property EditImage As Image

        Public Shared Function Create(model As Item) As ItemViewModel
            Select Case model.Type
                Case Item.ItemType.Artillery
                    Return New ArtilleryViewModel(model)
                Case Item.ItemType.Melee
                    Return New MeleeViewModel(model)
                Case Item.ItemType.Armor
                    Return New ItemViewModel(model)
            End Select
            Return Nothing
        End Function

        Public Sub New(model As Item)
            Me.Model = model
            ChangeImageCommand = New RelayCommand(AddressOf ChangeImage)
            DeletePerkCommand = New RelayCommand(AddressOf DeletePerk)
            DeleteEnchantmentCommand = New RelayCommand(AddressOf DeleteEnchantment)
            DeleteRuneCommand = New RelayCommand(AddressOf DeleteRune)

            AllPerks = New AdvancedCollectionView(PerkRepository.Current.Perks) With {
                .Filter = Function(p As Perk) Not Me.Model.HasPerk(p.Id)
            }
            AllPerks.SortDescriptions.Add(New SortDescription("Description", SortDirection.Ascending))
            AllEnchantments = New AdvancedCollectionView(EnchantmentRepository.Current.Enchantments) With {
                .Filter = Function(e As Enchantment) Not Me.Model.HasEnchantment(e.Id) AndAlso Me.Model.Type = e.Type
            }
            AllEnchantments.SortDescriptions.Add(New SortDescription("Name", SortDirection.Ascending))
            AllRunes = New AdvancedCollectionView(RuneRepository.Current.Runes) With {
                .Filter = Function(e As Rune) Not Me.Model.HasRune(e.Name)
            }
            AllRunes.SortDescriptions.Add(New SortDescription("Name", SortDirection.Ascending))

        End Sub

        Public ReadOnly Property Id As Guid
            Get
                Return Model.Id
            End Get
        End Property

        Public ReadOnly Property Type As Item.ItemType
            Get
                Return Model.Type
            End Get
        End Property

        Public Property Name As String
            Get
                Return Model.Name
            End Get
            Set(value As String)
                If Not value.Equals(Model.Name) Then
                    Model.Name = value
                    OnPropertyChanged("Name")
                    Modified = True
                End If
            End Set
        End Property

        Public Property Description As String
            Get
                Return Model.Description
            End Get
            Set(value As String)
                If Not value.Equals(Model.Description) Then
                    Model.Description = value
                    OnPropertyChanged("Description")
                    Modified = True
                End If
            End Set
        End Property

        Public Property Image As String
            Get
                Return Model.Image
            End Get
            Set(value As String)
                If Not value.Equals(Model.Image) Then
                    Model.Image = value
                    OnPropertyChanged("Image")
                    Modified = True
                End If
            End Set
        End Property

        Public Property mruToken As String
            Get
                Return Model.mruToken
            End Get
            Set(value As String)
                If Not value.Equals(Model.mruToken) Then
                    Model.mruToken = value
                    OnPropertyChanged("mruToken")
                    Modified = True
                End If
            End Set
        End Property

        Public Property IsUnique As Boolean
            Get
                Return Model.IsUnique
            End Get
            Set(value As Boolean)
                If value <> Model.IsUnique Then
                    Model.IsUnique = value
                    OnPropertyChanged("IsUnique")
                    Modified = True
                End If
            End Set
        End Property

        Public Sub AddPerk(toAdd As Perk)
            Model.Perks.Add(toAdd)
            AllPerks.Refresh()
            Modified = True
        End Sub

        Public Sub Save(repository As ItemRepository)
            If Modified Then
                repository.UpdateItem(Model)
                Modified = False
            End If
        End Sub

        Public Sub Delete(repository As ItemRepository)
            repository.DeleteItem(Model)
            Modified = False
        End Sub

        Public Sub DeletePerk()
            If SelectedPerk IsNot Nothing Then
                Model.Perks.Remove(SelectedPerk)
                Modified = True
            End If
        End Sub

        Public Sub AddEnchantment(enchantment As Enchantment)
            Model.Enchantments.Add(enchantment)
            AllEnchantments.Refresh()
            Modified = True
        End Sub

        Public Sub DeleteEnchantment()
            If SelectedEnchantment IsNot Nothing Then
                Model.Enchantments.Remove(SelectedEnchantment)
                Modified = True
            End If
        End Sub

        Public Sub AddRune(rune As Rune)
            Model.Runes.Add(rune)
            AllRunes.Refresh()
            Modified = True
        End Sub

        Public Sub DeleteRune()
            If SelectedRune IsNot Nothing Then
                Model.Runes.Remove(SelectedRune)
                Modified = True
            End If
        End Sub

        Private Async Sub ChangeImage()
            Dim openPicker = New Pickers.FileOpenPicker With {
                .SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary,
                .ViewMode = Pickers.PickerViewMode.Thumbnail
            }

            ' Filter to include a sample subset of file types.
            openPicker.FileTypeFilter.Clear()
            openPicker.FileTypeFilter.Add(".png")
            openPicker.FileTypeFilter.Add(".jpg")

            ' Open the file picker.
            Dim file = Await openPicker.PickSingleFileAsync()

            ' file is null if user cancels the file picker.
            If file IsNot Nothing Then
                Model.Image = file.Path
                Modified = True
                Dim mru = Windows.Storage.AccessCache.StorageApplicationPermissions.MostRecentlyUsedList
                Model.mruToken = mru.Add(file)

                Await LoadImageAsync(file)
            End If

        End Sub

        Public Async Function LoadImageAsync() As Task
            If Not String.IsNullOrEmpty(Model.mruToken) Then
                Try
                    Dim mru = Windows.Storage.AccessCache.StorageApplicationPermissions.MostRecentlyUsedList
                    Dim file As StorageFile = Await mru.GetFileAsync(Model.mruToken)
                    If file IsNot Nothing Then
                        Await LoadImageAsync(file)
                    End If
                Catch ex As Exception
                End Try
            End If

        End Function

        Private Async Function LoadImageAsync(ByVal imageFile As Windows.Storage.StorageFile) As Task

            If imageFile IsNot Nothing Then
                Try
                    ' Open a stream for the selected file.
                    Dim fileStream = Await imageFile.OpenAsync(Windows.Storage.FileAccessMode.Read)

                    ' Set the image source to the selected bitmap.
                    Dim bitmapImage = New Windows.UI.Xaml.Media.Imaging.BitmapImage()

                    bitmapImage.SetSource(fileStream)
                    DisplayImage.Source = bitmapImage
                    EditImage.Source = bitmapImage
                Catch ex As Exception
                End Try
            End If
        End Function

        Public Overrides Function GetItemType() As Item.ItemType
            Return Type
        End Function
    End Class

End Namespace
