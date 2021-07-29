Imports DungeonItems.Model

Namespace Global.DungeonItems.ViewModels

    Public Class TypeGroups

        Public Property GroupedElements As New ObservableCollection(Of TypeGroup)

        Public Function GetGroup(type As Item.ItemType) As TypeGroup
            Return GroupedElements.FirstOrDefault(Function(otherGroup) otherGroup.Type.Equals(type))
        End Function

        Public Function GetOrCreateGroup(type As Item.ItemType) As TypeGroup
            Dim match = GetGroup(type)
            If match IsNot Nothing Then
                Return match
            Else
                Dim newGroup As New TypeGroup() With {.Type = type}
                GroupedElements.Add(newGroup)
                Return newGroup
            End If
        End Function

        Public Sub Add(entry As ItemTypeBase)
            GetOrCreateGroup(entry.GetItemType).Add(entry)
        End Sub

        Public Sub Delete(entry As ItemTypeBase)
            GetOrCreateGroup(entry.GetItemType).Remove(entry)
        End Sub

        Public Sub RemoveGroup(type As Item.ItemType)
            Dim g = GetGroup(type)
            If g IsNot Nothing Then
                GroupedElements.Remove(g)
            End If
        End Sub

        Public Sub ApplyFilter(filter As TypeFilterViewModel, collection As ICollection)
            Dim addMelees As Boolean = False
            Dim addArmors As Boolean = False
            Dim addArtilleries As Boolean = False

            If filter.SelectArmor Then
                addArmors = GetGroup(Item.ItemType.Armor) Is Nothing
            Else
                RemoveGroup(Item.ItemType.Armor)
            End If

            If filter.SelectMelee Then
                addMelees = GetGroup(Item.ItemType.Melee) Is Nothing
            Else
                RemoveGroup(Item.ItemType.Melee)
            End If

            If filter.SelectArtillery Then
                addArtilleries = GetGroup(Item.ItemType.Artillery) Is Nothing
            Else
                RemoveGroup(Item.ItemType.Artillery)
            End If

            For Each e In collection
                If filter.visible(e) Then
                    Select Case e.GetItemType
                        Case Item.ItemType.Melee
                            If addMelees Then
                                Add(e)
                            End If
                        Case Item.ItemType.Artillery
                            If addArtilleries Then
                                Add(e)
                            End If
                        Case Item.ItemType.Armor
                            If addArmors Then
                                Add(e)
                            End If
                    End Select
                End If
            Next
        End Sub

    End Class

End Namespace
