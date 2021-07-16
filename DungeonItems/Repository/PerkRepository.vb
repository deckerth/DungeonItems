Imports DungeonItems.Model
Imports Windows.Storage

Namespace Global.DungeonItems.Repository

    Public Class PerkRepository

        Private Shared _current As PerkRepository
        Public Shared ReadOnly Property Current As PerkRepository
            Get
                If _current Is Nothing Then
                    _current = New PerkRepository
                End If
                Return _current
            End Get
        End Property

        Public Property Perks As New ObservableCollection(Of Perk)

        Private ContentLoaded As Boolean

        Public Sub Reload()
            ContentLoaded = False
            Load()
        End Sub

        Public Sub Load()
            If Not ContentLoaded Then
                Dim localSettings = ApplicationData.Current.LocalSettings
                Dim perkList = localSettings.CreateContainer("PerksList", ApplicationDataCreateDisposition.Always)

                Perks.Clear()

                For Each perk In perkList.Values
                    Dim id As Guid
                    Dim descr As String
                    Dim perkComposite As ApplicationDataCompositeValue

                    perkComposite = perk.Value
                    id = perkComposite("Id")
                    descr = perkComposite("Description")
                    Perks.Add(New Perk(id) With {.Description = descr})
                Next
                ContentLoaded = True

            End If
        End Sub

        Public Function GetPerk(id As Guid) As Perk
            Return Perks.FirstOrDefault(Function(other) other.Id = id)
        End Function

        Public Sub AddPerk(toAdd As Perk)

            If Not ContentLoaded Then
                Load()
            End If

            Perks.Add(toAdd)

            Dim localSettings = Windows.Storage.ApplicationData.Current.LocalSettings
            Dim perkList = localSettings.CreateContainer("PerksList", Windows.Storage.ApplicationDataCreateDisposition.Always)
            Dim perkComposite = New Windows.Storage.ApplicationDataCompositeValue()
            perkComposite("Id") = toAdd.Id
            perkComposite("Description") = toAdd.Description
            perkList.Values(toAdd.Id.ToString()) = perkComposite
        End Sub

        Public Sub DeletePerk(toDelete As Perk)

            If Not ContentLoaded Then
                Load()
            End If

            If toDelete IsNot Nothing Then
                Perks.Remove(toDelete)
            End If

            Dim localSettings = Windows.Storage.ApplicationData.Current.LocalSettings
            Dim perkList = localSettings.CreateContainer("PerksList", Windows.Storage.ApplicationDataCreateDisposition.Always)
            Dim index As String = Nothing
            For Each item In perkList.Values
                If item.Key = toDelete.Id.ToString Then
                    index = item.Key
                    Exit For
                End If
            Next
            If index IsNot Nothing Then
                perkList.Values.Remove(index)
            End If

        End Sub

        Public Sub UpdatePerk(newValue As Perk)
            DeletePerk(newValue)
            AddPerk(newValue)
        End Sub

    End Class

End Namespace
