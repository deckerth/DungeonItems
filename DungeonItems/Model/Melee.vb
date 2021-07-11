Namespace Global.DungeonItems.Model

    Public Class Melee
        Inherits Item

        Public Property Force As Double
        Public Property Range As Double
        Public Property Speed As Double

        Public Sub New()
            MyBase.New(ItemType.Melee)
        End Sub

        Public Sub New(Id As Guid)
            MyBase.New(ItemType.Melee, Id)
        End Sub

    End Class

End Namespace

