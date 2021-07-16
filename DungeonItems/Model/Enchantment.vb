Namespace Global.DungeonItems.Model

    Public Class Enchantment
        Implements IItemType

        Private _Id As Guid
        Public ReadOnly Property Id As Guid
            Get
                Return _Id
            End Get
        End Property

        Private _Type As Item.ItemType
        Public ReadOnly Property Type As Item.ItemType
            Get
                Return _Type
            End Get
        End Property

        Public ReadOnly Property TypeString As String
            Get
                Return Item.GetStringFromType(Type)
            End Get
        End Property

        Public Property Name As String
        Public Property Description As String
        Public Property Image As String
        Public Property mruToken As String

        Public Sub New(Type As Item.ItemType, Id As Guid)
            _Id = Id
            _Type = Type
        End Sub

        Public Sub New(Type As Item.ItemType)
            _Id = Guid.NewGuid()
            _Type = Type
        End Sub

        Public Function GetItemType() As Item.ItemType Implements IItemType.GetItemType
            Return _Type
        End Function
    End Class

End Namespace


