Imports DungeonItems.Model

Namespace Global.DungeonItems.ViewModels

    Public MustInherit Class ItemTypeBase
        Inherits BindableBase
        Implements IItemType

        Public MustOverride Function GetItemType() As Item.ItemType Implements IItemType.GetItemType

    End Class

End Namespace
