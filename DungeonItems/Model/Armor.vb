Namespace Global.DungeonItems.Model

    Public Class Armor
        Inherits Item

        Public Sub New()
            MyBase.New(ItemType.Armor)
        End Sub

        Public Sub New(Id As Guid)
            MyBase.New(ItemType.Armor, Id)
        End Sub

    End Class



End Namespace
