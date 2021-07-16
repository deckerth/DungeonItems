Imports DungeonItems.Model
Imports DungeonItems.ViewModels

Namespace Global.DungeonItems.Views

    Public NotInheritable Class PerkEditDialog
        Inherits ContentDialog

        Private OriginalModel As PerkViewModel
        Public Property Model As PerkViewModel
        Public Property Cancelled As Boolean = False

        Public Sub New(model As PerkViewModel)
            OriginalModel = model
            Me.Model = New PerkViewModel(New Perk())
            Me.Model.Description = model.Description
        End Sub

        Private Sub ContentDialog_PrimaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)
            OriginalModel.Description = Model.Description
        End Sub

        Private Sub ContentDialog_SecondaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)
            Cancelled = True
        End Sub
    End Class

End Namespace

