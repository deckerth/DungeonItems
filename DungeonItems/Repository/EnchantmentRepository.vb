Imports DungeonItems.Model
Imports DungeonItems.ViewModels
Imports Windows.Storage

Namespace Global.DungeonItems.Repository

    Public Class EnchantmentRepository

        Private Shared _current As EnchantmentRepository
        Public Shared ReadOnly Property Current As EnchantmentRepository
            Get
                If _current Is Nothing Then
                    _current = New EnchantmentRepository
                End If
                Return _current
            End Get
        End Property

        Public Property Enchantments As New ObservableCollection(Of Enchantment)

        Private ContentLoaded As Boolean

        Public Sub Reload()
            ContentLoaded = False
            Load()
        End Sub

        Public Sub Load()
            If Not ContentLoaded Then
                Dim localSettings = ApplicationData.Current.LocalSettings
                Dim enchantmentList = localSettings.CreateContainer("EnchantmentsList", ApplicationDataCreateDisposition.Always)

                Enchantments.Clear()

                For Each enchantment In enchantmentList.Values
                    Dim id As Guid
                    Dim name As String
                    Dim descr As String
                    Dim image As String
                    Dim mruToken As String
                    Dim typeString As String
                    Dim type As Item.ItemType

                    Dim enchantmentComposite As ApplicationDataCompositeValue

                    enchantmentComposite = enchantment.Value
                    id = enchantmentComposite("Id")
                    typeString = enchantmentComposite("Type")
                    type = Item.GetTypeFromString(typeString)
                    name = enchantmentComposite("Name")
                    descr = enchantmentComposite("Description")
                    image = enchantmentComposite("Image")
                    mruToken = enchantmentComposite("MRUToken")
                    Enchantments.Add(New Enchantment(type, id) With {.Description = descr, .Image = image, .mruToken = mruToken, .Name = name})
                Next
                ContentLoaded = True
            End If
        End Sub

        Public Function GetEnchantment(id As Guid) As Enchantment
            Return Enchantments.FirstOrDefault(Function(other) other.Id = id)
        End Function

        Public Sub AddEnchantment(toAdd As Enchantment)

            If Not ContentLoaded Then
                Load()
            End If

            Enchantments.Add(toAdd)

            Dim localSettings = Windows.Storage.ApplicationData.Current.LocalSettings
            Dim enchantmentList = localSettings.CreateContainer("EnchantmentsList", Windows.Storage.ApplicationDataCreateDisposition.Always)
            Dim enchantmentComposite = New Windows.Storage.ApplicationDataCompositeValue()
            enchantmentComposite("Id") = toAdd.Id
            enchantmentComposite("Type") = toAdd.TypeString
            enchantmentComposite("Name") = toAdd.Name
            enchantmentComposite("Description") = toAdd.Description
            enchantmentComposite("Image") = toAdd.Image
            enchantmentComposite("MRUToken") = toAdd.mruToken

            enchantmentList.Values(toAdd.Id.ToString()) = enchantmentComposite
        End Sub

        Public Sub DeleteEnchantment(toDelete As Enchantment)

            If Not ContentLoaded Then
                Load()
            End If

            If toDelete IsNot Nothing Then
                Enchantments.Remove(toDelete)
            End If

            Dim localSettings = Windows.Storage.ApplicationData.Current.LocalSettings
            Dim enchantmentList = localSettings.CreateContainer("EnchantmentsList", Windows.Storage.ApplicationDataCreateDisposition.Always)
            Dim index As String = Nothing
            For Each item In enchantmentList.Values
                If item.Key = toDelete.Id.ToString Then
                    index = item.Key
                    Exit For
                End If
            Next
            If index IsNot Nothing Then
                enchantmentList.Values.Remove(index)
            End If

        End Sub

        Public Sub UpdateEnchantment(newValue As Enchantment)
            DeleteEnchantment(newValue)
            AddEnchantment(newValue)
        End Sub

        Public Sub Upsert(e As Enchantment)
            Dim current = GetEnchantment(e.Id)
            If current Is Nothing Then
                AddEnchantment(e)
            Else
                Dim vm = New EnchantmentViewModel(current) With {
                    .Description = e.Description,
                    .Image = e.Image,
                    .mruToken = e.mruToken,
                    .Name = e.Name
                }
                If vm.Modified Then
                    UpdateEnchantment(e)
                End If
            End If
        End Sub

        Private Sub New()
            _current = Me
        End Sub

    End Class

End Namespace
