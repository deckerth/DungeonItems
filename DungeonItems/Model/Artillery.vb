Namespace Global.DungeonItems.Model

    Public Class Artillery
        Inherits Item


        Public Property Force As Double
        Public Property Speed As Double
        Public Property Ammo As Double

        Public Sub New()
            MyBase.New(ItemType.Artillery)
        End Sub

        Public Sub New(Id As Guid)
            MyBase.New(ItemType.Artillery, Id)
        End Sub


    End Class

End Namespace
