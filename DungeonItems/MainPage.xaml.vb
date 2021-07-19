' Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

Imports DungeonItems.Model
Imports DungeonItems.Repository
Imports DungeonItems.ViewModels
Imports DungeonItems.Views
''' <summary>
''' Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page

    Public ViewModel As ItemsViewModel

    Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
        MyBase.OnNavigatedTo(e)
        ViewModel = DirectCast(e.Parameter, ItemsViewModel)
        GroupedItemsCVS.Source = ViewModel.GroupedItems.GroupedElements
        ItemsList.ItemsSource = GroupedItemsCVS.View
        ViewModel.DetailFrame = DetailFrame
        ViewModel.RootFrame = Frame
        DetailFrame.Navigate(GetType(BlankPage))
    End Sub

    Private Sub DoNotDeleteButton_Click(sender As Object, e As RoutedEventArgs)
        DeleteItemFlyout.Hide()
    End Sub

    Private Sub DoDeleteButton_Click(sender As Object, e As RoutedEventArgs)
        DeleteItemFlyout.Hide()
    End Sub
End Class
