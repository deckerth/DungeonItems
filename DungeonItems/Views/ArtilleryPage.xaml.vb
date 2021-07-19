Imports DungeonItems.Model
Imports DungeonItems.ViewModels

Namespace Global.DungeonItems.Views

    Public NotInheritable Class ArtilleryPage
        Inherits Page

        Public Property ViewModel As ArtilleryViewModel

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            ViewModel = DirectCast(e.Parameter, ArtilleryViewModel)
            ViewModel.DisplayImage = ItemImageDisplay
            ViewModel.EditImage = ItemImageEdit
            Await ViewModel.LoadImageAsync()
        End Sub

        Private Sub PerkChooser_ItemClick(sender As Object, e As ItemClickEventArgs)
            ViewModel.AddPerk(e.ClickedItem)
        End Sub
    End Class

End Namespace

