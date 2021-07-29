Imports DungeonItems.Model

Namespace Global.DungeonItems.ViewModels

    Public Class TypeFilterViewModel
        Inherits BindableBase

        Public Event UpdateFilter()

        Private updateFilters As Boolean = True
        Private _selectAll As Boolean = True
        Public Property SelectAll As Boolean
            Get
                Return _selectAll
            End Get
            Set(value As Boolean)
                If value <> _selectAll Then
                    SetProperty(Of Boolean)(_selectAll, value, "SelectAll")
                    If _selectAll Then
                        updateFilters = False
                        SelectArmor = True
                        SelectArtillery = True
                        SelectMelee = True
                        updateFilters = True
                        filter()
                    ElseIf updateFilters Then
                        SelectArmor = False
                        SelectArtillery = False
                        SelectMelee = False
                    End If
                    updateFilters = True
                End If
            End Set
        End Property

        Private _selectMelee As Boolean = True
        Public Property SelectMelee As Boolean
            Get
                Return _selectMelee
            End Get
            Set(value As Boolean)
                If value <> _selectMelee Then
                    SetProperty(Of Boolean)(_selectMelee, value, "SelectMelee")
                    filter()
                    If Not _selectMelee Then
                        updateFilters = False
                        SelectAll = False
                    ElseIf _selectArtillery And _selectArmor Then
                        SelectAll = True
                    End If
                End If
            End Set
        End Property

        Private _selectArtillery As Boolean = True
        Public Property SelectArtillery As Boolean
            Get
                Return _selectArtillery
            End Get
            Set(value As Boolean)
                If value <> _selectArtillery Then
                    SetProperty(Of Boolean)(_selectArtillery, value, "SelectArtillery")
                    filter()
                End If
                If Not _selectArtillery Then
                    updateFilters = False
                    SelectAll = False
                ElseIf _selectMelee And _selectArmor Then
                    SelectAll = True
                End If
            End Set
        End Property

        Private _selectArmor As Boolean = True
        Public Property SelectArmor As Boolean
            Get
                Return _selectArmor
            End Get
            Set(value As Boolean)
                If value <> _selectArmor Then
                    SetProperty(Of Boolean)(_selectArmor, value, "SelectArmor")
                    filter()
                    If Not _selectArmor Then
                        updateFilters = False
                        SelectAll = False
                    ElseIf _selectArtillery And _selectMelee Then
                        SelectAll = True
                    End If
                End If
            End Set
        End Property

        Public Shared Function ItemVisibility(Type As Item.ItemType, selectMelee As Boolean, selectArtillery As Boolean, selectArmor As Boolean) As Visibility
            Dim visible As Boolean
            Select Case Type
                Case Item.ItemType.Melee
                    visible = selectMelee
                Case Item.ItemType.Artillery
                    visible = selectArtillery
                Case Item.ItemType.Armor
                    visible = selectArmor
                Case Item.ItemType.Undefined
                    visible = True
            End Select
            Return If(visible, Visibility.Visible, Visibility.Collapsed)
        End Function

        Public Function visible(entry As ItemTypeBase) As Boolean
            Return entry.GetItemType = Item.ItemType.Armor And SelectArmor OrElse
                entry.GetItemType = Item.ItemType.Artillery And SelectArtillery OrElse
                entry.GetItemType = Item.ItemType.Melee And SelectMelee
        End Function


        Public Sub filter()
            If updateFilters Then
                RaiseEvent UpdateFilter()
            End If
        End Sub

    End Class

End Namespace
