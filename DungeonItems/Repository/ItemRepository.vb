Imports DungeonItems.Model
Imports DungeonItems.Model.Item
Imports Windows.Storage

Namespace Global.DungeonItems.Repository

    Public Class ItemRepository

        Private Shared _current As ItemRepository
        Public Shared ReadOnly Property Current As ItemRepository
            Get
                If _current Is Nothing Then
                    _current = New ItemRepository
                End If
                Return _current
            End Get
        End Property

        Public Property Items As New ObservableCollection(Of Item)

        Private ContentLoaded As Boolean

        Private Sub New()
            _current = Me
        End Sub

        Public Sub Reload()
            ContentLoaded = False
            Load()
        End Sub

        Public Sub Load()
            If Not ContentLoaded Then
                Dim localSettings = ApplicationData.Current.LocalSettings
                Dim itemList = localSettings.CreateContainer("ItemsList", ApplicationDataCreateDisposition.Always)

                PerkRepository.Current.Load()
                EnchantmentRepository.Current.Load()

                Items.Clear()

                For Each itemWithPerks In itemList.Containers
                    Dim perks As New ObservableCollection(Of Perk)
                    Dim enchantments As New ObservableCollection(Of Enchantment)
                    Dim runes As New ObservableCollection(Of Rune)
                    Dim id As Guid
                    Dim type As String = ""
                    Dim name As String = ""
                    Dim descr As String = ""
                    Dim image As String = ""
                    Dim mruToken As String = ""
                    Dim isUnique As Boolean
                    Dim itemComposite As ApplicationDataCompositeValue

                    For Each entry In itemWithPerks.Value.Values
                        If entry.Key.StartsWith("PERK") Then
                            Dim perkComposite As ApplicationDataCompositeValue = entry.Value
                            Dim itemPerk = PerkRepository.Current.GetPerk(perkComposite("Id"))
                            If itemPerk IsNot Nothing Then
                                perks.Add(itemPerk)
                            End If
                        ElseIf entry.Key.StartsWith("ENCHANTMENT") Then
                            Dim enchantmentComposite As ApplicationDataCompositeValue = entry.Value
                            Dim itemEnchantment = EnchantmentRepository.Current.GetEnchantment(enchantmentComposite("Id"))
                            If itemEnchantment IsNot Nothing Then
                                enchantments.Add(itemEnchantment)
                            End If
                        ElseIf entry.Key.StartsWith("RUNE") Then
                            Dim runeComposite As ApplicationDataCompositeValue = entry.Value
                            Dim itemRune = RuneRepository.Current.GetRune(runeComposite("Name"))
                            If itemRune IsNot Nothing Then
                                runes.Add(itemRune)
                            End If
                        Else
                            itemComposite = entry.Value
                            Try
                                id = itemComposite("Id")
                                type = itemComposite("Type")
                                name = itemComposite("Name")
                                descr = itemComposite("Description")
                                image = itemComposite("Image")
                                mruToken = itemComposite("mruToken")
                                isUnique = itemComposite("IsUnique")
                            Catch ex As Exception
                            End Try
                        End If
                    Next

                    If name <> "" Then
                        Dim itemType As ItemType = Item.GetTypeFromString(type)

                        Dim newItem As Item = ItemFactory.CreateItem(itemType, id)
                        newItem.Name = name
                        newItem.Description = descr
                        newItem.Image = image
                        newItem.mruToken = mruToken
                        newItem.IsUnique = isUnique
                        newItem.Perks = perks
                        newItem.Enchantments = enchantments
                        newItem.Runes = runes

                        Select Case itemType
                            Case ItemType.Melee
                                Dim melee As Melee = DirectCast(newItem, Melee)
                                melee.Force = itemComposite("Force")
                                melee.Range = itemComposite("Range")
                                melee.Speed = itemComposite("Speed")
                            Case ItemType.Artillery
                                Dim artillery As Artillery = DirectCast(newItem, Artillery)
                                artillery.Force = itemComposite("Force")
                                artillery.Ammo = itemComposite("Ammo")
                                artillery.Speed = itemComposite("Speed")
                        End Select

                        Items.Add(newItem)
                    End If
                Next
                ContentLoaded = True
            End If
        End Sub

        Public Sub AddItem(toAdd As Item)

            If Not ContentLoaded Then
                Load()
            End If

            Items.Add(toAdd)

            Dim localSettings = Windows.Storage.ApplicationData.Current.LocalSettings
            Dim itemList = localSettings.CreateContainer("ItemsList", Windows.Storage.ApplicationDataCreateDisposition.Always)
            Dim itemWithPerks = itemList.CreateContainer(toAdd.Id.ToString, Windows.Storage.ApplicationDataCreateDisposition.Always)
            Dim itemComposite = New Windows.Storage.ApplicationDataCompositeValue()
            itemComposite("Id") = toAdd.Id
            itemComposite("Type") = toAdd.TypeString
            itemComposite("Name") = toAdd.Name
            itemComposite("Description") = toAdd.Description
            itemComposite("Image") = toAdd.Image
            itemComposite("mruToken") = toAdd.mruToken
            itemComposite("IsUnique") = toAdd.IsUnique

            Select Case toAdd.Type
                Case ItemType.Melee
                    Dim melee As Melee = DirectCast(toAdd, Melee)
                    itemComposite("Force") = melee.Force
                    itemComposite("Range") = melee.Range
                    itemComposite("Speed") = melee.Speed
                Case ItemType.Artillery
                    Dim artillery As Artillery = DirectCast(toAdd, Artillery)
                    itemComposite("Force") = artillery.Force
                    itemComposite("Ammo") = artillery.Ammo
                    itemComposite("Speed") = artillery.Speed
            End Select

            itemWithPerks.Values(toAdd.Id.ToString()) = itemComposite
            For Each perk In toAdd.Perks
                Dim perkComposite = New ApplicationDataCompositeValue()
                perkComposite("Id") = perk.Id
                itemWithPerks.Values("PERK" + perk.Id.ToString()) = perkComposite
            Next
            For Each enchantment In toAdd.Enchantments
                Dim enchantmentComposite = New ApplicationDataCompositeValue()
                enchantmentComposite("Id") = enchantment.Id
                itemWithPerks.Values("ENCHANTMENT" + enchantment.Id.ToString()) = enchantmentComposite
            Next
            For Each rune In toAdd.Runes
                Dim runeComposite = New ApplicationDataCompositeValue()
                runeComposite("Name") = rune.Name
                itemWithPerks.Values("RUNE" + rune.Name.ToString()) = runeComposite
            Next
        End Sub

        Public Sub DeleteItem(toDelete As Item)

            If Not ContentLoaded Then
                Load()
            End If

            If toDelete IsNot Nothing Then
                Items.Remove(toDelete)
            End If

            Dim localSettings = Windows.Storage.ApplicationData.Current.LocalSettings
            Dim itemList = localSettings.CreateContainer("ItemsList", Windows.Storage.ApplicationDataCreateDisposition.Always)
            Dim index As String = Nothing
            For Each item In itemList.Containers
                If item.Key = toDelete.Id.ToString Then
                    index = item.Key
                    Exit For
                End If
            Next
            If index IsNot Nothing Then
                itemList.DeleteContainer(index)
            End If

        End Sub

        Public Sub UpdateItem(newValue As Item)
            DeleteItem(newValue)
            AddItem(newValue)
        End Sub

    End Class

End Namespace
