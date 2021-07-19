Namespace Global.DungeonItems.Model

    Public MustInherit Class Item
        Implements IItemType

        Public Enum ItemType
            Melee
            Armor
            Artillery
            Undefined
        End Enum

        Private _Type As ItemType
        Public ReadOnly Property Type As ItemType
            Get
                Return _Type
            End Get
        End Property

        Public ReadOnly Property TypeString As String
            Get
                Return GetStringFromType(Type)
            End Get
        End Property

        Public Shared Function GetStringFromType(type As ItemType) As String
            Select Case type
                Case ItemType.Melee
                    Return "Nahkampf"
                Case ItemType.Artillery
                    Return "Fernkampf"
                Case ItemType.Armor
                    Return "Rüstung"
                Case ItemType.Undefined
                    Return "Typ auswählen"
            End Select
            Return ""
        End Function


        Public Shared Function GetTypeFromString(typeString As String) As ItemType
            If typeString.Equals("Nahkampf") Then
                Return ItemType.Melee
            ElseIf typeString.Equals("Rüstung") Then
                Return ItemType.Armor
            ElseIf typeString.Equals("Fernkampf") Then
                Return ItemType.Artillery
            End If
            Return ItemType.Undefined
        End Function

        Private _Id As Guid
        Public ReadOnly Property Id As Guid
            Get
                Return _Id
            End Get
        End Property

        Private Shared instanceCounter As Integer = 0

        Public Property Name As String
        Public Property Description As String
        Public Property Image As String
        Public Property mruToken As String
        Public Property Perks As New List(Of Perk)
        Public Property Enchantments As New List(Of Enchantment)
        Public Property Runes As New List(Of Rune)
        Public Property IsUnique As Boolean
        Public Property InstanceCount As Integer

        Public Sub New(Type As ItemType, Id As Guid)
            _Type = Type
            _Id = Id
            InstanceCount = instanceCounter
            instanceCounter += 1
        End Sub

        Public Sub New(Type As ItemType)
            _Type = Type
            _Id = Guid.NewGuid()
            InstanceCount = instanceCounter
            instanceCounter += 1
        End Sub

        Public Function GetItemType() As ItemType Implements IItemType.GetItemType
            Return _Type
        End Function
    End Class

End Namespace
