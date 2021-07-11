Imports DungeonItems.Model
Imports DungeonItems.Model.Item
Imports Windows.Storage

Namespace Global.DungeonItems.Repository

    Public Class ItemRepository

        Public Property Items As New ObservableCollection(Of Item)

        Private ContentLoaded As Boolean

        Public Sub Load()

            Dim localSettings = Windows.Storage.ApplicationData.Current.LocalSettings
            Dim itemList = localSettings.CreateContainer("ItemsList", Windows.Storage.ApplicationDataCreateDisposition.Always)

            Items.Clear()

            For Each itemWithPerks In itemList.Containers
                Dim perks As List(Of Perk) = New List(Of Perk)
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
                        Dim itemPerk = New Perk With {
                            .IsUnique = perkComposite("PerkIsUnique"),
                            .Description = perkComposite("Description")
                        }
                        perks.Add(itemPerk)
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
                Dim perkComposite = New Windows.Storage.ApplicationDataCompositeValue()
                perkComposite("IsUnique") = perk.IsUnique
                perkComposite("Description") = perk.Description
                itemWithPerks.Values("PERK" + Guid.NewGuid().ToString()) = perkComposite
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
